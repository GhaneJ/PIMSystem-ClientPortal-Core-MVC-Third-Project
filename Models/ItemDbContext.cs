using Microsoft.EntityFrameworkCore;

namespace PIMSystemITEMCRUD.Models
{
    public class ItemDbContext : DbContext
    {
        public ItemDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; }
    }
}
