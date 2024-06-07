using System.ComponentModel.DataAnnotations;

namespace Clarity_Crate.Dtos
{
    public class SubjectUpdateDto
    {
        [Required]
        public string? Name { get; set; }
    }
}
