using System.Data;
using Clarity_Crate.Data;
using Clarity_Crate.Models;
using Microsoft.EntityFrameworkCore;

namespace Clarity_Crate.Services
{
	public class TermService
	{
		private readonly ApplicationDbContext _context;
		public List<Term> Terms { get; set; } = new List<Term>();
		public bool isCreating = false;
		public bool isGettingItems = false;
		public bool isSearching = false;
		//page size for pagination
		//Pagination Information
		private const int PAGE_SIZE = 9;

		public TermService(ApplicationDbContext context)
		{
			_context = context;
		}

		//CREATE
		public async Task CreateTerm(Term term, Definition definition)
		{
			try
			{
				isCreating = !isCreating;

				//first, check if the term with that name doesn't already exist
				var termExist = await _context.Term.Where(t => t.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

				//if the term with that name already exists,
				//then just add the new definition
				if (termExist is not null)
				{
					termExist.Definitions.Add(definition);

					await _context.SaveChangesAsync();
					isCreating = !isCreating;
				}

				else
				{
					
					//save the term and get the id
					_context.Add(term);
					await _context.SaveChangesAsync();
					var termId = term.Id;


					//add the term id to the definition
					definition.TermId = termId;

					//save the definition
					_context.Definition.Add(definition);
					await _context.SaveChangesAsync();

					isCreating = !isCreating;
				}

			}
			catch (DBConcurrencyException ex)
			{


			}


		}
		//READ
		public async Task GetTerms()
		{
			isGettingItems = !isGettingItems;
			//get all terms with their definitions and levels
			Terms = await _context.Term
				.Include(t => t.Definitions)
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
			try
			{
				_context.Update(term);
				await _context.SaveChangesAsync();
			}

			catch (DBConcurrencyException ex)
			{


			}

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
		public async Task<PaginationInfo<Term>> FilterTerms(string searchTerm, int curriculumId, int subjectId, int topicId, int levelId, int pageNumber)
		{

			isSearching = !isSearching;
			//get all terms with their definitions and levels
			//that match the search term, curriculum, subject, topic, and level
			// while keeping in mind that the search term can be empty
			var terms = await _context.Term
				.Include(t => t.Definitions)
				.Include(t => t.Levels)
				.Where(t => t.Name.Contains(searchTerm) || searchTerm == "")
				.Where(t => t.Definitions.Any(d => d.CurriculumId == curriculumId || curriculumId == 0))
				.Where(t => t.Definitions.Any(s => s.SubjectId == subjectId || subjectId == 0))
				.Where(t => t.Definitions.Any(t => t.TopicId == topicId || topicId == 0))
				.Where(t => t.Levels.Any(l => l.Id == levelId) || levelId == 0)
				.ToListAsync();


			//total filtered items
			int totalItems = terms.Count();


			// Get the terms for the current page
			var currentPageTerms = terms.Skip((pageNumber - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();


			isSearching = !isSearching;


			//PAGINATION INFO    

			//calculate the total number of pages
			int totalPages = (int)Math.Ceiling((double)totalItems / PAGE_SIZE);

			//return pagination information including the terms
			var paginationInfo = new PaginationInfo<Term>
			{
				PageSize = PAGE_SIZE,
				PageNumber = pageNumber,
				TotalPages = totalPages,
				Items = currentPageTerms
			};


			return paginationInfo;


		}

		//Search a term by name
		/*
		public async Task<PaginationInfo<Term>> SearchTerm(string term, int pageNumber)
		{

			isSearching = !isSearching;



			//total items
			int totalItems = await _context.Term.Where(t => t.Name.Contains(term) || term == "").CountAsync();

			// Get the terms for the current page
			var terms = await _context.Term
				.Include(t => t.Definition)
				.Include(t => t.Levels)
				.Where(t => t.Name.Contains(term) || term == "")
				.Skip((pageNumber - 1) * PAGE_SIZE)
				.Take(PAGE_SIZE)
				.ToListAsync();

			isSearching = !isSearching;

			//PAGINATION INFO    

			//calculate the total number of pages
			int totalPages = (int)Math.Ceiling((double)totalItems / PAGE_SIZE);

			//return pagination information including the terms
			var paginationInfo = new PaginationInfo<Term>
			{
				PageSize = PAGE_SIZE,
				PageNumber = pageNumber,
				TotalPages = totalPages,
				Items = terms
			};


			return paginationInfo;
		}
		*/

	}
}
