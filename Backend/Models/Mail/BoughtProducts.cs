﻿namespace Backend.Models.Mail
{
    public class BoughtProducts
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}