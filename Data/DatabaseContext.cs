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
            new Glass("Old Fashioned Glass", "A short tumbler used for serving spirits, such as whisky, neat or with ice cubes."),
            new Glass("Cosmopolitan Glass", "The elegant answer to serving your most popular cocktails and other mixed drinks."),
            new Glass("Highball Glass", "Barware with straight sides and a flat base or footed stem."),
            new Glass("Collins Glass", "Cylindrical in shape and narrower and taller than a highball glass."),
            new Glass("Zombie Glass", "The tallest and most narrow of all the cocktail glasses, which helps to highlight colorful drinks."),
            new Glass("Weizen Glass", "A tall glass with a narrow base and walls that flare out slightly."),
            new Glass("Pilsner Glass", "Tall and slim, made of thin glass and tapered in shape."),
            new Glass("Sling Glass", "A tall glass that is narrower at the bottom and wider at the top."),
            new Glass("Pint Glass", "Has a simple and somewhat skinny cylindrical shape that gets wider as it goes up."),
            new Glass("Beer Mug", "A glass of a standard size with a handle, to drink beer from."),
            new Glass("Irish Coffee Glass", "A type of liqueur glass which has straight sides and is designed for drinks prepared using the pousse-café method."),
            new Glass("Red Wine Glass", "Large with full, round bowls and big openings, which enable you to enjoy the aroma of the wine."),
            new Glass("White Wine Glass", "Will have a more acute bow within the bowl, and a smaller opening at the rim of the glass."),
            new Glass("Balloon Wine Glass", "Characterized by an almost spherical shape that is much more rounded than other types of wine glass."),
            new Glass("Sherry Glass", "It has a small bowl and narrow mouth to help trap the complex aromas of the dry, nutty wine."),
            new Glass("Champagne Flute", "It is normally a long narrow glass with a long stem, both about equal in length."),
            new Glass("Champagne Coupe", "It is shallow, broad-rimmed, and was stemmed in design."),
            new Glass("Martini Glass", "It features a \"v\" shaped bowl design that requires the drink to be sipped."),
            new Glass("Margarita Glass", "It is a variant of the classic coupe glass which is typically used to serve Margaritas."),
            new Glass("Goblet Glass", "It is a fancy drinking glass with a foot and a stem."),
            new Glass("Pokal Glass", "It has a wide shape which allows you to also use it as a bowl for serving delicious desserts."),
            new Glass("Milkshake Glass", "It features a sturdy base and a classic ribbed design that's perfect for milkshakes, soft drinks and smoothies."),
            new Glass("Hurricane Glass", "It is a tall, curved glass that's shaped like a hurricane lamp or vase."),
            new Glass("Poco Grande Glass", "It is basically a big-bowled highball that looks exactly like the tropical drink emoji."),
            new Glass("Brandy Sniffer", "It resembles an extraordinarily squat wine glass, with a voluminous bowl and low center of gravity."),
            new Glass("Wobble Cognac Glass", "It is more like a work of art, but are designed to serve their primary purpose to the best of your satisfaction."),
            new Glass("Tulip Whisky Glass", "It has a narrow rim with a long stem, similar to a wine glass."),
            new Glass("Tulip Glass", "It is designed to capture the head and promote the aroma and flavor of Belgian ales and other malty, hoppy beers."),
            new Glass("Grappa Glass", "It is slightly bulbous at the bottom, narrowing in the middle and opening out again at the rim."),
            new Glass("Pousse Café Glass", "It is very narrow in order to make it easy to craft multi layered drinks using several liqueurs of different colors in separate layers."),
            new Glass("Cordial Glass", "It is a small, stemmed glass designed to hold just a bit of your favorite cordial or liqueur."),
            new Glass("Absinthe Glass", "It is larger than a normal water glass and has a reservoir integrated in the stem, to measure the correct amount of Absinthe."),
            new Glass("Shooter Glass", "It is a small glass that holds a single measure of spirits."),
            new Glass("Shot Glass", "It is a small glass without a stem which you use for drinking shots.")
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