using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class InventoryTransactionEFCoreRepository : IInventoryTransactionRepository
    {
        private readonly IDbContextFactory<IMSDbContext> _dbContextFactory;

        public InventoryTransactionEFCoreRepository(IDbContextFactory<IMSDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, decimal price)
        {
            var transaction = new InventoryTransaction
            {
                ProductionNumber = productionNumber,
                InventoryId = inventory.Id,
                QuantityBefore = inventory.Quantity,
                QuantityAfter = inventory.Quantity - quantityToConsume,
                UnitPrice = price,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                ActivityType = InventoryTransactionType.ProduceProduct,
            };
            using var db = _dbContextFactory.CreateDbContext();
            await db.InventoryTransactions.AddAsync(transaction);
            await db.SaveChangesAsync();
        }

        public async Task PurchaseAsync(string orderNumber, Inventory inventory, int quantity, string doneBy, decimal price)
        {
            var transaction = new InventoryTransaction
            {
                OrderNumber = orderNumber,
                InventoryId = inventory.Id,
                QuantityBefore = inventory.Quantity,
                QuantityAfter = inventory.Quantity + quantity,
                UnitPrice = price,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                ActivityType = InventoryTransactionType.PurchaseInventory,
            };
            using var db = _dbContextFactory.CreateDbContext();
            await db.InventoryTransactions.AddAsync(transaction);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryTransaction>> SearchAsync(string inventoryName, DateTime? dateFrom, DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            using var db = _dbContextFactory.CreateDbContext();

            var query = db.InventoryTransactions
                .Include(it => it.Inventory)
                .Where(it => it.Inventory.Name.ToLower().IndexOf(inventoryName.ToLower()) >= 0);

            if (dateFrom is not null)
            {
                query = query.Where(it => it.TransactionDate >= dateFrom);
            }

            if (dateTo is not null)
            {
                query = query.Where(it => it.TransactionDate <= dateTo);
            }

            if (transactionType is not null)
            {
                query = query.Where(it => it.ActivityType == transactionType);
            }

            return await query.OrderByDescending(it => it.TransactionDate).ToListAsync();
        }
    }
}
