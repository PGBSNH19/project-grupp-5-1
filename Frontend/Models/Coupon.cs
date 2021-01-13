using System;
using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "CouponName to long (15 character limit).")]
        public string Code { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Description to long (30 character limit).")]
        public string Description { get; set; }

        [Required]
        [CustomDateRange(1)]
        public DateTime StartDate { get; set; }

        [Required]
        [CustomDateRange(1)]
        [DateLessThan("StartDate", ErrorMessage = "End Date can't be smaller then Start Date")]
        public DateTime EndDate { get; set; }

        public bool Enabled { get; set; }

        [Required]
        [Range(0.01, 0.99, ErrorMessage = "You need to specify a precentage number betweem 0,01 and 0,99")]
        public decimal Discount { get; set; }
    }

    public class CustomDateRangeAttribute : RangeAttribute
    {
        public CustomDateRangeAttribute(int yearsEndFromNow) :
            base(typeof(DateTime), DateTime.Now.Date.ToString(), DateTime.Now.Date.AddYears(yearsEndFromNow).ToString())
        { }
    }

    public class DateLessThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty, typeof(DateTime));

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue < comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}