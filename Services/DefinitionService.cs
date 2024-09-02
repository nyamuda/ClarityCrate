using Clarity_Crate.Data;
using Clarity_Crate.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Extensions;

namespace Clarity_Crate.Services
{
	public class DefinitionService
	{
		private readonly ApplicationDbContext _context;


		public DefinitionService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task ToggleLikeAsync(int userId, int definitionId)
		{
			//first, get the user who made the like
			var user = await _context.Users.Include(u => u.LikedDefinitions).FirstOrDefaultAsync(u => u.Id == userId.ToString());

			//second,get the liked definition
			var definition = await _context.Definition.FirstOrDefaultAsync(d => d.Id == definitionId);

			if (user == null || definition == null)
				throw new InvalidOperationException("User or Definition not found.");

			//third, check if the like for that definition already exists
			var existingLike = await _context.DefinitionLike.FirstOrDefaultAsync(dl => dl.UserId == userId && dl.ItemId == definitionId);

			//if the like for that definition does not exist, then create a new one
			if (existingLike == null)
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
			}

			await _context.SaveChangesAsync();
		}
	}
}