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

        public string Prompt { get; set; } = string.Empty;

        public bool IsSummarizing { get; set; } = false;

        private readonly RestClient _restClient;

        public SummarizationService()
        {
            _restClient = new RestClient("https://any-jumbo-stone-curlew.anvil.app"); // Base URL
        }

        public async Task GetParagraphSummary(string text, int maxLength, int minLength)
        {
            var request = new RestRequest("summarize", Method.Post) // Endpoint
                .AddHeader("Content-Type", "application/json") // Headers
                .AddJsonBody(new { text = text }); // Request body

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
