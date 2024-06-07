using System.ComponentModel.DataAnnotations;

namespace Clarity_Crate.Dtos
{
    public class CurriculumUpdateDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
