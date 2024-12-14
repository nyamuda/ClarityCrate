using Clarity_Crate.Dtos;
using System.Globalization;
using System.Text.RegularExpressions;
using RestSharp;

namespace Clarity_Crate.Services
{
    public class SummarizationService
    {
        public string Text { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public string CustomPrompt { get; set; } = string.Empty;

        public string ParagraphPrompt { get; set; } = "Provide a clear and comprehensive summary of the following document, emphasizing its key information: ";

        public string BulletPointsPrompt { get; set; } = "Provide a structured summary of the following content, listing the key points in bullet point format: ";

        public string Prompt { get; set; } = string.Empty;

        public bool IsSummarizing { get; set; } = false;

        public int SummaryPercentage { get; set; } = 40;
        private readonly RestClient _restClient;

        public SummarizationService()
        {
            _restClient = new RestClient("https://any-jumbo-stone-curlew.anvil.app"); // Base URL
        }

        public async Task GetSummary(string text, int maxLength, int minLength)
        {
            var requestBody = new
            {
                text = text,
                prompt="summarize",
                max_length=maxLength,
                min_length=minLength
            };

            var request = new RestRequest("summarize", Method.Post) // Endpoint
                .AddHeader("Content-Type", "application/json") // Headers
                .AddJsonBody(requestBody); // Request body

            try
            {
               
                IsSummarizing = true;


                var response = await _restClient.ExecuteAsync<SummaryDto>(request);

                if (response.IsSuccessful && response.Data != null)
                {
                    // Converting to sentence case if needed
                    Summary = response.Data.summary;
                }
                else
                {
                    var errorContent = response.Content ?? "No details provided.";
                    throw new HttpRequestException($"Error: {response.StatusCode}, Details: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while summarizing: {ex.Message}", ex);
            }
            finally
            {
                IsSummarizing = false;
            }
        }
    }
}
