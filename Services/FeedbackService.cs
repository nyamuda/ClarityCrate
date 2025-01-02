using Clarity_Crate.Models;
using Clarity_Crate.Dtos;
using Clarity_Crate.Data;
namespace Clarity_Crate.Services
{
    public class FeedbackService
    {

        private ApplicationDbContext _context;
        public bool IsAddingFeedback { get; set; } = false;




        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        //add feedback

        public async Task<bool> AddFeedback(FeedbackCreateDto feedbackDto)
        {
           try
            {
                IsAddingFeedback = true;
                var feedback = new Feedback
                {
                    Title = feedbackDto.Title,
                    Content = feedbackDto.Content
                };
                _context.Feedback.Add(feedback);
                await _context.SaveChangesAsync();

                IsAddingFeedback = false;

                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


    }
}
