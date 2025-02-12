using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task DeleteAsync(int id);
        Task<Product?> GetByIdAsync(int id);
        Task UpdateAsync(Product product);
        Task<IEnumerable<Product>> GetByNameAsync(string name);
    }
}
