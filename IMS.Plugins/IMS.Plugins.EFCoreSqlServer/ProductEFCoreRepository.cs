using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer
{
    public class ProductEFCoreRepository : IProductRepository
    {
        private readonly IDbContextFactory<IMSDbContext> _dbContextFactory;

        public ProductEFCoreRepository(IDbContextFactory<IMSDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task AddAsync(Product product)
        {
            using var db = _dbContextFactory.CreateDbContext();
            await db.Products.AddAsync(product);
            FlagInventoryUnchanged(product, db);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var product = await db.Products.FindAsync(id);
            if (product is null)
                return;
            db.Products.Remove(product);
            await db.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            using var db = _dbContextFactory.CreateDbContext();
            return await db.Products
                .Include(p => p.ProductInventories)
                .ThenInclude(pt => pt.Inventory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var products = await db.Products
                .Where(product => product.Name.ToLower().IndexOf(name.ToLower()) >= 0)
                .ToListAsync();
            return products;
        }

        public async Task UpdateAsync(Product product)
        {
            using var db = _dbContextFactory.CreateDbContext();
            var currentProduct = await db.Products
                .Include(p => p.ProductInventories)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (currentProduct is null)
                return;

            currentProduct.Name = product.Name;
            currentProduct.Price = product.Price;
            currentProduct.Quantity = product.Quantity;
            currentProduct.ProductInventories = product.ProductInventories;
            FlagInventoryUnchanged(currentProduct, db);
            await db.SaveChangesAsync();
        }

        private void FlagInventoryUnchanged(Product product, IMSDbContext db)
        {
            if (product.ProductInventories is not null && product.ProductInventories.Any())
            {
                foreach (var productInventory in product.ProductInventories)
                {
                    db.Entry(productInventory.Inventory).State = EntityState.Unchanged;
                }
            }
        }
    }
}
