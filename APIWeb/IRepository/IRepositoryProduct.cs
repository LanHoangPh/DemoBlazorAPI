using APIWeb.Data;

namespace APIWeb.IRepository
{
    public interface IRepositoryProduct
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
    }
}
