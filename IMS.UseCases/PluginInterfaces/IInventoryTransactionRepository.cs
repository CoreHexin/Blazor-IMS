using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IInventoryTransactionRepository
    {
        Task PurchaseAsync(string orderNumber, Inventory inventory, int quantity, string doneBy, double price);
        Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price);
    }
}