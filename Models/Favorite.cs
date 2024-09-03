using Clarity_Crate.Data;

namespace Clarity_Crate.Models
{
	public class Favorite<T>
	{
		public int Id { get; set; }

		public string UserId { get; set; }
		public ApplicationUser User { get; set; }

		public int ItemId { get; set; } // ID of the item being favored
		public T? Item { get; set; } // The item itself (could be a Definition, Post, etc.)
	}
}