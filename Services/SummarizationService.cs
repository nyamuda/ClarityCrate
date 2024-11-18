using Clarity_Crate.Dtos;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace Clarity_Crate.Services

{
    public class SummarizationService
    {

        public string Text { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public bool IsSummarizing { get; set; }=false;
        private HttpClient _httpClient;


        public SummarizationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task GetParagraphSummary(string text, int maxLength, int minLength)
        {

            string Url = "https://clarity-crate-fine-tuned-model.onrender.com/summarize";
            var requestBody = new SummaryRequestDto
            {
                Text = text,
                Max_Length = maxLength,
                Min_Length = minLength
            };

            IsSummarizing = true;

            var response = await _httpClient.PostAsJsonAsync(Url, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadFromJsonAsync<SummaryDto>();
                //the text is in lowercase
                //converting it to sentence case
                string summaryText = body.summary;
                var sentenceRegex = new Regex(@"(^[a-z])|[?!.:,;]\s+(.)", RegexOptions.ExplicitCapture);
                Summary = sentenceRegex.Replace(summaryText.ToLower(), s => s.Value.ToUpper());
                IsSummarizing = false;
            }


          else
            {
                IsSummarizing = false;
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error: {response.StatusCode}, Details: {errorContent}");
                
            }



        }


    }
}
