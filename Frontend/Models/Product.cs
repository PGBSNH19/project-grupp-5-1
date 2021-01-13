using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 500, ErrorMessage = "Stock must be between 1 and 500 .")]
        public int Stock { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "Name too long (60 character limit).")]
        public string Name { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "Description too long (250 character limit).")]
        public string Description { get; set; }

        [Required]
        public bool IsFeatured { get; set; }

        public bool IsAvailable { get; set; }

        public int ProductCategoryId { get; set; }

        public ICollection<ProductPrice> ProductPrices { get; set; }

        public string ProductCategoryName { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Price must be between 1 and 100000.")]
        public decimal Price { get; set; }

        [Required]
        [PriceLessThan("Price", ErrorMessage = "Sale Price cannot be larger then Original price")]
        public decimal? SalePrice { get; set; }

        public decimal? CurrentPrice { get; set; }
    }

    public class PriceLessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public PriceLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (decimal)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty, typeof(decimal));

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (decimal)property.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}