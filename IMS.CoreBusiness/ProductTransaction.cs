using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class ProductTransaction
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;
        public string ProductionNumber { get; set; } = string.Empty;

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int QuantityBefore { get; set; }

        [Required]
        public int QuantityAfter { get; set; }

        public Decimal? UnitPrice { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string DoneBy { get; set; } = string.Empty;

        [Required]
        public ProductTransactionType ActivityType { get; set; }

        public Product? Product { get; set; }

    }
}
