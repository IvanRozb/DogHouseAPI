﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(RepositoryDbContext))]
    partial class RepositoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Dog", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("TailLength")
                        .HasColumnType("float");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Name");

                    b.ToTable("dogs", (string)null);

                    b.HasData(
                        new
                        {
                            Name = "Neo",
                            Color = "red&amber",
                            TailLength = 22.0,
                            Weight = 32.0
                        },
                        new
                        {
                            Name = "Jessy",
                            Color = "black&white",
                            TailLength = 7.0,
                            Weight = 14.0
                        },
                        new
                        {
                            Name = "Buddy",
                            Color = "golden",
                            TailLength = 18.0,
                            Weight = 45.0
                        },
                        new
                        {
                            Name = "Luna",
                            Color = "gray",
                            TailLength = 12.0,
                            Weight = 28.0
                        },
                        new
                        {
                            Name = "Rocky",
                            Color = "brown",
                            TailLength = 15.0,
                            Weight = 36.0
                        },
                        new
                        {
                            Name = "Bella",
                            Color = "white",
                            TailLength = 10.0,
                            Weight = 20.0
                        },
                        new
                        {
                            Name = "Max",
                            Color = "spotted",
                            TailLength = 14.0,
                            Weight = 30.0
                        },
                        new
                        {
                            Name = "Coco",
                            Color = "chocolate",
                            TailLength = 20.0,
                            Weight = 40.0
                        },
                        new
                        {
                            Name = "Milo",
                            Color = "tan",
                            TailLength = 9.0,
                            Weight = 18.0
                        },
                        new
                        {
                            Name = "Daisy",
                            Color = "fawn",
                            TailLength = 16.0,
                            Weight = 35.0
                        },
                        new
                        {
                            Name = "Bailey",
                            Color = "sable",
                            TailLength = 11.0,
                            Weight = 25.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
