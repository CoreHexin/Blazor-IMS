using IMS.CoreBusiness;
using IMS.WebApp.ViewModelValidations;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class SellViewModel
    {
        [Required]
        public string OrderNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "必须选择一个产品")]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "不是一个有效的产品数量")]
        [Sell_EnsureEnoughProductQuantity]
        public int QuantityToSell { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "不是一个有效的价格")]
        public decimal UnitPrice { get; set; }

        public Product? Product { get; set; }
    }
}
