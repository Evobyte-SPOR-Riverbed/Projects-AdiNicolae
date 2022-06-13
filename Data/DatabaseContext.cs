using Microsoft.EntityFrameworkCore;
using RestAPIApp.Models;

namespace RestAPIApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Cocktail> Cocktails { get; set; }
    }
}
