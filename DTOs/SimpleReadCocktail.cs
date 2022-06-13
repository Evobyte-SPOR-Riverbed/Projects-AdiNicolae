namespace RestAPIApp.DTOs.Cocktail;

public class SimpleReadCocktail
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
}
