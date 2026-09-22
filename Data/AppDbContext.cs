using Microsoft.EntityFrameworkCore;
namespace BenefitEligibilityApi.Data
{
    public class AppDbContext : DbContext
    {
        // This constructor is required by Entity Framework
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // This property tells Entity Framework to create a table called "Applications" in the DB
        public DbSet<BenefitApplication> Applications { get; set; }
    }
}