using Clarity_Crate.Dtos;
using System.Globalization;
using System.Text.RegularExpressions;
using RestSharp;
using Microsoft.EntityFrameworkCore;
using Clarity_Crate.Data;
using Clarity_Crate.Models;
using MudBlazor.Charts;
using Azure;

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
        public bool IsSendingFeedback { get; set; } = false;

        public int SummaryPercentage { get; set; } = 40;
        public string TotalDocumentsSummarized { get; set; } = "0";
        public string TotalWordsSummarized { get; set; } = "0";

        private string? _huggingFaceApiKey;


        private readonly RestClient _restClient;

        private readonly ApplicationDbContext _context;
        private readonly AppService _appService;

        public SummarizationService(ApplicationDbContext context, AppService appService, IConfiguration configuration)
        {
            _restClient = new RestClient("https://any-jumbo-stone-curlew.anvil.app"); // Base URL
            _context = context;
            _appService = appService;
            _huggingFaceApiKey = configuration["Authentication:HuggingFace:ApiKey"];


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
           
          try
            {

                var summary = await _context.Summary.FirstOrDefaultAsync();
                if (summary != null)
                {

                    var words = summary.NumWordsSummarized;
                    var documents = summary.NumDocumentsSummarized;



                    //format the numbers
                    TotalWordsSummarized = FormatNumber(words);
                    TotalDocumentsSummarized = FormatNumber(documents);



                }
                else
                {
                    Console.WriteLine("summary is empty");
                }

            }
            catch(Exception e)
            {
                Console.WriteLine("Error while getting summary info");
                Console.WriteLine(e);

                TotalWordsSummarized = FormatNumber(1000);
                TotalDocumentsSummarized = FormatNumber(50);

            }
        }

        //Format summary info
        //such as total number of words or documents summarized
        public string FormatNumber(int value)
        {
            //to display the total words/documents with +1
            //e.g +10 Words Summarized -- if the total number of words summarized is 11
            int number = value - 1;


            // Handle millions
            if (number >= 1_000_000) 
            {
                int roundedValue = (number / 100_000) * 100_000; // Round to the nearest 100K
                return $"{roundedValue / 1_000_000}M";
            }
            // Handle thousands
            else if (number >= 1_000) 
            {
                int roundedValue = (number / 100) * 100; // Round to the nearest 100
                return roundedValue >= 10_000 ? $"{roundedValue / 1_000}K+" : $"{roundedValue / 1_000}K+";
            }
            // Handle smaller numbers
            else
            {
                var formattedNum = $"{number}+";
                return formattedNum;
            }
        }
        public async Task SummarizeAsync(string text)
        {
            const string ApiUrl = "https://api-inference.huggingface.co/models/nyamuda/summasphere"; // Replace with your summarization model URL

            try
            {
                IsSummarizing = true;

                // Create a RestClient
                var client = new RestClient(ApiUrl);

                // Create a RestRequest
                var request = new RestRequest()
                    .AddHeader("Authorization", $"Bearer {_huggingFaceApiKey}")
                    .AddHeader("Content-Type", "application/json");

                // Set the request body
                var inputs = $"{Prompt}: {text}";
                var requestBody = new
                {
                    inputs = inputs
                };

                request.AddJsonBody(requestBody);

                // Retry logic for model loading
                const int maxRetries = 5;
                const int delaySeconds = 5; // Wait for 5 seconds between retries
                int attempt = 0;

                while (attempt < maxRetries)
                {
                    // Execute the request
                    var response = await client.ExecutePostAsync<List<SummaryResponseDto>>(request);

                    if (response.IsSuccessful && response.Data != null)
                    {
                        // Successful response
                        Summary = response.Data[0].Summary;

                        // Save the number of words summarized to the database
                        await SaveWordCount(text);

                        IsSummarizing = false;
                        return;
                    }
                    else if (response.Content?.Contains("currently loading") == true)
                    {
                        // Model is still loading; wait and retry                      
                        _appService.ShowSnackBar(message:$"Model is loading. Retrying in {delaySeconds} seconds...",severity:"normal",position:"top-right");
                        await Task.Delay(delaySeconds * 1000);
                    }
                    else
                    {
                        // Handle other errors
                        IsSummarizing = false;
                        

                        var errorContent = response.Content ?? "No details provided.";
                        throw new HttpRequestException($"Error: {response.StatusCode}, Details: {errorContent}");
                    }

                    attempt++;
                }

                // If retries are exhausted
                IsSummarizing = false;
                _appService.ShowSnackBar(message:"Model loading took too long. Please try again later.", severity:"error");
            }
            catch (Exception ex)
            {
                IsSummarizing = false;
                _appService.ShowSnackBar(message: "Temporary issue. Please try again later.", severity: "warning");
                //throw new Exception($"An error occurred while summarizing: {ex.Message}", ex);
            }
        }
        


        public async Task SendFeedback(string feedbackContent)
        {
            IsSendingFeedback = true;
            // Check if summary exists
            var summary = await _context.Summary.FirstOrDefaultAsync();

            if (summary != null)
            {
                summary.Feedback.Add(feedbackContent);
                // Save changes to the database
                await _context.SaveChangesAsync();

                IsSendingFeedback = false;

                // show snack bar
                _appService.ShowSnackBar(message: "Thanks for your feedback! We’ll use it to make improvements.", position:"bottom-left",severity:"normal");

            }

            else
            {
                IsSendingFeedback = false;
                // show snack bar
                _appService.ShowSnackBar(message:"Sorry, we encountered an issue while processing your feedback. Please try again.", severity:"error");

            }

        }
    }
}
