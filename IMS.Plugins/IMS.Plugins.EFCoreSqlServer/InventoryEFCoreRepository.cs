using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class InventoryEFCoreRepository : IInventoryRepository
    {
        private readonly IDbContextFactory<IMSDbContext> _dbContextFactory;

        public InventoryEFCoreRepository(IDbContextFactory<IMSDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Inventory inventory)
        {
            using var db = _dbContextFactory.CreateDbContext();
            await db.Inventories.AddAsync(inventory);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var inventory = await db.Inventories.FindAsync(id);
            if (inventory is null)
                return;
            db.Inventories.Remove(inventory);
            await db.SaveChangesAsync();
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var inventory = await db.Inventories.FindAsync(id);
            return inventory;
        }

        public async Task<IEnumerable<Inventory>> GetByNameAsync(string name)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var inventories = await db.Inventories
                .Where(inventory => inventory.Name.ToLower().IndexOf(name.ToLower()) >= 0)
                .ToListAsync();
            return inventories;
        }

        public async Task UpdateAsync(Inventory inventory)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var currentInventory = await db.Inventories.FindAsync(inventory.Id);
            if (currentInventory is null)
                return;

            currentInventory.Name = inventory.Name;
            currentInventory.Price = inventory.Price;
            currentInventory.Quantity = inventory.Quantity;
            await db.SaveChangesAsync();
        }
    }
}
