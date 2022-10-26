﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PIM_Dashboard.Data;

#nullable disable

namespace PIM_Dashboard.Migrations
{
    [DbContext(typeof(PIMDbContext))]
    partial class PIMDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PIM_Dashboard.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemId"), 1L, 1);

                    b.Property<string>("ItemBaseColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemBrandColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ItemCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("ItemEngineType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemFoodGroup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemForceSend")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ItemNutritionalFacts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemPackageQuantity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemPackageType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemQuantityType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("ItemRetailPrice")
                        .HasColumnType("float");

                    b.Property<string>("ItemServiceInterval")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemSize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("ResourceFileName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ResourceImageTitle")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ItemId");

                    b.HasIndex("ItemName")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("PIM_Dashboard.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<DateTime>("ProductCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProductLifecycleStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("ProductLongDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductManager")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ProductShortDescription")
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("ResourceFileName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ResourceImageTitle")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ProductId");

                    b.HasIndex("ProductName")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PIM_Dashboard.Models.Resource", b =>
                {
                    b.Property<int>("ResourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResourceId"), 1L, 1);

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("ResourceFileName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ResourceImageTitle")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ResourceId");

                    b.HasIndex("ProductId");

                    b.ToTable("Resource");
                });

            modelBuilder.Entity("PIM_Dashboard.Models.Item", b =>
                {
                    b.HasOne("PIM_Dashboard.Models.Product", "Product")
                        .WithMany("Items")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PIM_Dashboard.Models.Resource", b =>
                {
                    b.HasOne("PIM_Dashboard.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PIM_Dashboard.Models.Product", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
