using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductTransactionRepository
    {
        Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy);

        Task<IEnumerable<ProductTransaction>> SearchAsync(
            string productName,
            DateTime? dateFrom,
            DateTime? dateTo,
            ProductionTransactionType? transactionType
        );

        Task SellAsync(
            string orderNumber,
            Product product,
            int quantity,
            decimal unitPrice,
            string doneBy
        );
    }
}
