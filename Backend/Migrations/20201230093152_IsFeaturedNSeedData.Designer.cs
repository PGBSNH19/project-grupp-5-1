﻿// <auto-generated />
using System;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20201230093152_IsFeaturedNSeedData")]
    partial class IsFeaturedNSeedData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Backend.Models.Coupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(5,4)");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Coupon");
                });

            modelBuilder.Entity("Backend.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("CouponId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateRegistered")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CouponId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Backend.Models.OrderedProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderedProduct");
                });

            modelBuilder.Entity("Backend.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFeatured")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductCategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Stock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Product");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Pair these Samsung Galaxy Buds True Wireless Earbuds with your device and go.",
                            IsAvailable = true,
                            IsFeatured = false,
                            Name = "SAMSUNG Galaxy Buds",
                            ProductCategoryId = 1,
                            Stock = 65
                        },
                        new
                        {
                            Id = 2,
                            Description = "Stay on top of your fitness with this Apple Watch Series 3.",
                            IsAvailable = true,
                            IsFeatured = true,
                            Name = "Apple Watch Series 3 GPS",
                            ProductCategoryId = 1,
                            Stock = 34
                        },
                        new
                        {
                            Id = 3,
                            Description = "Meet the 2nd generation Nest Mini, the speaker you control with your voice.",
                            IsAvailable = true,
                            IsFeatured = false,
                            Name = "Google Nest Mini (2nd Generation) - Charcoal",
                            ProductCategoryId = 1,
                            Stock = 11
                        },
                        new
                        {
                            Id = 4,
                            Description = "Keep your favorite songs, photos, videos and games, thanks to 32GB of built-in memory.",
                            IsAvailable = true,
                            IsFeatured = false,
                            Name = "SAMSUNG Galaxy Tab A 8.0\" 32 GB",
                            ProductCategoryId = 1,
                            Stock = 23
                        },
                        new
                        {
                            Id = 5,
                            Description = "At less than four pounds, this thin and light silver Chromebook laptop is easy to take from room to room or on the road.",
                            IsAvailable = true,
                            IsFeatured = true,
                            Name = "HP 14\" Pentium 4GB / 64GB Chromebook",
                            ProductCategoryId = 1,
                            Stock = 43
                        },
                        new
                        {
                            Id = 6,
                            Description = "Enjoy all-day comfort and distraction-free music with the new Studio ANC.",
                            IsAvailable = false,
                            IsFeatured = false,
                            Name = "JLab Audio Studio ANC On-Ear",
                            ProductCategoryId = 1,
                            Stock = 37
                        });
                });

            modelBuilder.Entity("Backend.Models.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductCategory");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Uncategorized"
                        });
                });

            modelBuilder.Entity("Backend.Models.ProductPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateChanged")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<decimal?>("SalePrice")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPrice");
                });

            modelBuilder.Entity("Backend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "Ahmad",
                            LastName = "Yassin",
                            Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                            Role = 1,
                            Username = "ayassin"
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "Andre",
                            LastName = "Morad",
                            Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                            Role = 1,
                            Username = "amorad"
                        },
                        new
                        {
                            Id = 3,
                            FirstName = "Nor",
                            LastName = "Shiervani",
                            Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                            Role = 1,
                            Username = "nshiervani"
                        },
                        new
                        {
                            Id = 4,
                            FirstName = "Irvin",
                            LastName = "Perez",
                            Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                            Role = 1,
                            Username = "iperez"
                        },
                        new
                        {
                            Id = 5,
                            FirstName = "Micael",
                            LastName = "Wolter",
                            Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                            Role = 1,
                            Username = "mwolter"
                        },
                        new
                        {
                            Id = 6,
                            FirstName = "Jim",
                            LastName = "Bob",
                            Password = "7c4a8d09ca3762af61e59520943dc26494f8941b",
                            Role = 2,
                            Username = "customerjim"
                        });
                });

            modelBuilder.Entity("Backend.Models.Weather", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Weather");

                    b.HasData(
                        new
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
                });

            modelBuilder.Entity("Backend.Models.Order", b =>
                {
                    b.HasOne("Backend.Models.Coupon", "Coupon")
                        .WithMany("Orders")
                        .HasForeignKey("CouponId");

                    b.HasOne("Backend.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coupon");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Models.OrderedProduct", b =>
                {
                    b.HasOne("Backend.Models.Order", "Order")
                        .WithMany("OrderedProduct")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Product", "Product")
                        .WithMany("OrderedProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Backend.Models.Product", b =>
                {
                    b.HasOne("Backend.Models.ProductCategory", "ProductCategory")
                        .WithMany("Products")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("Backend.Models.ProductPrice", b =>
                {
                    b.HasOne("Backend.Models.Product", "Product")
                        .WithMany("ProductPrices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Backend.Models.Coupon", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Backend.Models.Order", b =>
                {
                    b.Navigation("OrderedProduct");
                });

            modelBuilder.Entity("Backend.Models.Product", b =>
                {
                    b.Navigation("OrderedProducts");

                    b.Navigation("ProductPrices");
                });

            modelBuilder.Entity("Backend.Models.ProductCategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Backend.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
