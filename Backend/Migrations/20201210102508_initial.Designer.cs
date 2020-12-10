﻿// <auto-generated />
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20201210102508_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

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
#pragma warning restore 612, 618
        }
    }
}
