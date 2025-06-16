using Microsoft.EntityFrameworkCore;
using YummyFoods.Data;
using YummyFoods.Models;
using YummyFoods.Repository.IRepository;

namespace YummyFoods.Repository
{
    public class ProductRepository(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment) : IProductRepository
    {
        public async Task<Product> CreateAsync(Product product)
        {
           await context.Products.AddAsync(product);
           await context.SaveChangesAsync();
           return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await context.Products.FindAsync(id);
            var imagePath = Path.Combine(webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);

            }
            if (product is not null)
            {
                context.Products.Remove(product);
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<Product> GetAsync(int id)
        {
           var product = context.Products.Find(id);
            if (product is null)
            {
                return new Product();
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products.Include(p=>p.Category).ToListAsync();
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = await context.Products.FindAsync(product.Id);
            if (existingProduct is not null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.CategoryId = product.CategoryId;    
                context.Products.Update(existingProduct);
                await context.SaveChangesAsync();
                return existingProduct;
            }
            return product;
        }
    }
}
