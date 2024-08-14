using Clarity_Crate.Data;
using Clarity_Crate.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Clarity_Crate.Services
{
    public class TopicService
    {
        private readonly ApplicationDbContext _context;
        public List<Topic> Topics { get; set; } = new List<Topic>();
        public bool isCreating = false;
        public bool isUpdating = false;
        public bool isGettingItems = false;

        public TopicService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task GetTopics()
        {
            isGettingItems = !isGettingItems;
            Topics = await _context.Topic.Include(t => t.Subjects).ToListAsync();
            isGettingItems = !isGettingItems;

        }

        public async Task<Topic?> GetTopicById(int id)
        {
            return await _context.Topic.FindAsync(id);
        }

        public async Task<Boolean> CreateTopic(Topic topic)
        {
            try
            {
                isCreating = true;
                _context.Topic.Add(topic);
                await _context.SaveChangesAsync();
                isCreating = false;
                return true;
            }
            catch (DBConcurrencyException ex)
            {
                isCreating = false;
                return false;
            }
        }


        public async Task<Boolean> UpdateTopic(Topic topic)
        {
            try
            {
                isUpdating = true;
                Topic topicExists = await _context.Topic.FindAsync(topic.Id);
                if (topicExists != null)
                {
                    topicExists.Name = topic.Name;
                    topicExists.Subjects = topic.Subjects;
                    await _context.SaveChangesAsync();
                    isUpdating = false;
                    return true;
                }

                return false;
            }
            catch (DBConcurrencyException e)
            {
                isUpdating = false;
                return false;
            }
        }

        public async Task<Boolean> DeleteTopic(int id)
        {
            try
            {
                Topic topic = await _context.Topic.FindAsync(id);
                if (topic != null)
                {
                    _context.Topic.Remove(topic);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (DBConcurrencyException e)
            {
                return false;
            }
        }

        //get topcis for a particular subject
        public async Task<List<Topic>> FilterTopicsBySubject(int subjectId)
        {
            //keep in mind that the subjectId can be 0
            return await _context.Topic.Where(t => t.Subjects.Any(s => s.Id == subjectId) || subjectId == 0).ToListAsync();
        }
    }
}
