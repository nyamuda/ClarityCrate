using Clarity_Crate.Data;

namespace Clarity_Crate.Models
{
	public class Definition
	{
		public int Id { get; set; }
		public string? Content { get; set; }


		public List<string>? Keywords { get; set; }

		public List<Comment<Definition>>? Comments { get; set; }

		public int TopicId { get; set; }

		public int CurriculumId { get; set; }

		public int SubjectId { get; set; }


		public int TermId { get; set; }

		public Topic? Topic { get; set; }

		public Curriculum? Curriculum { get; set; }

		public Subject? Subject { get; set; }

		public Term? Term { get; set; }

		public List<DefinitionLike> Likes { get; set; } = new List<DefinitionLike>();

		public List<DefinitionFavorite> Favorites { get; set; } = new List<DefinitionFavorite>();
	}


	public class DefinitionLike : Like<Definition>
	{

	}

	public class DefinitionFavorite : Favorite<Definition>
	{

	}
}
