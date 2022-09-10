﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PIMSystemITEMCRUD.Models;

#nullable disable

namespace PIMSystemITEMCRUD.Migrations
{
    [DbContext(typeof(ItemDbContext))]
    [Migration("20220909164004_Initialize Db")]
    partial class InitializeDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PIMSystemITEMCRUD.Models.Item", b =>
                {
                    b.Property<string>("ItemName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ItemCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("ItemImageName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ItemImageTitle")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ItemPackageType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ItemPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ItemName");

                    b.ToTable("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
