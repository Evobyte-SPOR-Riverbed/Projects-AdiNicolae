using Drinktionary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Drinktionary.Data;

public class DatabaseContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DatabaseContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public DbSet<Cocktail> Cocktails { get; set; }

    public DbSet<Glass> Glasses { get; set; }

    public DbSet<Ingredient> Ingredients { get; set; }

    public DbSet<PreparationMethod> PreparationMethods { get; set; }

    public DbSet<Review> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
    }
}