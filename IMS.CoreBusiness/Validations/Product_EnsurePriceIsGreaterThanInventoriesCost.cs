using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness.Validations
{
    public class Product_EnsurePriceIsGreaterThanInventoriesCost : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var product = validationContext.ObjectInstance as Product;

            if (product != null)
            {
                if (!ValidatePricing(product))
                {
                    return new ValidationResult(
                        $"产品价格低于成本价 {CalculateTotalInventoriesCost(product)}",
                        new List<string>() { validationContext.MemberName }
                    );
                }
            }

            return ValidationResult.Success;
        }

        private double CalculateTotalInventoriesCost(Product product)
        {
            if (product is null || product.ProductInventories.Count() == 0)
                return 0;

            return product.ProductInventories.Sum(x => x.Inventory.Price * x.InventoryQuantity);
        }

        private bool ValidatePricing(Product product)
        {
            if (product.ProductInventories is null || product.ProductInventories.Count() == 0)
                return true;

            if (CalculateTotalInventoriesCost(product) > product.Price)
                return false;

            return true;
        }
    }
}
