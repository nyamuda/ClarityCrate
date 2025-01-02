using System.ComponentModel.DataAnnotations;
namespace Clarity_Crate.Dtos

{
    public class FeedbackCreateDto
{
      
        public string Title { get; set; }
        [Required(ErrorMessage = "Field is required.")]
        public string Content { get; set; }
    }
}
