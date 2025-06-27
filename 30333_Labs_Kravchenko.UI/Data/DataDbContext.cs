using _30333_Labs_Kravchenko.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _30333_Labs_Kravchenko.UI.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {

        }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
