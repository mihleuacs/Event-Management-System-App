using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAPI.Models;
using WebAPI.Services.Interface;

namespace WebAPI.Services.Repos
{
    public class EventPostRepository : IEventPostRepository
    {
        private readonly ApplicationDbContext _context;
        public EventPostRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<EventPost> AddEventPostAsync(EventPost eventPost)
        {
            _context.events.Add(eventPost);
            await _context.SaveChangesAsync();
            return eventPost;  
        }

        public async Task<EventPost> UpdateEventPostAsync(EventPost eventPost)
        {
            _context.events.Update(eventPost);
            await _context.SaveChangesAsync();
            return eventPost;
        }

        public async Task DeleteEventPostAsync(EventPost eventPost)
        {
            _context.events.Remove(eventPost);
            await _context.SaveChangesAsync();
        }

        public async Task<EventPost?> FindEventPostByIdAsync(int id)
        {
            return await _context.events.FindAsync(id);
        }

        public async Task<IEnumerable<EventPost>> GetEventPostsAsync()
        {
            return await _context.events.ToListAsync();
        }


    }
}
