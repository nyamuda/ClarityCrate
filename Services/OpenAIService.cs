
using OpenAI_API;
using OpenAI_API.Models;

namespace Clarity_Crate.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly OpenAIAPI _api = new OpenAIAPI("");

        public OpenAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }

        public async Task<(string role, string content)> GetAssistantResponse(string prompt)
        {
            try
            {
                var chat = _api.Chat.CreateConversation();
                chat.Model = Model.ChatGPTTurbo;
                chat.RequestParameters.Temperature = 0.5;

                //System instructions
                chat.AppendSystemMessage("You're a high school physics instructor who helps high school students with physics problems.");

                //user
                chat.AppendUserInput(prompt);
                string response = await chat.GetResponseFromChatbotAsync();

                return ("assistant", response);
            }
            catch (HttpRequestException e)
            {
                throw e;
                return ("error", "sorry, there was an error");
            }
        }
    }
}
