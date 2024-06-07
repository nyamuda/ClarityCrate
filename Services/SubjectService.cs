using Clarity_Crate.Data;
using Clarity_Crate.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Clarity_Crate.Services
{
    public class SubjectService
    {
        private readonly ApplicationDbContext _context;
        public List<Subject> Subjects { get; set; }
        public bool isGettingItems = false;


        public SubjectService(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<bool> CreateSubject(Subject subject)
        {
            try
            {
                _context.Subject.Add(subject);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DBConcurrencyException ex)
            {

                return false;
            }
        }

        public async Task<Subject?> GetSubject(int id)
        {
            var subject = await _context.Subject.Include(s => s.Curriculums).FirstOrDefaultAsync(s => s.Id == id);

            return subject;
        }

        public async Task GetSubjects()
        {
            isGettingItems = !isGettingItems;
            //get subjects with their curriculum
            var subjects = await _context.Subject.Include(s => s.Curriculums).ToListAsync();

            Subjects = subjects;

            isGettingItems = !isGettingItems;
        }

        public async Task<Boolean> UpdateSubject(Subject subject)
        {
            var itemExists = await _context.Subject.FindAsync(subject.Id);

            if (itemExists == null)
            {
                return false;
            }

            try
            {
                itemExists.Name = subject.Name;
                itemExists.Curriculums = subject.Curriculums;


                await _context.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return false;
            }
        }


        public async Task<Boolean> DeleteSubject(int id)
        {
            var itemExists = await _context.Subject.FindAsync(id);

            if (itemExists == null)
            {
                return false;
            }

            _context.Subject.Remove(itemExists);

            await _context.SaveChangesAsync();

            return true;
        }

        //get subjects for a particular curriculum
        public async Task<List<Subject>> FilterSubjectByCurriculum(int curriculumId)
        {
            //keep in mind that the curriculumId can be 0
            var subjects = await _context.Subject.Include(s => s.Curriculums).Where(s => s.Curriculums.Any(c => c.Id == curriculumId) || curriculumId == 0).ToListAsync();

            return subjects;
        }
    }
}
