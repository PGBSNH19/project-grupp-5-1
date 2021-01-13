using System;

namespace Backend.DTO
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Enabled { get; set; }
        public decimal Discount { get; set; }
    }
}