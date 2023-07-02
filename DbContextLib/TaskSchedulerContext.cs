using Microsoft.EntityFrameworkCore;
using Mzeey.Entities;
using Microsoft.Extensions.Configuration;

namespace Mzeey.DbContextLib
{
    public class TaskSchedulerContext : DbContext
    {

        
        public DbSet<AuthenticationToken> AuthenticationTokens { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSetting> NotificationSettings { get; set; }
        public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<OrganisationSpace> OrganisationSpaces { get; set; }
        public DbSet<OrganisationSpaceInvitation> OrganisationSpaceInvitations { get; set; }
        public DbSet<OrganisationUserSpace> OrganisationUserSpaces { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<TaskItemComment> TaskItemComments { get; set; }
        public DbSet<User> Users { get; set; }


        public TaskSchedulerContext() { }

        public TaskSchedulerContext(DbContextOptions<TaskSchedulerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // AuthenticationToken
            modelBuilder.Entity<AuthenticationToken>()
                .HasKey(at => at.Id);

            modelBuilder.Entity<AuthenticationToken>()
                .HasOne(at => at.User)
                .WithMany(u => u.AuthenticationTokens);

            //Notification
            modelBuilder.Entity<Notification>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Recipient)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.NotificationType)
                .WithMany(nt => nt.Notifications)
                .HasForeignKey(n => n.NotificationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            // NotificationSetting
            modelBuilder.Entity<NotificationSetting>()
                .HasKey(ns => ns.Id);

            modelBuilder.Entity<NotificationSetting>()
                .HasOne(ns => ns.User)
                .WithMany(u => u.NotificationSettings)
                .HasForeignKey(ns => ns.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<NotificationSetting>()
                .HasOne(ns => ns.NotificationType)
                .WithMany(nt => nt.NotificationSettings)
                .HasForeignKey(ns => ns.NotificationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            //NotificationTemplate
            modelBuilder.Entity<NotificationTemplate>()
                .HasKey(ntp => ntp.Id);

            modelBuilder.Entity<NotificationTemplate>()
                .HasOne(ntp => ntp.NotificationType)
                .WithMany(nt => nt.NotificationTemplates)
                .HasForeignKey(ntp => ntp.NotificationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // NotificationType
            modelBuilder.Entity<NotificationType>()
                .HasKey(nty => nty.Id);

            // OrganisationSpace
            modelBuilder.Entity<OrganisationSpace>()
                .HasKey(os => os.Id);

            modelBuilder.Entity<OrganisationSpace>()
                .HasMany(os => os.TaskItems)
                .WithOne(t => t.OrganisationSpace)
                .HasForeignKey(t => t.OrganisationSpaceId);

            modelBuilder.Entity<OrganisationSpace>()
                .HasMany(os => os.OrganisationUserSpaces)
                .WithOne(ous => ous.OrganisationSpace)
                .HasForeignKey(os => os.OrganisationSpaceId);

            //OrganisationSpaceInvitation
            modelBuilder.Entity<OrganisationSpaceInvitation>()
                .HasKey(osi => osi.Id);

            modelBuilder.Entity<OrganisationSpaceInvitation>()
                .HasOne(osi => osi.OrganisationSpace)
                .WithMany(os => os.OrganisationSpaceInvitations)
                .HasForeignKey(osi => osi.OrganisationSpaceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<OrganisationSpaceInvitation>()
                .HasOne(osi => osi.Inviter)
                .WithMany(u => u.SentOrganisationSpaceInvitations)
                .HasForeignKey(osi => osi.InviterId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<OrganisationSpaceInvitation>()
                .HasOne(osi => osi.Invitee)
                .WithMany(u => u.ReceivedOrganisationSpaceInvitations)
                .HasForeignKey(osi => osi.InviteeId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<OrganisationSpaceInvitation>()
                .HasOne(osi => osi.Role)
                .WithMany(r => r.OrganisationSpaceInvitations)
                .HasForeignKey(osi => osi.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull);



            // OrganisationUserSpace
            modelBuilder.Entity<OrganisationUserSpace>()
                .HasKey(os => os.Id);

            modelBuilder.Entity<OrganisationUserSpace>()
                .HasOne(ous => ous.User)
                .WithMany(u => u.OrganisationUserSpaces)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<OrganisationUserSpace>()
                .HasOne(ous => ous.OrganisationSpace)
                .WithMany(os => os.OrganisationUserSpaces)
                .HasForeignKey(ur => ur.OrganisationSpaceId);

            modelBuilder.Entity<OrganisationUserSpace>()
                .HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);

            //PasswordResetToken
            modelBuilder.Entity<PasswordResetToken>()
                .HasKey(p => p.Id );

            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(p => p.User)
                .WithMany(u => u.PasswordResetTokens)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            // TaskItem
            modelBuilder.Entity<TaskItem>()
                .HasKey(ti => ti.Id);

            modelBuilder.Entity<TaskItem>()
                .HasOne(ti => ti.OrganisationSpace)
                .WithMany(os => os.TaskItems)
                .HasForeignKey(ti => ti.OrganisationSpaceId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(ti => ti.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(ti => ti.UserId);

            //TaskAssignments
            modelBuilder.Entity<TaskAssignment>()
                .HasKey(ta => ta.Id);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.TaskItem)
                .WithMany(ti => ti.TaskAssignments)
                .HasForeignKey(ta => ta.TaskItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Assigner)
                .WithMany(u => u.AssignedTaskAssignments)
                .HasForeignKey(ta => ta.AssignerId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Assignee)
                .WithMany(u => u.ReceivedTaskAssignments)
                .HasForeignKey(ta => ta.AssigneeId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.ClientSetNull);



            // TaskItemComment
            modelBuilder.Entity<TaskItemComment>()
                .HasKey(tc => tc.Id);

            modelBuilder.Entity<TaskItemComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.TaskItemComments)
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<TaskItemComment>()
                .HasOne(tc => tc.TaskItem)
                .WithMany(ti => ti.TaskItemComments)
                .HasForeignKey(tc => tc.TaskItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);



            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.AuthenticationTokens)
                .WithOne(at => at.User);

            base.OnModelCreating(modelBuilder);
        }

    }
}