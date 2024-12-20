using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace Clarity_Crate.Dtos
{
    public class SummaryDto
{

        public string summary { get; set; }
}

    public class SummaryRequestDto
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        /*
        [JsonPropertyName("max_length")]
        public int Max_Length { get; set; }

        [JsonPropertyName("min_length")]
        public int Min_Length { get; set; }*/
    }

    public class SummaryResponseDto
    {
        [JsonPropertyName("summary_text")]
        public string Summary { get; set; }
    }

    public class SummaryFeedbackDto
    {
        [Required(ErrorMessage ="Required")]
        public string Content { get; set; }
    }
}
