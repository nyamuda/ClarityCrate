using Clarity_Crate.Data;
using Clarity_Crate.Models;
using Microsoft.EntityFrameworkCore;

namespace Clarity_Crate.Services
{
    public class LevelService
    {

        private readonly ApplicationDbContext _context;

        public List<Level> Levels { get; set; }

        public bool isAddingItem = false;
        public bool isUpdatingItem = false;
        public bool isGettingItems = false;

        public LevelService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get all levels
        public async Task GetLevels()
        {
            isGettingItems = !isGettingItems;
            Levels = await _context.Level.ToListAsync();
            isGettingItems = !isGettingItems;
        }

        //Get a level by id
        public async Task<Level?> GetLevel(int id)
        {
            var level = await _context.Level.FirstOrDefaultAsync(l => l.Id == id);

            return level;
        }

        //Update a level
        public async Task<Boolean> UpdateLevel(int id, Level level)
        {
            isUpdatingItem = !isUpdatingItem;
            var itemExists = await _context.Level.FindAsync(id);

            if (itemExists == null)
            {
                isUpdatingItem = !isUpdatingItem;
                return false;
            }

            try
            {
                isUpdatingItem = !isUpdatingItem;
                itemExists.Name = level.Name;
                itemExists.Terms = level.Terms;

                await _context.SaveChangesAsync();

                return true;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                isUpdatingItem = !isUpdatingItem;
                return false;
            }
        }

        //Add a level
        public async Task<Boolean> AddLevel(Level level)
        {
            isAddingItem = !isAddingItem;
            try
            {
                _context.Level.Add(level);
                await _context.SaveChangesAsync();

                isAddingItem = !isAddingItem;

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                isAddingItem = !isAddingItem;
                return false;
            }
        }

        //Delete a level
        public async Task<Boolean> DeleteLevel(int id)
        {
            var level = await _context.Level.FindAsync(id);

            if (level == null)
            {
                return false;
            }

            try
            {
                _context.Level.Remove(level);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return false;
            }
        }
    }
}
