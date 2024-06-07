namespace Clarity_Crate.Models
{
    public class Topic
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<Subject> Subjects { get; set; } = new List<Subject>();


        public List<Definition> Definitions { get; set; } = new List<Definition>();


    }
}
