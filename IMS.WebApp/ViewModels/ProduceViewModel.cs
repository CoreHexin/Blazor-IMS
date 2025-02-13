using IMS.CoreBusiness;
using IMS.WebApp.ViewModelValidations;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class ProduceViewModel
    {
        [Required]
        public string ProductionNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "该产品不存在")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        [Produce_EnsureEnoughInventoryQuantity]
        public int QuantityToProduce { get; set; }

        public Product? Product { get; set; }
    }
}
