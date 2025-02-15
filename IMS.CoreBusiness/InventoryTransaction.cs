using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class InventoryTransaction
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;
        public string ProductionNumber { get; set; } = string.Empty;

        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int QuantityBefore { get; set; }

        [Required]
        public int QuantityAfter { get; set; }

        public Decimal UnitPrice { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string DoneBy { get; set; } = string.Empty;

        [Required]
        public InventoryTransactionType ActivityType { get; set; }

        public Inventory? Inventory { get; set; }
    }
}
