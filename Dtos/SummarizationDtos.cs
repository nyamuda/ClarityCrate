using System.Text.Json.Serialization;

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

        [JsonPropertyName("max_length")]
        public int Max_Length { get; set; }

        [JsonPropertyName("min_length")]
        public int Min_Length { get; set; }
    }
}
