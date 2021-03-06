// <auto-generated />
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
    [Migration("20220707153104_UserLocked")]
    partial class UserLocked
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

                    b.Property<string>("FileName")
                        .HasColumnType("longtext");

                    b.Property<string>("FilePath")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Image")
                        .HasColumnType("longblob");

                    b.Property<string>("ImageName")
                        .HasColumnType("longtext");

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

            modelBuilder.Entity("CoreMVC.Models.User.GroupModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("GroupModel");
                });

            modelBuilder.Entity("CoreMVC.Models.User.RoleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RoleName")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserModelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserModelId");

                    b.ToTable("RoleModel");
                });

            modelBuilder.Entity("CoreMVC.Models.User.UserGroupModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserGroupModel");
                });

            modelBuilder.Entity("CoreMVC.Models.User.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(240)
                        .HasColumnType("varchar(240)");

                    b.Property<bool?>("Locked")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("Sex")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("UserModel");
                });

            modelBuilder.Entity("CoreMVC.Models.User.GroupModel", b =>
                {
                    b.HasOne("CoreMVC.Models.User.UserModel", null)
                        .WithMany("GroupModels")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("CoreMVC.Models.User.RoleModel", b =>
                {
                    b.HasOne("CoreMVC.Models.User.UserModel", null)
                        .WithMany("Roles")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("CoreMVC.Models.User.UserModel", b =>
                {
                    b.Navigation("GroupModels");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
