using System;

namespace Backend.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime DateRegistered { get; set; }
        public int? CouponId { get; set; }
        public int UserId { get; set; }
    }
}