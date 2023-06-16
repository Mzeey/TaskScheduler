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
        public DbSet<AuthenticationToken> AuthenticationTokens { get; set; }

        public TaskSchedulerContext() { }

        public TaskSchedulerContext(DbContextOptions<TaskSchedulerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks)               
                .WithOne(t => t.User)                
                .HasForeignKey(t => t.UserId);       

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)                 
                .WithMany(r => r.Users)              
                .HasForeignKey(u => u.RoleId);
        }
    }
}