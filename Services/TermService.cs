using Clarity_Crate.Data;
using Clarity_Crate.Models;
using Microsoft.EntityFrameworkCore;

namespace Clarity_Crate.Services
{
    public class TermService
    {
        private readonly ApplicationDbContext _context;
        public List<Term> Terms { get; set; } = new List<Term>();
        public bool isProcessing = false;
        public bool isGettingItems = false;
        public bool isSearching = false;

        public TermService(ApplicationDbContext context)
        {
            _context = context;
        }

        //CREATE
        public async Task CreateTerm(Term term, Definition definition)
        {
            isProcessing = !isProcessing;

            //first save the term and add the saved term to the definition
            //save the term and get the id
            _context.Add(term);
            await _context.SaveChangesAsync();
            var termId = term.Id;


            //add the term id to the definition
            definition.TermId = termId;

            //save the definition
            _context.Definition.Add(definition);
            await _context.SaveChangesAsync();

            isProcessing = !isProcessing;



        }
        //READ
        public async Task GetTerms()
        {
            isGettingItems = !isGettingItems;
            //get all terms with their definitions and levels
            Terms = await _context.Term
                .Include(t => t.Definition)
                .Include(t => t.Levels)
                .ToListAsync();

            isGettingItems = !isGettingItems;
        }

        public async Task<Term?> GetTermById(int id)
        {
            return await _context.Term.FirstOrDefaultAsync(t => t.Id == id);
        }

        //UPDATE
        public async Task UpdateTerm(Term term)
        {
            _context.Update(term);
            await _context.SaveChangesAsync();
        }

        //DELETE
        public async Task<Boolean> DeleteTerm(int id)
        {
            var term = await GetTermById(id);
            if (term != null)
            {
                _context.Remove(term);
                await _context.SaveChangesAsync();

                //remove all definitions associated with the term
                var definitions = await _context.Definition.Where(d => d.TermId == id).ToListAsync();
                foreach (var definition in definitions)
                {
                    _context.Remove(definition);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            return false;


        }

        //Search for a term`
        //for a particular curriculum, subject, topic, and level
        public async Task<List<Term>> FilterTerms(string searchTerm, int curriculumId, int subjectId, int topicId, int levelId)
        {
            isSearching = !isSearching;
            //get all terms with their definitions and levels
            //that match the search term, curriculum, subject, topic, and level
            //keeping in mind that the search term can be empty
            var terms = await _context.Term
                .Include(t => t.Definition)
                .Include(t => t.Levels)
                .Where(t => t.Name.Contains(searchTerm) || searchTerm == "")
                .Where(t => t.Definition.CurriculumId == curriculumId || curriculumId == 0)
                .Where(t => t.Definition.SubjectId == subjectId || subjectId == 0)
                .Where(t => t.Definition.TopicId == topicId || topicId == 0)
                .Where(t => t.Levels.Any(l => l.Id == levelId) || levelId == 0)
                .ToListAsync();

            isSearching = !isSearching;
            return terms;


        }

        public async Task<List<Term>> SearchTerm(string term)
        {
            isSearching = !isSearching;

            var terms = await _context.Term
                .Include(t => t.Definition)
                .Include(t => t.Levels)
                .Where(t => t.Name.Contains(term) || term == "")
                .ToListAsync();

            isSearching = !isSearching;
            return terms;
        }

    }
}
