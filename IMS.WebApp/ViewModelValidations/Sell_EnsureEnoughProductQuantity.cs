using IMS.WebApp.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModelValidations
{
    public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var sellViewModel = validationContext.ObjectInstance as SellViewModel;

            if (sellViewModel is null || sellViewModel.Product is null || value is null)
                return ValidationResult.Success;

            if (sellViewModel.QuantityToSell > sellViewModel.Product.Quantity)
                return new ValidationResult("库存不足", [validationContext.MemberName!]);

            return ValidationResult.Success;
        }
    }
}
