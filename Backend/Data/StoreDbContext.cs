using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext()
        {

        }

        public StoreDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var userRoleEnumConverter = new EnumToNumberConverter<UserRole, int>();

            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.Role)
                    .HasConversion(userRoleEnumConverter)
                    .HasDefaultValueSql("((0))");

            });

            builder.Entity<Weather>().ToTable("Weather");
            builder.Entity<Weather>().HasKey(p => p.Id);
            builder.Entity<Weather>().HasData(new
            {
                Id = 1,
                Name = "Freezing"
            },
            new
            {
                Id = 2,
                Name = "Bracing"
            },
            new
            {
                Id = 3,
                Name = "Chilly"
            },
            new
            {
                Id = 4,
                Name = "Cool"
            },
            new
            {
                Id = 5,
                Name = "Mild"
            },
            new
            {
                Id = 6,
                Name = "Warm"
            },
            new
            {
                Id = 7,
                Name = "Balmy"
            },
            new
            {
                Id = 8,
                Name = "Hot"
            },
            new
            {
                Id = 9,
                Name = "Sweltering"
            },
            new
            {
                Id = 10,
                Name = "Scorching"
            });

            builder.Entity<ProductCategory>().ToTable("ProductCategory");
            builder.Entity<ProductCategory>().HasKey(p => p.Id);
            builder.Entity<ProductCategory>().HasData(new
            {
                Id = 1,
                CategoryName = "Uncategorized"
            });

            base.OnModelCreating(builder);
        }

        public virtual DbSet<Weather> Weather { get; set; }
        public virtual DbSet<Coupon> Coupon { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderedProduct> OrderedProduct { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductPrice> ProductPrice { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
