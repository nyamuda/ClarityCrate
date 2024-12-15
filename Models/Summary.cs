using System.ComponentModel.DataAnnotations;

namespace Clarity_Crate.Models
{
    public class Summary
    {
        public int Id { get; set; }

        public int NumWordsSummarized { get; set; } = 0;

        public int NumDocumentsSummarized { get; set; } = 0;


    }
}
