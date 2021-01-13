using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

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
            },
            new
            {
                Id = 2,
                CategoryName = "Headphones"
            },
            new
            {
                Id = 3,
                CategoryName = "Smart Watches"
            },
            new
            {
                Id = 4,
                CategoryName = "Tablets"
            });

            builder.Entity<Product>().ToTable("Product");
            builder.Entity<Product>().HasKey(p => p.Id);
            builder.Entity<Product>().HasData(new
            {
                Id = 1,
                Name = "SAMSUNG Galaxy Buds",
                Description = "Pair these Samsung Galaxy Buds True Wireless Earbuds with your device and go.",
                IsAvailable = true,
                IsFeatured = false,
                Stock = 65,
                ProductCategoryId = 2
            },
            new
            {
                Id = 2,
                Name = "Apple Watch Series 3 GPS",
                Description = "Stay on top of your fitness with this Apple Watch Series 3.",
                IsAvailable = true,
                IsFeatured = true,
                Stock = 34,
                ProductCategoryId = 3
            },
            new
            {
                Id = 3,
                Name = "Google Nest Mini (2nd Generation) - Charcoal",
                Description = "Meet the 2nd generation Nest Mini, the speaker you control with your voice.",
                IsAvailable = true,
                IsFeatured = false,
                Stock = 11,
                ProductCategoryId = 1
            },
            new
            {
                Id = 4,
                Name = "SAMSUNG Galaxy Tab A 8.0\" 32 GB",
                Description = "Keep your favorite songs, photos, videos and games, thanks to 32GB of built-in memory.",
                IsAvailable = true,
                IsFeatured = false,
                Stock = 23,
                ProductCategoryId = 4
            },
            new
            {
                Id = 5,
                Name = "HP 14\" Pentium 4GB / 64GB Chromebook",
                Description = "At less than four pounds, this thin and light silver Chromebook laptop is easy to take from room to room or on the road.",
                IsAvailable = true,
                IsFeatured = true,
                Stock = 43,
                ProductCategoryId = 1
            },
            new
            {
                Id = 6,
                Name = "JLab Audio Studio ANC On-Ear",
                Description = "Enjoy all-day comfort and distraction-free music with the new Studio ANC.",
                IsAvailable = false,
                IsFeatured = false,
                Stock = 37,
                ProductCategoryId = 2
            });

            builder.Entity<User>().ToTable("User");
            builder.Entity<User>().HasKey(p => p.Id);
            builder.Entity<User>().HasData(new
            {
                Id = 1,
                FirstName = "Ahmad",
                LastName = "Yassin",
                Username = "akyassin",
                Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                Role = UserRole.Admin
            },
            new
            {
                Id = 2,
                FirstName = "Andre",
                LastName = "Morad",
                Username = "amorad",
                Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                Role = UserRole.Admin
            },
            new
            {
                Id = 3,
                FirstName = "Nor",
                LastName = "Shiervani",
                Username = "nshiervani",
                Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                Role = UserRole.Admin
            },
            new
            {
                Id = 4,
                FirstName = "Irvin",
                LastName = "Perez",
                Username = "iperez",
                Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                Role = UserRole.Admin
            },
            new
            {
                Id = 5,
                FirstName = "Micael",
                LastName = "Wolter",
                Username = "mwolter",
                Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                Role = UserRole.Admin
            },
            new
            {
                Id = 6,
                FirstName = "Jim",
                LastName = "Bob",
                Username = "customerjim",
                Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                Role = UserRole.Customer
            });

            builder.Entity<Coupon>().ToTable("Coupon");
            builder.Entity<Coupon>().HasKey(p => p.Id);
            builder.Entity<Coupon>().HasData(new
            {
                Id = 1,
                Code = "Flash",
                Description = "Get 25% off on all stocks we have.",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                Enabled = true,
                Discount = 0.25m
            },
            new
            {
                Id = 2,
                Code = "Year2021",
                Description = "You will get 10% discount on every product you buy..",
                StartDate = DateTime.Now.AddDays(3),
                EndDate = DateTime.Now.AddDays(13),
                Enabled = true,
                Discount = 0.1m
            });

            builder.Entity<ProductPrice>().ToTable("ProductPrice");
            builder.Entity<ProductPrice>().HasKey(p => p.Id);
            builder.Entity<ProductPrice>().HasData(new
            {
                Id = 1,
                Price = 1300m,
                SalePrice = 900m,
                DateChanged = DateTime.Now,
                ProductId = 1,
            },
            new
            {
                Id = 2,
                Price = 2290m,
                DateChanged = DateTime.Now,
                ProductId = 2,
            },
            new
            {
                Id = 3,
                Price = 329m,
                DateChanged = DateTime.Now,
                ProductId = 3,
            },
            new
            {
                Id = 4,
                Price = 2490m,
                DateChanged = DateTime.Now,
                ProductId = 4,
            },
            new
            {
                Id = 5,
                Price = 3790m,
                SalePrice = 3200m,
                DateChanged = DateTime.Now,
                ProductId = 5,
            },
            new
            {
                Id = 6,
                Price = 319m,
                SalePrice = 250m,
                DateChanged = DateTime.Now,
                ProductId = 6,
            });

            //Image name must be created as GUID numbers by client side, those number for
            //initial seeding only.
            builder.Entity<ProductImage>().ToTable("ProductImage");
            builder.Entity<ProductImage>().HasKey(p => p.Id);
            builder.Entity<ProductImage>().HasData(new
            {
                Id = 1,
                ImageName = "01A.jpg",
                IsDefault = true,
                ProductId = 1,
            },
            new
            {
                Id = 2,
                ImageName = "01B.jpg",
                IsDefault = false,
                ProductId = 1,
            },
            new
            {
                Id = 3,
                ImageName = "02A.jpg",
                IsDefault = true,
                ProductId = 2,
            },
            new
            {
                Id = 4,
                ImageName = "02B.jpg",
                IsDefault = false,
                ProductId = 2,
            },
            new
            {
                Id = 5,
                ImageName = "03A.jpg",
                IsDefault = true,
                ProductId = 3,
            },
            new
            {
                Id = 6,
                ImageName = "03B.jpg",
                IsDefault = false,
                ProductId = 3,
            },
            new
            {
                Id = 7,
                ImageName = "04A.jpg",
                IsDefault = true,
                ProductId = 4,
            },
            new
            {
                Id = 8,
                ImageName = "04B.jpg",
                IsDefault = false,
                ProductId = 4,
            },
            new
            {
                Id = 9,
                ImageName = "05A.jpg",
                IsDefault = true,
                ProductId = 5,
            },
            new
            {
                Id = 10,
                ImageName = "05B.jpg",
                IsDefault = false,
                ProductId = 5,
            },
            new
            {
                Id = 11,
                ImageName = "06A.jpg",
                IsDefault = true,
                ProductId = 6,
            },
            new
            {
                Id = 12,
                ImageName = "06B.jpg",
                IsDefault = false,
                ProductId = 6,
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
        public virtual DbSet<ProductImage> ProductImage { get; set; }
    }
}