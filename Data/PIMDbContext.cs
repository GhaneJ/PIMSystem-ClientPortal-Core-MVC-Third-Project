using Microsoft.EntityFrameworkCore;
using PIMSystemITEMCRUD.Models;

namespace PIMSystemITEMCRUD.Data
{
    public class PIMDbContext : DbContext
    {
        public PIMDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
