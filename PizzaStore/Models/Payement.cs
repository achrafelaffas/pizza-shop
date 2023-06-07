
using System;
using System.ComponentModel.DataAnnotations;

namespace PizzaStore.Models
{
    public class Payement
    {
        [Required(ErrorMessage = "Name on the card is required")]
        public string NameOnTheCard { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [CreditCard(ErrorMessage = "Invalid credit card number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiration date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
        [ExpirationDate(ErrorMessage = "Card has expired")]
        public DateTime ExpirationDate { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [Range(100, 999, ErrorMessage = "Invalid CVV")]
        public int CVV { get; set; }
    }

    public class ExpirationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime expirationDate)
            {
                if (expirationDate < DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
