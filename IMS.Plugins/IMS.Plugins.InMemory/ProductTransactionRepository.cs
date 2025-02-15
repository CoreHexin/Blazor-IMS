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
            _productTransactions.Add(
                new ProductTransaction
                {
                    ProductionNumber = productionNumber,
                    ProductId = product.Id,
                    QuantityBefore = product.Quantity,
                    QuantityAfter = product.Quantity + quantity,
                    TransactionDate = DateTime.Now,
                    DoneBy = doneBy,
                    ActivityType = ProductionTransactionType.ProduceProduct,
                }
            );
        }

        public async Task<IEnumerable<ProductTransaction>> SearchAsync(
            string productName,
            DateTime? dateFrom,
            DateTime? dateTo,
            ProductionTransactionType? transactionType
        )
        {
            var products = await _productRepository.GetByNameAsync(productName);
            var query = products.Join(
                _productTransactions,
                product => product.Id,
                productTransaction => productTransaction.ProductId,
                (product, productTransaction) =>
                    new ProductTransaction
                    {
                        OrderNumber = productTransaction.OrderNumber,
                        ProductionNumber = productTransaction.ProductionNumber,
                        ProductId = productTransaction.ProductId,
                        QuantityBefore = productTransaction.QuantityBefore,
                        QuantityAfter = productTransaction.QuantityAfter,
                        UnitPrice = productTransaction.UnitPrice,
                        TransactionDate = productTransaction.TransactionDate,
                        DoneBy = productTransaction.DoneBy,
                        ActivityType = productTransaction.ActivityType,
                        Product = product,
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

            return query.OrderByDescending(x => x.TransactionDate);
        }

        public async Task SellAsync(
            string orderNumber,
            Product product,
            int quantity,
            decimal unitPrice,
            string doneBy
        )
        {
            _productTransactions.Add(
                new ProductTransaction
                {
                    OrderNumber = orderNumber,
                    ProductId = product.Id,
                    QuantityBefore = product.Quantity,
                    QuantityAfter = product.Quantity - quantity,
                    UnitPrice = unitPrice,
                    TransactionDate = DateTime.Now,
                    DoneBy = doneBy,
                    ActivityType = ProductionTransactionType.SellProduct,
                }
            );
            await Task.CompletedTask;
        }
    }
}
