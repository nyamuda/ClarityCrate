namespace Clarity_Crate.Models
{
	public class Term
	{

		public int Id { get; set; }

		public string? Name { get; set; }


		public List<Definition> Definitions { get; set; } = new List<Definition>();

		public IEnumerable<Level> Levels { get; set; } = new List<Level>();


	}


}
