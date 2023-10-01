﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TomAntillWebDevServices.Data;

namespace TomAntillWebDevServices.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230926203319_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("TomAntillWebDevServices.Data.Auth.DataModels.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.Auth.DataModels.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.Auth.DataModels.UserSite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebsiteName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSite");
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.DataModels.BasePicture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<string>("PictureCategory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WebsiteName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BasePicture");
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.DataModels.Email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EmailSettingsId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebsiteName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmailSettingsId");

                    b.ToTable("Email");
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.DataModels.EmailSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Template")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebsiteName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EmailSettings");
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.Auth.DataModels.UserRole", b =>
                {
                    b.HasOne("TomAntillWebDevServices.Data.Auth.DataModels.User", null)
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.Auth.DataModels.UserSite", b =>
                {
                    b.HasOne("TomAntillWebDevServices.Data.Auth.DataModels.User", null)
                        .WithMany("UserSites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.DataModels.Email", b =>
                {
                    b.HasOne("TomAntillWebDevServices.Data.DataModels.EmailSettings", "EmailSettings")
                        .WithMany()
                        .HasForeignKey("EmailSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmailSettings");
                });

            modelBuilder.Entity("TomAntillWebDevServices.Data.Auth.DataModels.User", b =>
                {
                    b.Navigation("UserRoles");

                    b.Navigation("UserSites");
                });
#pragma warning restore 612, 618
        }
    }
}
