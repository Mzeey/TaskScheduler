using Microsoft.EntityFrameworkCore;
using Mzeey.Entities;
using Microsoft.Extensions.Configuration;

namespace Mzeey.DbContextLib
{
    public class TaskSchedulerContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OrganisationSpace> OrganisationSpaces { get; set; }
        public DbSet<TaskItemComment> TaskItemComments { get; set; }
        public DbSet<OrganisationUserSpace> OrganisationUserSpaces { get; set; }
        public DbSet<AuthenticationToken> AuthenticationTokens { get; set; }


        public TaskSchedulerContext() { }

        public TaskSchedulerContext(DbContextOptions<TaskSchedulerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            // AuthenticationToken
            modelBuilder.Entity<AuthenticationToken>()
                .HasKey(at => at.TokenId);

            modelBuilder.Entity<AuthenticationToken>()
                .HasOne(at => at.User)
                .WithMany(u => u.AuthenticationTokens);

            // OrganisationSpace
            modelBuilder.Entity<OrganisationSpace>()
                .HasKey(os => os.Id);

            modelBuilder.Entity<OrganisationSpace>()
                .HasMany(os => os.TaskItems)
                .WithOne(t => t.OrganisationSpace)
                .HasForeignKey(t => t.OrganisationSpaceId);

            modelBuilder.Entity<OrganisationSpace>()
                .HasMany(os => os.OrganisationUserSpaces)
                .WithOne(ur => ur.OrganisationSpace)
                .HasForeignKey(ur => ur.OrganisationSpaceId);

            // OrganisationUserRole
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