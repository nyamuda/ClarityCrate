using System.ComponentModel.DataAnnotations;

namespace Clarity_Crate.Models
{
    public class Level
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Term> Terms { get; set; } = new List<Term>();
    }
}
