using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> _inventories;

        public InventoryRepository()
        {
            _inventories = new List<Inventory>()
            {
                new Inventory()
                {
                    Id = 1,
                    Name = "Bike Seat",
                    Quantity = 10,
                    Price = 2,
                },
                new Inventory()
                {
                    Id = 2,
                    Name = "Bike Body",
                    Quantity = 10,
                    Price = 15,
                },
                new Inventory()
                {
                    Id = 3,
                    Name = "Bike Wheels",
                    Quantity = 20,
                    Price = 8,
                },
                new Inventory()
                {
                    Id = 4,
                    Name = "Bike Pedels",
                    Quantity = 20,
                    Price = 1,
                },
            };
        }

        public async Task AddAsync(Inventory inventory)
        {
            if (
                _inventories.Any(x =>
                    x.Name.Equals(inventory.Name, StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                return;
            }

            var maxId = _inventories.Max(x => x.Id);
            inventory.Id = maxId + 1;
            _inventories.Add(inventory);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var inventory = _inventories.FirstOrDefault(x => x.Id == id);
            if (inventory != null)
            {
                _inventories.Remove(inventory);
            }
            await Task.CompletedTask;
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            var inventory = _inventories.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(inventory); 
        }

        public async Task<IEnumerable<Inventory>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(_inventories);
            }

            return _inventories
                .Where(inventory =>
                    inventory.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();
        }

        public async Task UpdateAsync(Inventory inventory)
        {
            if (
                _inventories.Any(x =>
                    x.Id != inventory.Id
                    && x.Name.Equals(inventory.Name, StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                return;
            }

            var inventoryToUpdate = _inventories.Where(x => x.Id == inventory.Id).FirstOrDefault();

            if (inventoryToUpdate is null)
            {
                return;
            }

            inventoryToUpdate.Name = inventory.Name;
            inventoryToUpdate.Quantity = inventory.Quantity;
            inventoryToUpdate.Price = inventory.Price;

            await Task.CompletedTask;
        }
    }
}
