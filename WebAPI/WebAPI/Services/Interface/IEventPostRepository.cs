using WebAPI.Models;

namespace WebAPI.Services.Interface
{
    public interface IEventPostRepository
    {
        Task<EventPost> AddEventPostAsync(EventPost eventPost);
        Task<EventPost> UpdateEventPostAsync(EventPost eventPost);
        Task<IEnumerable<EventPost>> GetEventPostsAsync();
        Task<EventPost?> FindEventPostByIdAsync(int id);
        Task DeleteEventPostAsync(EventPost eventPost);
    }
}
