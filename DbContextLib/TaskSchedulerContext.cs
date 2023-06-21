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
        public DbSet<OrganisationUserRole> OrganisationUserRoles { get; set; }
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
                .HasMany(os => os.OrganisationUserRoles)
                .WithOne(ur => ur.OrganisationSpace)
                .HasForeignKey(ur => ur.OrganisationSpaceId);

            // OrganisationUserRole
            modelBuilder.Entity<OrganisationUserRole>()
                .HasKey(ur => ur.Id);

            modelBuilder.Entity<OrganisationUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.OrganisationUserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<OrganisationUserRole>()
                .HasOne(ur => ur.OrganisationSpace)
                .WithMany(os => os.OrganisationUserRoles)
                .HasForeignKey(ur => ur.OrganisationSpaceId);

            modelBuilder.Entity<OrganisationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId);

            // Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.OrganisationUserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

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

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            base.OnModelCreating(modelBuilder);
        }

    }
}