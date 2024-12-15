using Clarity_Crate.Dtos;
using System.Globalization;
using System.Text.RegularExpressions;
using RestSharp;
using Microsoft.EntityFrameworkCore;
using Clarity_Crate.Data;
using Clarity_Crate.Models;

namespace Clarity_Crate.Services
{
    public class SummarizationService
    {
        public string Text { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public string CustomPrompt { get; set; } = string.Empty;

        public string ParagraphPrompt { get; set; } = "Provide a clear and comprehensive summary of the following document, emphasizing its key information";

        public string BulletPointsPrompt { get; set; } = "Provide a structured summary of the following content, listing the key points in bullet point format";

        public string Prompt { get; set; } = string.Empty;

        public bool IsSummarizing { get; set; } = false;

        public int SummaryPercentage { get; set; } = 40;
        public int TotalDocumentsSummarized { get; set; } = 0;
        public int TotalWordsSummarized { get; set; } = 0;


        private readonly RestClient _restClient;

        private readonly ApplicationDbContext _context;
        private readonly AppService _appService;

        public SummarizationService(ApplicationDbContext context, AppService appService)
        {
            _restClient = new RestClient("https://any-jumbo-stone-curlew.anvil.app"); // Base URL
            _context = context;
            _appService = appService;
        }

        public async Task GetSummary(string text, int maxLength, int minLength)
        {
            var requestBody = new
            {
                text = text,
                prompt=Prompt,
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
                    //get the summary
                    Summary = response.Data.summary;
                    IsSummarizing = false;

                    //save the number of words summarized to the database
                    await SaveWordCount(text);
                   

                }
                else
                {
                    IsSummarizing = false;
                    //show error message snack bar
                    _appService.ShowSnackBar("An error occurred while summarizing", false);

                    var errorContent = response.Content ?? "No details provided.";
                    throw new HttpRequestException($"Error: {response.StatusCode}, Details: {errorContent}");


                }
            }
            catch (Exception ex)
            {
                IsSummarizing = false;
                //show error message snack bar
                _appService.ShowSnackBar("An error occurred while summarizing", false);

                throw new Exception($"An error occurred while summarizing: {ex.Message}", ex);
            }
            
        }
        //Count the number of words of text
        private int WordCount(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? 0 : text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        }

        //Save the number of words summarized
        private async Task SaveWordCount(string text)
        {
            int numWordsSummarized = WordCount(text);

            var summary = await _context.Summary.FirstOrDefaultAsync();
            if (summary != null)
            {
                summary.NumWordsSummarized += numWordsSummarized;

                _context.Summary.Update(summary);
                await _context.SaveChangesAsync();
            }
            else
            {
                var item = new Summary
                {
                    NumWordsSummarized = numWordsSummarized
                };
                _context.Summary.Add(item);
                await _context.SaveChangesAsync();
            }

        }
        //Get the total number of words and documents summarized so far
        public async Task GetSummaryInfo()
        {
            var summary = await _context.Summary.FirstOrDefaultAsync();
            if (summary != null)
            {
                TotalWordsSummarized= summary.NumWordsSummarized;
                TotalDocumentsSummarized = summary.NumDocumentsSummarized;

            }
        }

        //Format summary info
        //such as total number of words or documents summarized
        public string FormatNumber(int number)
        {
            if (number >= 1_000_000) // Handle millions
            {
                int roundedValue = (number / 100_000) * 100_000; // Round to the nearest 100K
                return $"{roundedValue / 1_000_000}M";
            }
            else if (number >= 1_000) // Handle thousands
            {
                int roundedValue = (number / 100) * 100; // Round to the nearest 100
                return roundedValue >= 10_000 ? $"{roundedValue / 1_000}K" : $"{roundedValue / 1_000}+";
            }
            else // Handle smaller numbers
            {
                return number.ToString();
            }
        }
    }
}
