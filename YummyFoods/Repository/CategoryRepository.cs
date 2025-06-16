using Microsoft.EntityFrameworkCore;
using YummyFoods.Data;
using YummyFoods.Models;
using YummyFoods.Repository.IRepository;

namespace YummyFoods.Repository
{
    public class CategoryRepository(ApplicationDbContext context) : ICategoryRepository
    {
        public async Task<Category> CreateAsync(Category category)
        {
           await context.Categories.AddAsync(category);
           await context.SaveChangesAsync();
           return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category is not null)
            {
                context.Categories.Remove(category);
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Category> GetAsync(int id)
        {
           var category = context.Categories.Find(id);
            if (category is null)
            {
                return new Category();
            }
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = await context.Categories.FindAsync(category.Id);
            if (existingCategory is not null)
            {
                existingCategory.Name = category.Name;
                context.Categories.Update(existingCategory);
                await context.SaveChangesAsync();
                return existingCategory;
            }
            return category;
        }
    }
}
