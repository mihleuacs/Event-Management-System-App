using WebAPI.Models;

namespace WebAPI.Services.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
        Task<Category?> FindCategoryByIdAsync(int id);
        Task<Category?> FindCategoryByNameAsync(string categoryName);
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}
