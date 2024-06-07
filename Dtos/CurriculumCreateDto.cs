using System.ComponentModel.DataAnnotations;

namespace Clarity_Crate.Dtos
{
    public class CurriculumCreateDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
