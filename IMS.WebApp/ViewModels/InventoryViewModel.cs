using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class InventoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "数量必须大于等于0")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "单价必须大于等于0")]
        [Precision(18,2)]
        public decimal Price { get; set; }
    }
}
