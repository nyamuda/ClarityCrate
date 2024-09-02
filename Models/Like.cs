using Clarity_Crate.Data;

namespace Clarity_Crate.Models
{
	public class Like<T>
	{
		public int Id { get; set; }
		public ApplicationUser User { get; set; }
		public int UserId { get; set; }

		//the item the like is for

		public int ItemId { get; set; } // ID of the item being liked
		public T? Item { get; set; }

	}
}