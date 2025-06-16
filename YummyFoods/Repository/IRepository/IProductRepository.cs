using YummyFoods.Models;

namespace YummyFoods.Repository.IRepository
{
    public interface IProductRepository
    {
        public Task <Product> CreateAsync(Product product);
        public Task<Product> UpdateAsync(Product product);
        public Task <bool> DeleteAsync(int id);
        public Task<Product> GetAsync(int id);
        public Task <IEnumerable<Product>> GetAllAsync();

    }
}
