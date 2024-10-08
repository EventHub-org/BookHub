using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;
using System.IO; 

namespace BookHub.DAL.DataAccess
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            string envPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\.env");

            DotNetEnv.Env.Load(envPath);
            Env.Load();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string not found in environment variables.");
            }

            optionsBuilder.UseSqlServer(connectionString); 

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
