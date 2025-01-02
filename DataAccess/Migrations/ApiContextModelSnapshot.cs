﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApiContext))]
    partial class ApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Entity.UrlInfo", b =>
                {
                    b.Property<int>("RecordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("InsertDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsShortUrlSpecified")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LongUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SpecifiedShortUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RecordId");

                    b.ToTable("UrlInfos");
                });
#pragma warning restore 612, 618
        }
    }
}