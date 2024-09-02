using Clarity_Crate.Data;

namespace Clarity_Crate.Models
{
	public class Comment<T>
	{
		public int Id { get; set; }

		public string? Content { get; set; }

		public int UserId { get; set; }


		//the item the comment is for
		public T? Item { get; set; }

	}
}