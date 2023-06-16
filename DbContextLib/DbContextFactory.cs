using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Mzeey.DbContextLib
{
    public class DbContextFactory : IDesignTimeDbContextFactory<TaskSchedulerContext>
    {
        public TaskSchedulerContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var optionsBuilder = new DbContextOptionsBuilder<TaskSchedulerContext>();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            return new TaskSchedulerContext(optionsBuilder.Options);

        }
    }
}
