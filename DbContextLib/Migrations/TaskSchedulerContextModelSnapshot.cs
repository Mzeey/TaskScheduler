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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

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

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AuthenticationTokens");
                });

            modelBuilder.Entity("Mzeey.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<int>("NotificationTypeId")
                        .HasColumnType("int");

                    b.Property<string>("RecipientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId");

                    b.HasIndex("RecipientId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Mzeey.Entities.NotificationSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("NotificationTypeId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("NotificationSettings");
                });

            modelBuilder.Entity("Mzeey.Entities.NotificationTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NotificationTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NotificationTypeId");

                    b.ToTable("NotificationTemplates");
                });

            modelBuilder.Entity("Mzeey.Entities.NotificationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificationTypes");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationSpace", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("OrganisationSpaces");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationSpaceInvitation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("InvitationStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvitationToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InviteeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InviterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OrganisationSpaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InviteeId");

                    b.HasIndex("InviterId");

                    b.HasIndex("OrganisationSpaceId");

                    b.HasIndex("RoleId");

                    b.ToTable("OrganisationSpaceInvitations");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationUserSpace", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

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

                    b.ToTable("OrganisationUserSpaces");
                });

            modelBuilder.Entity("Mzeey.Entities.PasswordResetToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordResetTokens");
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

            modelBuilder.Entity("Mzeey.Entities.TaskAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AssigneeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AssignerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AssignmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TaskItemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("AssignerId");

                    b.HasIndex("TaskItemId");

                    b.ToTable("TaskAssignments");
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

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

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

            modelBuilder.Entity("Mzeey.Entities.Notification", b =>
                {
                    b.HasOne("Mzeey.Entities.NotificationType", "NotificationType")
                        .WithMany("Notifications")
                        .HasForeignKey("NotificationTypeId")
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.User", "Recipient")
                        .WithMany("Notifications")
                        .HasForeignKey("RecipientId")
                        .IsRequired();

                    b.Navigation("NotificationType");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("Mzeey.Entities.NotificationSetting", b =>
                {
                    b.HasOne("Mzeey.Entities.NotificationType", "NotificationType")
                        .WithMany("NotificationSettings")
                        .HasForeignKey("NotificationTypeId")
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.User", "User")
                        .WithMany("NotificationSettings")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("NotificationType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mzeey.Entities.NotificationTemplate", b =>
                {
                    b.HasOne("Mzeey.Entities.NotificationType", "NotificationType")
                        .WithMany("NotificationTemplates")
                        .HasForeignKey("NotificationTypeId")
                        .IsRequired();

                    b.Navigation("NotificationType");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationSpace", b =>
                {
                    b.HasOne("Mzeey.Entities.User", "Creator")
                        .WithMany("CreatedOrganisationSpaces")
                        .HasForeignKey("CreatorId")
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationSpaceInvitation", b =>
                {
                    b.HasOne("Mzeey.Entities.User", "Invitee")
                        .WithMany("ReceivedOrganisationSpaceInvitations")
                        .HasForeignKey("InviteeId")
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.User", "Inviter")
                        .WithMany("SentOrganisationSpaceInvitations")
                        .HasForeignKey("InviterId")
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.OrganisationSpace", "OrganisationSpace")
                        .WithMany("OrganisationSpaceInvitations")
                        .HasForeignKey("OrganisationSpaceId")
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.Role", "Role")
                        .WithMany("OrganisationSpaceInvitations")
                        .HasForeignKey("RoleId")
                        .IsRequired();

                    b.Navigation("Invitee");

                    b.Navigation("Inviter");

                    b.Navigation("OrganisationSpace");

                    b.Navigation("Role");
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

            modelBuilder.Entity("Mzeey.Entities.PasswordResetToken", b =>
                {
                    b.HasOne("Mzeey.Entities.User", "User")
                        .WithMany("PasswordResetTokens")
                        .HasForeignKey("UserId")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mzeey.Entities.TaskAssignment", b =>
                {
                    b.HasOne("Mzeey.Entities.User", "Assignee")
                        .WithMany("ReceivedTaskAssignments")
                        .HasForeignKey("AssigneeId")
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.User", "Assigner")
                        .WithMany("AssignedTaskAssignments")
                        .HasForeignKey("AssignerId")
                        .IsRequired();

                    b.HasOne("Mzeey.Entities.TaskItem", "TaskItem")
                        .WithMany("TaskAssignments")
                        .HasForeignKey("TaskItemId")
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Assigner");

                    b.Navigation("TaskItem");
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

            modelBuilder.Entity("Mzeey.Entities.NotificationType", b =>
                {
                    b.Navigation("NotificationSettings");

                    b.Navigation("NotificationTemplates");

                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("Mzeey.Entities.OrganisationSpace", b =>
                {
                    b.Navigation("OrganisationSpaceInvitations");

                    b.Navigation("OrganisationUserSpaces");

                    b.Navigation("TaskItems");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Mzeey.Entities.Role", b =>
                {
                    b.Navigation("OrganisationSpaceInvitations");
                });

            modelBuilder.Entity("Mzeey.Entities.TaskItem", b =>
                {
                    b.Navigation("TaskAssignments");

                    b.Navigation("TaskItemComments");
                });

            modelBuilder.Entity("Mzeey.Entities.User", b =>
                {
                    b.Navigation("AssignedTaskAssignments");

                    b.Navigation("AuthenticationTokens");

                    b.Navigation("CreatedOrganisationSpaces");

                    b.Navigation("NotificationSettings");

                    b.Navigation("Notifications");

                    b.Navigation("OrganisationUserSpaces");

                    b.Navigation("PasswordResetTokens");

                    b.Navigation("ReceivedOrganisationSpaceInvitations");

                    b.Navigation("ReceivedTaskAssignments");

                    b.Navigation("SentOrganisationSpaceInvitations");

                    b.Navigation("TaskItemComments");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
