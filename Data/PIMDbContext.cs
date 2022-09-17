using Microsoft.EntityFrameworkCore;
using PIM_Dashboard.Models;

namespace PIM_Dashboard.Data
{
    public class PIMDbContext : DbContext
    {
        public PIMDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
