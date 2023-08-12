﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDoListAPI.Data;

#nullable disable

namespace ToDoListAPI.Migrations
{
    [DbContext(typeof(ToDoListAPIContext))]
    [Migration("20230812003531_UpdateToDoToDoListIdNull")]
    partial class UpdateToDoToDoListIdNull
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ToDoListAPI.Models.ToDo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Done")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ToDoListId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ToDoListId");

                    b.HasIndex("UserId");

                    b.ToTable("ToDo");
                });

            modelBuilder.Entity("ToDoListAPI.Models.ToDoList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Closed")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ToDoList");
                });

            modelBuilder.Entity("ToDoListAPI.Models.ToDoListCollaborator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ToDoListId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ToDoListId");

                    b.HasIndex("UserId");

                    b.ToTable("ToDoListCollaborator");
                });

            modelBuilder.Entity("ToDoListAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ToDoListId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("ToDoListId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ToDoListAPI.Models.ToDo", b =>
                {
                    b.HasOne("ToDoListAPI.Models.ToDoList", "ToDoList")
                        .WithMany()
                        .HasForeignKey("ToDoListId");

                    b.HasOne("ToDoListAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ToDoList");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ToDoListAPI.Models.ToDoList", b =>
                {
                    b.HasOne("ToDoListAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ToDoListAPI.Models.ToDoListCollaborator", b =>
                {
                    b.HasOne("ToDoListAPI.Models.ToDoList", "ToDoList")
                        .WithMany()
                        .HasForeignKey("ToDoListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ToDoListAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ToDoList");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ToDoListAPI.Models.User", b =>
                {
                    b.HasOne("ToDoListAPI.Models.ToDoList", null)
                        .WithMany("Collaborators")
                        .HasForeignKey("ToDoListId");
                });

            modelBuilder.Entity("ToDoListAPI.Models.ToDoList", b =>
                {
                    b.Navigation("Collaborators");
                });
#pragma warning restore 612, 618
        }
    }
}
