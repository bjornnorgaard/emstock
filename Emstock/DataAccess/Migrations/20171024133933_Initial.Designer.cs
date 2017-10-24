﻿// <auto-generated />
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Models.Enums;
using System;

namespace DataAccess.Migrations
{
    [DbContext(typeof(EmstockContext))]
    [Migration("20171024133933_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Models.CategoryType", b =>
                {
                    b.Property<long>("CategoryId");

                    b.Property<long>("TypeId");

                    b.HasKey("CategoryId", "TypeId");

                    b.HasIndex("TypeId");

                    b.ToTable("CategoryType");
                });

            modelBuilder.Entity("Models.Component", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdminComment");

                    b.Property<int>("ComponentTypeId");

                    b.Property<long?>("CurrentLoanInformationId");

                    b.Property<int>("Number");

                    b.Property<string>("SerialNo");

                    b.Property<int>("Status");

                    b.Property<long?>("TypeId");

                    b.Property<string>("UserComment");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("Models.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ImageData");

                    b.Property<string>("ImageMimeType")
                        .HasMaxLength(128);

                    b.Property<byte[]>("Thumbnail");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Models.Type", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdminComment");

                    b.Property<string>("Datasheet");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Info");

                    b.Property<string>("Location");

                    b.Property<string>("Manufacturer");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<string>("WikiLink");

                    b.HasKey("Id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("Models.CategoryType", b =>
                {
                    b.HasOne("Models.Category", "Category")
                        .WithMany("CategoryTypes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Models.Type", "Type")
                        .WithMany("CategoryTypes")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Models.Component", b =>
                {
                    b.HasOne("Models.Type", "Type")
                        .WithMany("Components")
                        .HasForeignKey("TypeId");
                });
#pragma warning restore 612, 618
        }
    }
}
