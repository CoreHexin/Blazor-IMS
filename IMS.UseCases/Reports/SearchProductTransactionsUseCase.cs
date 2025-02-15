using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.Reports.Interfaces;

namespace IMS.UseCases.Reports
{
    public class SearchProductTransactionsUseCase : ISearchProductTransactionsUseCase
    {
        private readonly IProductTransactionRepository _productTransactionRepository;

        public SearchProductTransactionsUseCase(
            IProductTransactionRepository productTransactionRepository
        )
        {
            _productTransactionRepository = productTransactionRepository;
        }

        public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(
            string productName,
            DateTime? dateFrom,
            DateTime? dateTo,
            ProductionTransactionType? transactionType
        )
        {
            if (dateTo is not null)
            {
                dateTo = dateTo.Value.AddDays(1);
            }

            return await _productTransactionRepository.SearchAsync(
                productName,
                dateFrom,
                dateTo,
                transactionType
            );
        }
    }
}
