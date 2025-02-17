using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductTransactionEFCoreRepository : IProductTransactionRepository
    {
        private readonly IDbContextFactory<IMSDbContext> _dbContextFactory;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;

        public ProductTransactionEFCoreRepository(
            IDbContextFactory<IMSDbContext> dbContextFactory,
            IInventoryRepository inventoryRepository,
            IProductRepository productRepository,
            IInventoryTransactionRepository inventoryTransactionRepository
        )
        {
            _dbContextFactory = dbContextFactory;
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
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
            using var db = _dbContextFactory.CreateDbContext();
            await db.ProductTransactions.AddAsync(
                new ProductTransaction
                {
                    ProductionNumber = productionNumber,
                    ProductId = product.Id,
                    QuantityBefore = product.Quantity,
                    QuantityAfter = product.Quantity + quantity,
                    TransactionDate = DateTime.Now,
                    DoneBy = doneBy,
                    ActivityType = ProductTransactionType.ProduceProduct,
                }
            );
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductTransaction>> SearchAsync(
            string productName,
            DateTime? dateFrom,
            DateTime? dateTo,
            ProductTransactionType? transactionType
        )
        {
            using var db = _dbContextFactory.CreateDbContext();
            var query = db.ProductTransactions
                .Include(pt => pt.Product)
                .Where(pt =>
                    pt.Product.Name.ToLower().IndexOf(productName.ToLower()) >= 0
                );

            if (dateFrom is not null)
            {
                query = query.Where(x => x.TransactionDate >= dateFrom);
            }

            if (dateTo is not null)
            {
                query = query.Where(x => x.TransactionDate <= dateTo);
            }

            if (transactionType is not null)
            {
                query = query.Where(x => x.ActivityType == transactionType);
            }

            return await query.OrderByDescending(pt => pt.TransactionDate).ToListAsync();
        }

        public async Task SellAsync(
            string orderNumber,
            Product product,
            int quantity,
            decimal unitPrice,
            string doneBy
        )
        {
            using var db = _dbContextFactory.CreateDbContext();

            await db.ProductTransactions.AddAsync(
                new ProductTransaction
                {
                    OrderNumber = orderNumber,
                    ProductId = product.Id,
                    QuantityBefore = product.Quantity,
                    QuantityAfter = product.Quantity - quantity,
                    UnitPrice = unitPrice,
                    TransactionDate = DateTime.Now,
                    DoneBy = doneBy,
                    ActivityType = ProductTransactionType.SellProduct,
                }
            );
            await db.SaveChangesAsync();
        }
    }
}
