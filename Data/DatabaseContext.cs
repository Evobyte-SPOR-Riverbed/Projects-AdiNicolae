using Drinktionary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Drinktionary.Data;

public class DatabaseContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DatabaseContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public DbSet<Cocktail> Cocktails { get; set; }

    public DbSet<CocktailIngredient> CocktailIngredients { get; set; }

    public DbSet<Glass> Glasses { get; set; }

    public DbSet<Ingredient> Ingredients { get; set; }

    public DbSet<PreparationMethod> PreparationMethods { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Glass>().HasData(
            new Glass("Old Fashioned Glass"),
            new Glass("Rocks Glass"),
            new Glass("Cosmopolitan Glass"),
            new Glass("Highball Glass"),
            new Glass("Collins Glass"),
            new Glass("Zombie Glass"),
            new Glass("Weizen Glass"),
            new Glass("Pilsner Glass"),
            new Glass("Sling Glass"),
            new Glass("Pint Glass"),
            new Glass("Beer Mug"),
            new Glass("Irish Coffee Glass"),
            new Glass("Red Wine Glass"),
            new Glass("White Wine Glass"),
            new Glass("Balloon Wine Glass"),
            new Glass("Wine Tasting Glass"),
            new Glass("Sherry Glass"),
            new Glass("Champagne Flute"),
            new Glass("Champahne Coupe"),
            new Glass("Martini Glass"),
            new Glass("Margarite Glass"),
            new Glass("Goblet Glass"),
            new Glass("Pokal Glass"),
            new Glass("Milkshake Glass"),
            new Glass("Hurricane Glass"),
            new Glass("Poco Grande Glass"),
            new Glass("Brandy Sniffer"),
            new Glass("Wobble Cognac Glass"),
            new Glass("Tulip Whisky Glass"),
            new Glass("Tulip Glass"),
            new Glass("Grappa Glass"),
            new Glass("Pousse Cafe Glass"),
            new Glass("Cordial Glass"),
            new Glass("Absinthe Glass"),
            new Glass("Vodka Glass"),
            new Glass("Shooter Glass"),
            new Glass("Shot Glass")
        );

        modelBuilder.Entity<PreparationMethod>().HasData(
            new PreparationMethod("Building Method", "It is made by pouring the ingredients one by one into the glass in which it is to be served and then stirred. Ice is added if the recipe calls for it."),
            new PreparationMethod("Stirring Method", "Stirring refers to the mixing of the ingredients with ice, by stirring quickly in a mixing glass with the stirrer and then straining it into the appropriate glass."),
            new PreparationMethod("Shaking Method", "It is the mixing of ingredients thoroughly with ice by shaking them in a cocktail shaker and straining them into the appropriate glass."),
            new PreparationMethod("Blending Method", "The blending method of mixing a cocktail is used for combining fruits, solid foods, ice, etc. in an electric blender. Any drink that can be shaken may be made by blending as well."),
            new PreparationMethod("Layering Method", "The layering cocktail-making method is used when the ingredients used are of a different color, flavor, and sensitize. One ingredient is floated over the other by pouring gently over the back of a spoon into a small straight-sided glass.")
        );

        modelBuilder.Entity<Cocktail>()
            .HasMany(c => c.Ingredients)
            .WithMany(i => i.Cocktails)
            .UsingEntity<CocktailIngredient>(
                j => j
                    .HasOne(ci => ci.Ingredient)
                    .WithMany(i => i.CocktailIngredients)
                    .HasForeignKey(ci => ci.IngredientId),
                j => j
                    .HasOne(ci => ci.Cocktail)
                    .WithMany(c => c.CocktailIngredients)
                    .HasForeignKey(ci => ci.CocktailId),
                j =>
                {
                    j.Property(ci => ci.Quantity);
                    j.Property(ci => ci.Unit);
                    j.HasKey(ci => new { ci.CocktailId, ci.IngredientId });
                });
    }
}