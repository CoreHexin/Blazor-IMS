using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class PurchaseViewModel
    {
        [Required]
        public string OrderNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int InventoryId { get; set; }

        [Range(1, int.MaxValue)]
        public int QuantityToPurchase { get; set; }

        public double InventoryPrice { get; set; }
    }
}
