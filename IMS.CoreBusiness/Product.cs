using IMS.CoreBusiness.Validations;
using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "数量必须大于等于0")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "单价必须大于等于0")]
        public double Price { get; set; }

        [Product_EnsurePriceIsGreaterThanInventoriesCost]
        public List<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();

        public void AddInventory(Inventory inventory)
        {
            if (ProductInventories.Any(x => x.Inventory is not null && x.Inventory.Name.Equals(inventory.Name)))
                return;

            ProductInventories.Add(new ProductInventory
            {
                ProductId = Id,
                InventoryId = inventory.Id,
                InventoryQuantity = 1,
                Product = this,
                Inventory = inventory
            });
        }

        public void RemoveInventory(ProductInventory productInventory)
        {
            ProductInventories.Remove(productInventory);
        }
    }
}
