using Microsoft.EntityFrameworkCore;
using Coachify.DAL.Entities;

namespace Coachify.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Course> Course { get; set; }
        
    }
}