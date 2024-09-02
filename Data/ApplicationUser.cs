using Clarity_Crate.Models;
using Microsoft.AspNetCore.Identity;

namespace Clarity_Crate.Data
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{

		// Lists to store references to favorite and liked definitions
		public List<DefinitionLike> LikedDefinitions { get; set; } = new List<DefinitionLike>();
		public List<DefinitionFavorite> FavoriteDefinitions { get; set; } = new List<DefinitionFavorite>();

	}

}
