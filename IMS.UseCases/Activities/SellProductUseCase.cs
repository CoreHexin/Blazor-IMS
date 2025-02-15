using IMS.CoreBusiness;
using IMS.UseCases.Activities.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities
{
    public class SellProductUseCase : ISellProductUseCase
    {
        private readonly IProductTransactionRepository _productTransactionRepository;
        private readonly IProductRepository _productRepository;

        public SellProductUseCase(
            IProductTransactionRepository productTransactionRepository,
            IProductRepository productRepository
        )
        {
            _productTransactionRepository = productTransactionRepository;
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(
            string orderNumber,
            Product product,
            int quantity,
            decimal unitPrice,
            string doneBy
        )
        {
            await _productTransactionRepository.SellAsync(orderNumber, product, quantity, unitPrice, doneBy);

            product.Quantity -= quantity;
            await _productRepository.UpdateAsync(product);
        }
    }
}
