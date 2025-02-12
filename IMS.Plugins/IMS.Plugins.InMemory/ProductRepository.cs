using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "Bike",
                    Quantity = 10,
                    Price = 150,
                },
                new Product()
                {
                    Id = 2,
                    Name = "Car",
                    Quantity = 10,
                    Price = 25000,
                },
            };
        }

        public async Task AddAsync(Product product)
        {
            if (
                _products.Any(x =>
                    x.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                return;
            }

            var maxId = _products.Max(x => x.Id);
            product.Id = maxId + 1;
            _products.Add(product);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            await Task.CompletedTask;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(product);
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(_products);
            }

            return _products
                .Where(product =>
                    product.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();
        }

        public async Task UpdateAsync(Product product)
        {
            if (
                _products.Any(x =>
                    x.Id != product.Id
                    && x.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                return;
            }

            var productToUpdate = _products.Where(x => x.Id == product.Id).FirstOrDefault();

            if (productToUpdate is null)
            {
                return;
            }

            productToUpdate.Name = product.Name;
            productToUpdate.Quantity = product.Quantity;
            productToUpdate.Price = product.Price;

            await Task.CompletedTask;
        }
    }
}
