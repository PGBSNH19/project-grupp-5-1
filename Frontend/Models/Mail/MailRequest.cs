﻿using System.Collections.Generic;

namespace Frontend.Models.Mail
{
    public class MailRequest
    {
        public string UserName { get; set; }
        public string ToEmail { get; set; }
        public int OrderId { get; set; }
        public string Subject { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public decimal TotalPiceWithDiscount { get; set; }
        public string Date { get; set; }
        public string DiscountName { get; set; }
        public string Discount { get; set; }
        public IEnumerable<BuyedProducts> buyedProductsList { get; set; }
    }
}