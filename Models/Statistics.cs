using System.ComponentModel.DataAnnotations;

namespace Clarity_Crate.Models
{
    public class Statistics
    {
        public int Id { get; set; }

        public int NumWordsSummarized { get; set; } = 0;

        public int NumDocumentsProcessed { get; set; } = 0;



    }

   
}
