using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly IInventoryRepository _inventoryRepository;
        private List<InventoryTransaction> _inventoryTransaction = new List<InventoryTransaction>();

        public InventoryTransactionRepository(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetTransactionsAsync(
            string inventoryName,
            DateTime? dateFrom,
            DateTime? dateTo,
            InventoryTransactionType? transactionType
        ) 
        {
            var inventories = await _inventoryRepository.GetByNameAsync(inventoryName);

            var query = inventories.Join(
                _inventoryTransaction,
                inventory => inventory.Id,
                inventoryTransaction => inventoryTransaction.InventoryId,
                (inventory, inventoryTransaction) => new InventoryTransaction
                {
                    OrderNumber = inventoryTransaction.OrderNumber,
                    ProductionNumber = inventoryTransaction.ProductionNumber,
                    InventoryId = inventoryTransaction.InventoryId,
                    QuantityBefore = inventoryTransaction.QuantityBefore,
                    QuantityAfter = inventoryTransaction.QuantityAfter,
                    UnitPrice = inventoryTransaction.UnitPrice,
                    TransactionDate = inventoryTransaction.TransactionDate,
                    DoneBy = inventoryTransaction.DoneBy,
                    ActivityType = inventoryTransaction.ActivityType,
                    Inventory = inventory
                }
            );

            if (transactionType is not null)
            {
                query = query.Where(x => x.ActivityType == transactionType);
            }

            if (dateFrom is not null)
            {
                query = query.Where(x => x.TransactionDate >= dateFrom);
            }

            if (dateTo is not null)
            {
                query = query.Where(x => x.TransactionDate <= dateTo);
            }

            return query.OrderByDescending(x => x.TransactionDate); ;
        }

        public async Task ProduceAsync(
            string productionNumber,
            Inventory inventory,
            int quantityToConsume,
            string doneBy,
            decimal price
        )
        {
            _inventoryTransaction.Add(
                new InventoryTransaction
                {
                    ProductionNumber = productionNumber,
                    InventoryId = inventory.Id,
                    QuantityBefore = inventory.Quantity,
                    QuantityAfter = inventory.Quantity - quantityToConsume,
                    UnitPrice = price,
                    TransactionDate = DateTime.Now,
                    DoneBy = doneBy,
                    ActivityType = InventoryTransactionType.ProduceProduct,
                    Inventory = inventory,
                }
            );
            await Task.CompletedTask;
        }

        public async Task PurchaseAsync(
            string orderNumber,
            Inventory inventory,
            int quantity,
            string doneBy,
            decimal price
        )
        {
            _inventoryTransaction.Add(
                new InventoryTransaction
                {
                    OrderNumber = orderNumber,
                    InventoryId = inventory.Id,
                    QuantityBefore = inventory.Quantity,
                    QuantityAfter = inventory.Quantity + quantity,
                    UnitPrice = price,
                    TransactionDate = DateTime.Now,
                    DoneBy = doneBy,
                    ActivityType = InventoryTransactionType.PurchaseInventory,
                    Inventory = inventory,
                }
            );
            await Task.CompletedTask;
        }
    }
}
