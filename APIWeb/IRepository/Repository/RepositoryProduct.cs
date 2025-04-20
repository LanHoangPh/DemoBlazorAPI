using APIWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace APIWeb.IRepository.Repository
{
    public class RepositoryProduct : IRepositoryProduct
    {
        private readonly AppDbContext _dbContext;

        public RepositoryProduct(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task CreateProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product is not null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbContext.Products.FindAsync(id);
        }

        public async Task UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
