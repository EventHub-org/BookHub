using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookHub.DAL.DataAccess
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Отримання рядка підключення зі змінної середовища
            var connectionString = "Server=LAPTOP-9SF7UDGK\\BOOKHUB;Database=BookHub;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string not found in environment variables.");
            }

            optionsBuilder.UseSqlServer(connectionString); // Метод UseSqlServer

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
