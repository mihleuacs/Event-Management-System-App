using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebAPI.Models;
using WebAPI.Services.Interface;

namespace WebAPI.Services.Repos
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            _context.categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            _context.categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> FindCategoryByIdAsync(int id)
        {
            return await _context.categories.FindAsync(id);
        }

        public async Task<Category?> FindCategoryByNameAsync(string categoryName)
        {
            return await _context.categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.categories.ToListAsync();
        }
    }
}
