using System.ComponentModel.DataAnnotations;

namespace RestAPIApp.DTOs.Cocktail;

public class CreateUpdateCocktail
{
    [Required(ErrorMessage = "Cocktail name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Cocktail instructions are required.")]
    public string Instructions { get; set; }

    public List<string> Ingredients { get; set; }

    public Models.Cocktail ToCocktail(bool generateId = false) =>
        new Models.Cocktail
        {
            Id = generateId ? Guid.NewGuid() : default,
            Name = Name,
            Instructions = Instructions,
            Ingredients = String.Join(',', Ingredients)
        };
}
