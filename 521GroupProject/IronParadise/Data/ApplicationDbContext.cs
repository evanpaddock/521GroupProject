using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IronParadise.Models;

namespace IronParadise.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<IronParadise.Models.Workout>? Workout { get; set; }
        public DbSet<IronParadise.Models.FitnessInfo>? FitnessInfo { get; set; }
        public DbSet<IronParadise.Models.Exercise>? Exercise { get; set; }
    }
}