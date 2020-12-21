using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Coupon
    {
        [Required]
        [StringLength(15, ErrorMessage = "Name to long (15 character limit).")]
        public string Code { get; set; }
        [StringLength(30, ErrorMessage = "Name to long (30 character limit).")]
        public string Description { get; set; }
        [Required]
        [CustomDateRange(1)]
        public DateTime StartDate { get; set; }
        [Required]
        [CustomDateRange(1)]
        public DateTime EndDate { get; set; }
        public bool Enabled { get; set; }
        [Required]
        [Range(1, 99, ErrorMessage = "You need to specify a precentage number betweem 1 and 99")]
        public decimal Discount { get; set; }
    }


    public class CustomDateRangeAttribute : RangeAttribute
    {
        public CustomDateRangeAttribute(int yearsEndFromNow): 
            base(typeof(DateTime), DateTime.Now.Date.ToString(), DateTime.Now.Date.AddYears(yearsEndFromNow).ToString()) { }
    }


}
