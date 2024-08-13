using Clarity_Crate.Data;
using Clarity_Crate.Models;
using Microsoft.EntityFrameworkCore;

namespace Clarity_Crate.Services
{
    public class CurriculumService
    {
        private readonly ApplicationDbContext _context;


        public bool isCreating = false;
        public bool isUpdating = false;
        public bool isGettingItems = false;

        public List<Curriculum> Curriculums { get; set; }


        public CurriculumService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Curriculum?> GetCurriculum(int id)
        {
            //it must include the subjects
            var curriculum = await _context.Curriculum.Include(c => c.Subjects).FirstOrDefaultAsync(c => c.Id == id);

            return curriculum;
        }

        public async Task GetCurricula()
        {
            isGettingItems = !isGettingItems;
            //they must include the subjects
            var curricula = await _context.Curriculum.Include(c => c.Subjects).ToListAsync();

            Curriculums = curricula;
            isGettingItems = !isGettingItems;
        }

        public async Task<Boolean> UpdateCurriculum(int id, Curriculum curriculum)
        {
            isUpdating = !isUpdating;
            var itemExists = await _context.Curriculum.FindAsync(id);

            if (itemExists == null)
            {
                return false;
            }

            try
            {
                isUpdating = !isUpdating;

                itemExists.Name = curriculum.Name;

                await _context.SaveChangesAsync();

                return true;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                isUpdating = !isUpdating;
                throw;
            }
        }

        public async Task PostCurriculum(Curriculum curriculum)
        {
            isCreating = !isCreating;

            _context.Curriculum.Add(curriculum);

            await _context.SaveChangesAsync();

            isCreating = !isCreating;
        }

        public async Task<Boolean> DeleteCurriculum(int id)
        {
            var itemExists = await _context.Curriculum.FindAsync(id);

            if (itemExists == null)
            {
                return false;
            }

            _context.Curriculum.Remove(itemExists);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
