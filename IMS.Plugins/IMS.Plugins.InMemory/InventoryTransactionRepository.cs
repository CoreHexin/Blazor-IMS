using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private List<InventoryTransaction> _inventoryTransaction = new List<InventoryTransaction>();

        public async Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price)
        {
            _inventoryTransaction.Add(new InventoryTransaction
            {
                ProductionNumber = productionNumber,
                InventoryId = inventory.Id,
                QuantityBefore = inventory.Quantity,
                QuantityAfter = inventory.Quantity - quantityToConsume,
                UnitPrice = price,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                ActivityType = InventoryTransactionType.ProduceProduct,
                Inventory = inventory
            });
            await Task.CompletedTask;
        }

        public async Task PurchaseAsync(string orderNumber, Inventory inventory, int quantity, string doneBy, double price)
        {
            _inventoryTransaction.Add(new InventoryTransaction
            {
                OrderNumber = orderNumber,
                InventoryId = inventory.Id,
                QuantityBefore = inventory.Quantity,
                QuantityAfter = inventory.Quantity + quantity,
                UnitPrice = price,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                Inventory = inventory
            });
            await Task.CompletedTask;
        }
    }
}
