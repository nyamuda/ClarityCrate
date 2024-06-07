namespace Clarity_Crate.Models
{
    public class Definition
    {
        public int Id { get; set; }
        public string? Content { get; set; }

        public int TopicId { get; set; }

        public int CurriculumId { get; set; }

        public int SubjectId { get; set; }


        public int TermId { get; set; }

        public Topic? Topic { get; set; }

        public Curriculum? Curriculum { get; set; }

        public Subject? Subject { get; set; }


        public Term? Term { get; set; }

    }
}
