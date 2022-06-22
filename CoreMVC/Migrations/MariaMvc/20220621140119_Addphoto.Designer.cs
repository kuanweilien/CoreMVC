﻿// <auto-generated />
using System;
using CoreMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CoreMVC.Migrations.MariaMvc
{
    [DbContext(typeof(MariaDBContext))]
    [Migration("20220621140119_Addphoto")]
    partial class Addphoto
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CoreMVC.Models.MovieModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Rating")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.HasKey("Id");

                    b.ToTable("MovieModel");
                });

            modelBuilder.Entity("CoreMVC.Models.PhotoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("PhotoModel");
                });

            modelBuilder.Entity("CoreMVC.Models.StudentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("Sex")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<int?>("StudentNumber")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("StudentModel");
                });
#pragma warning restore 612, 618
        }
    }
}
