﻿// <auto-generated />
using Calculator.API.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Calculator.API.Migrations
{
    [DbContext(typeof(CalculationExpressionRecordContext))]
    partial class CalculationExpressionRecordContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Calculator.API.DTO.CalculationExpressionRecord", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Record")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("CalculationExpressionRecord");
                });
#pragma warning restore 612, 618
        }
    }
}