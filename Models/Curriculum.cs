using System.ComponentModel.DataAnnotations;

namespace Clarity_Crate.Models
{
    public class Curriculum
    {
        public int Id { get; set; }


        [Required]
        public string? Name { get; set; }

        public List<Subject> Subjects { get; } = new List<Subject>();

        public List<Definition> Definitions { get; set; } = new List<Definition>();
    }
}
