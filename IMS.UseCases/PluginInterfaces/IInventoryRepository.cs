using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryRepository
    {
        Task AddAsync(Inventory inventory);
        Task DeleteAsync(int id);
        Task<Inventory?> GetByIdAsync(int id);
        Task<IEnumerable<Inventory>> GetByNameAsync(string name);
        Task UpdateAsync(Inventory inventory);
    }
}
