﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Millionandup.MsProperty.Infrastructure.Repository.Contexts;

#nullable disable

namespace Millionandup.MsProperty.Infrastructure.Migrations
{
    [DbContext(typeof(PropertyContext))]
    partial class PropertyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Millionandup.MsProperty.Domain.AggregatesModel.Owner", b =>
                {
                    b.Property<Guid>("OwnerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Owner ID");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(125)")
                        .HasComment("Owner Address");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("date")
                        .HasComment("Owner Birthday");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasComment("Owner name");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnType("nvarchar(2048)")
                        .HasComment("Owner Photo URL");

                    b.HasKey("OwnerId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Owner", "dbo");
                });

            modelBuilder.Entity("Millionandup.MsProperty.Domain.AggregatesModel.Property", b =>
                {
                    b.Property<Guid>("PropertyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Property ID");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(125)")
                        .HasComment("Property Address");

                    b.Property<string>("CodeInternal")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Code Internal");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasComment("Property name");

                    b.Property<decimal>("Price")
                        .HasColumnType("money")
                        .HasComment("Property Price");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasComment("Property Age in years");

                    b.HasKey("PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("Property", "dbo");
                });

            modelBuilder.Entity("Millionandup.MsProperty.Domain.AggregatesModel.PropertyImage", b =>
                {
                    b.Property<Guid>("PropertyImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Property image ID");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit")
                        .HasComment("Log is eneable");

                    b.Property<string>("File")
                        .IsRequired()
                        .HasColumnType("nvarchar(2048)")
                        .HasComment("Date of the sale");

                    b.HasKey("PropertyImageId");

                    b.HasIndex("PropertyImageId");

                    b.ToTable("PropertyImage", "dbo");
                });

            modelBuilder.Entity("Millionandup.MsProperty.Domain.AggregatesModel.PropertyTrace", b =>
                {
                    b.Property<Guid>("PropertyTraceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Trace identifier");

                    b.Property<DateTime>("DateSale")
                        .HasColumnType("datetime")
                        .HasComment("Date of the sale");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasComment("Property name");

                    b.Property<Guid>("PropertyId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Property ID");

                    b.Property<decimal>("Tax")
                        .HasColumnType("money")
                        .HasComment("Taxes value");

                    b.Property<decimal>("Value")
                        .HasColumnType("money")
                        .HasComment("Property value");

                    b.HasKey("PropertyTraceId");

                    b.HasIndex("DateSale")
                        .IsDescending();

                    b.HasIndex("PropertyId");

                    b.HasIndex("PropertyTraceId");

                    b.ToTable("PropertyTrace", "dbo");
                });

            modelBuilder.Entity("OwnerByProperty", b =>
                {
                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PropertyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OwnerId", "PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("OwnerByProperty", "dbo");
                });

            modelBuilder.Entity("PropertyImageByProperty", b =>
                {
                    b.Property<Guid>("PropertyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PropertyImageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PropertyId", "PropertyImageId");

                    b.HasIndex("PropertyImageId");

                    b.ToTable("PropertyImageByProperty", "dbo");
                });

            modelBuilder.Entity("Millionandup.MsProperty.Domain.AggregatesModel.PropertyTrace", b =>
                {
                    b.HasOne("Millionandup.MsProperty.Domain.AggregatesModel.Property", "Property")
                        .WithMany("PropertyTrace")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");
                });

            modelBuilder.Entity("OwnerByProperty", b =>
                {
                    b.HasOne("Millionandup.MsProperty.Domain.AggregatesModel.Owner", null)
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Millionandup.MsProperty.Domain.AggregatesModel.Property", null)
                        .WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PropertyImageByProperty", b =>
                {
                    b.HasOne("Millionandup.MsProperty.Domain.AggregatesModel.Property", null)
                        .WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Millionandup.MsProperty.Domain.AggregatesModel.PropertyImage", null)
                        .WithMany()
                        .HasForeignKey("PropertyImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Millionandup.MsProperty.Domain.AggregatesModel.Property", b =>
                {
                    b.Navigation("PropertyTrace");
                });
#pragma warning restore 612, 618
        }
    }
}
