using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Coachify.DAL
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Используй путь к базе из OnConfiguring, или пропиши явный путь
            optionsBuilder.UseSqlite("Data Source=..\\Coachify.DAL\\coachify.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}