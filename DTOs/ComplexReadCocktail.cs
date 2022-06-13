namespace RestAPIApp.DTOs.Cocktail;

public class ComplexReadCocktail : SimpleReadCocktail
{
    public string Instructions { get; set; }
    public List<string> Ingredients { get; set; }
}
