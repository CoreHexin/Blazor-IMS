using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelValidations
{
    public class Produce_EnsureEnoughInventoryQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var produceViewModel = validationContext.ObjectInstance as ProduceViewModel;

            if (produceViewModel is null || produceViewModel.Product is null)
            {
                return ValidationResult.Success;
            }

            foreach (var pi in produceViewModel.Product.ProductInventories)
            {
                if (pi.Inventory is not null 
                    && pi.Inventory.Quantity < (produceViewModel.QuantityToProduce * pi.InventoryQuantity)) 
                {
                    return new ValidationResult(
                        $"原材料 {pi.Inventory.Name} 数量不足", 
                        [validationContext.MemberName!]
                    );
                }
            }

            return ValidationResult.Success;
        }
    }
}
