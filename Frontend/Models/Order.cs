﻿using System;

namespace Frontend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime DateRegistered { get; set; }
        public int? CouponId { get; set; }
        public int UserId { get; set; }
    }
}