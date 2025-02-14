using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IInventoryRepository _inventoryRepository;

        private List<ProductTransaction> _productTransactions = new List<ProductTransaction>();

        public ProductTransactionRepository(
            IProductRepository productRepository,
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository
        )
        {
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task ProduceAsync(
            string productionNumber,
            Product product,
            int quantity,
            string doneBy
        )
        {
            var prod = await _productRepository.GetByIdAsync(product.Id);
            if (prod == null)
                return;

            foreach (var pi in prod.ProductInventories)
            {
                if (pi.Inventory is null)
                    continue;

                // add inventory transaction
                await _inventoryTransactionRepository.ProduceAsync(
                    productionNumber,
                    pi.Inventory,
                    pi.InventoryQuantity * quantity,
                    doneBy,
                    -1
                );

                // decrease the inventories
                var inventory = await _inventoryRepository.GetByIdAsync(pi.Inventory.Id);
                inventory.Quantity -= pi.InventoryQuantity * quantity;
                await _inventoryRepository.UpdateAsync(inventory);
            }

            // add product transaction
            _productTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.Id,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                ActivityType = ProductionTransactionType.ProduceProduct
            });
        }

        public async Task SellAsync(string orderNumber, Product product, int quantity, string doneBy)
        {
            _productTransactions.Add(new ProductTransaction
            {
                OrderNumber = orderNumber,
                ProductId = product.Id,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                UnitPrice = product.Price,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                ActivityType = ProductionTransactionType.SellProduct
            });
            await Task.CompletedTask;
        }
    }
}
