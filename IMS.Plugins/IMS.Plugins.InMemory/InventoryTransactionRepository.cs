using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private List<InventoryTransaction> _inventoryTransaction = new List<InventoryTransaction>();

        public void PurchaseAsync(string orderNumber, Inventory inventory, int quantity, string doneBy, double price)
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
        }
    }
}
