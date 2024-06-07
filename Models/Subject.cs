namespace Clarity_Crate.Models
{
    public class Subject
    {
        public int Id { get; set; }


        public string? Name { get; set; }

        public IEnumerable<Curriculum> Curriculums { get; set; } = new List<Curriculum>();
        public List<Topic> Topics { get; } = new List<Topic>();

        public List<Definition> Definitions { get; set; } = new List<Definition>();
    }
}
