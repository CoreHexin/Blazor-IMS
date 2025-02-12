using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Inventories
{
    public class DeleteInventoryByIdUserCase : IDeleteInventoryByIdUserCase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public DeleteInventoryByIdUserCase(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(int id)
        {
            await _inventoryRepository.DeleteAsync(id);
        }
    }
}
