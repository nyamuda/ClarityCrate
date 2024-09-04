using Clarity_Crate.Data;
using Clarity_Crate.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Clarity_Crate.Services
{
    public class DefinitionService
    {
        private readonly ApplicationDbContext _context;

        public List<Definition> Definitions { get; set; } = new List<Definition>();

        public bool isSearching = false;
        public bool isCreating = false;
        public bool isGettingItems = false;
        public int Likes { get; set; }
        //page size for pagination
        //Pagination Information
        private const int PAGE_SIZE = 9;

        public DefinitionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task GetDefinitions()
        {
            isGettingItems = !isGettingItems;
            //get all terms with their definitions and levels
            Definitions = await _context.Definition
                .Include(d => d.Levels)
                .Include(d => d.Likes)
                .Include(d => d.Favorites)
                .ToListAsync();

            isGettingItems = !isGettingItems;
        }

        public async Task UpdateDefinition(Definition definition)
        {
            try
            {
                _context.Update(definition);
                await _context.SaveChangesAsync();
            }

            catch (DBConcurrencyException ex)
            {


            }

        }

        public async Task<Definition?> GetDefinitionById(int id)
        {
            return await _context.Definition.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Boolean> DeleteDefinition(int id)
        {
            var definition = await GetDefinitionById(id);
            if (definition != null)
            {
                _context.Remove(definition);
                await _context.SaveChangesAsync();


                return true;
            }
            return false;


        }


        public async Task ToggleLike(string userId, int definitionId, bool hasLiked)
        {
            Console.WriteLine("Like toggle executed");
            Console.WriteLine("Like toggle executed");
            Console.WriteLine("Like toggle executed");
            Console.WriteLine("Like toggle executed");
            Console.WriteLine("Like toggle executed");
            Console.WriteLine("Like toggle executed");
            Console.WriteLine("Like toggle executed");
            //first, get the user who made the like
            var user = await _context.Users.Include(u => u.LikedDefinitions).FirstOrDefaultAsync(u => u.Id == userId);

            //second,get the liked definition
            var definition = await _context.Definition.Include(d => d.Likes).FirstOrDefaultAsync(d => d.Id == definitionId);

            if (user == null || definition == null)
                throw new InvalidOperationException("User or Definition not found.");

            //third, check if the like for that definition already exists
            var existingLike = await _context.DefinitionLike.FirstOrDefaultAsync(dl => dl.UserId == userId && dl.ItemId == definitionId);


            //if the like for that definition does not exist, then create a new one
            if (existingLike == null && hasLiked)
            {

                // Create a new like for the definition
                var like = new DefinitionLike
                {
                    UserId = userId,
                    User = user,
                    ItemId = definitionId,
                    Item = definition
                };

                //add that like to the user
                user.LikedDefinitions.Add(like);

                //add that like to the definition likes list
                definition.Likes.Add(like);

                //save that like
                _context.DefinitionLike.Add(like);
                //save changes
                await _context.SaveChangesAsync();



            }

            //if the like already exists, then remove it
            else
            {

                // remove like from the user
                user.LikedDefinitions.Remove(existingLike);

                //remove like from the definition likes list
                definition.Likes.Remove(existingLike);

                //delete like	
                _context.DefinitionLike.Remove(existingLike);

                //save changes
                await _context.SaveChangesAsync();
            }





        }


        //Get the total number of likes for a definition
        public async Task<int> GetDefinitionLikes(int definitionId)
        {
            var definition = await _context.Definition.Include(d => d.Likes).FirstOrDefaultAsync(d => d.Id == definitionId);

            if (definition is not null)
            {
                return definition.Likes.Count;
            }
            return 0;
        }

        //Check to see if the user has liked the definition or not
        public bool HasUserLiked(string userId, Definition definition)
        {
            //check if the user has liked the definition

            var existingLike = definition.Likes.FirstOrDefault(dl => dl.UserId == userId);

            foreach (var like in definition.Likes)
            {
                Console.WriteLine($"{like.UserId}");
            }

            bool hasLiked = (existingLike is not null) ? true : false;

            return hasLiked;


        }

        //Search for a term`
        //for a particular curriculum, subject, topic, and level
        public async Task<PaginationInfo<Definition>> FilterDefinitions(string searchTerm, int curriculumId, int subjectId, int topicId, int levelId, int pageNumber)
        {

            isSearching = !isSearching;
            //get all terms with their definitions and levels
            //that match the search term, curriculum, subject, topic, and level
            // while keeping in mind that the search term can be empty
            var definitions = await _context.Definition
                .Include(d => d.Levels)
                .Include(d => d.Term)
                .Include(d => d.Likes)
                .Where(d => d.Term.Name.Contains(searchTerm) || searchTerm == "")
                .Where(d => d.CurriculumId == curriculumId || curriculumId == 0)
                .Where(d => d.SubjectId == subjectId || subjectId == 0)
                .Where(d => d.TopicId == topicId || topicId == 0)
                .Where(d => d.Levels.Any(l => l.Id == levelId) || levelId == 0)
                .ToListAsync();


            //total filtered items
            int totalItems = definitions.Count();





            // Get the definitions for the current page
            var currentPageDefinitions = definitions.Skip((pageNumber - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();





            //PAGINATION INFO    

            //calculate the total number of pages
            int totalPages = (int)Math.Ceiling((double)totalItems / PAGE_SIZE);

            //return pagination information including the definitions
            var paginationInfo = new PaginationInfo<Definition>
            {
                PageSize = PAGE_SIZE,
                PageNumber = pageNumber,
                TotalPages = totalPages,
                Items = currentPageDefinitions
            };

            isSearching = !isSearching;
            return paginationInfo;


        }

    }
}