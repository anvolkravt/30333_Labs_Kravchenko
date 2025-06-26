using Microsoft.EntityFrameworkCore;
using _30333_Labs_Kravchenko.Domain.Entities;

namespace _30333_Labs_Kravchenko.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Medication> Medications { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}