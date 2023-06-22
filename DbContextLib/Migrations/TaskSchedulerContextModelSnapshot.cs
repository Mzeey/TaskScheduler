﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mzeey.DbContextLib;

#nullable disable

namespace DbContextLib.Migrations
{
    [DbContext(typeof(TaskSchedulerContext))]
    partial class TaskSchedulerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Mzeey.Entities.AuthenticationToken", b =>
                {
                    b.Property<int>("TokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TokenId"), 1L, 1);

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TokenId");

                    b.HasIndex("UserId");

                    b.ToTable("AuthenticationTokens");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationSpace", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("OrganisationSpaces");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationUserSpace", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("OrganisationSpaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("RoleId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationSpaceId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("OrganisationUserRoles");
                });

            modelBuilder.Entity("Mzeey.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Mzeey.Entities.TaskItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrganisationSpaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationSpaceId");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Mzeey.Entities.TaskItemComment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("TaskItemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TaskItemId");

                    b.HasIndex("UserId");

                    b.ToTable("TaskItemComments");
                });

            modelBuilder.Entity("Mzeey.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganisationSpaceId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrganisationSpaceId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Mzeey.Entities.AuthenticationToken", b =>
                {
                    b.HasOne("Mzeey.Entities.User", "User")
                        .WithMany("AuthenticationTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationUserSpace", b =>
                {
                    b.HasOne("Mzeey.Entities.OrganisationSpace", "OrganisationSpace")
                        .WithMany("OrganisationUserSpaces")
                        .HasForeignKey("OrganisationSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.User", "User")
                        .WithMany("OrganisationUserSpaces")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("OrganisationSpace");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mzeey.Entities.TaskItem", b =>
                {
                    b.HasOne("Mzeey.Entities.OrganisationSpace", "OrganisationSpace")
                        .WithMany("TaskItems")
                        .HasForeignKey("OrganisationSpaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.User", "User")
                        .WithMany("Tasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrganisationSpace");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mzeey.Entities.TaskItemComment", b =>
                {
                    b.HasOne("Mzeey.Entities.TaskItem", "TaskItem")
                        .WithMany("TaskItemComments")
                        .HasForeignKey("TaskItemId")
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.User", "User")
                        .WithMany("TaskItemComments")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("TaskItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mzeey.Entities.User", b =>
                {
                    b.HasOne("Mzeey.Entities.OrganisationSpace", null)
                        .WithMany("Users")
                        .HasForeignKey("OrganisationSpaceId");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationSpace", b =>
                {
                    b.Navigation("OrganisationUserSpaces");

                    b.Navigation("TaskItems");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Mzeey.Entities.TaskItem", b =>
                {
                    b.Navigation("TaskItemComments");
                });

            modelBuilder.Entity("Mzeey.Entities.User", b =>
                {
                    b.Navigation("AuthenticationTokens");

                    b.Navigation("OrganisationUserSpaces");

                    b.Navigation("TaskItemComments");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
