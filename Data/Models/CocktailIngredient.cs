using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Data.Models;

[Table(nameof(CocktailIngredient))]
public class CocktailIngredient
{
    public CocktailIngredient()
    {
    }

    public CocktailIngredient(Guid cocktailId, Cocktail cocktail, Guid ingredientId, Ingredient ingredient, int quantity, string unit)
    {
        CocktailId = cocktailId;
        Cocktail = cocktail;
        IngredientId = ingredientId;
        Ingredient = ingredient;
        Quantity = quantity;
        Unit = unit;
    }

    public Guid CocktailId { get; set; }

    public Cocktail Cocktail { get; set; }

    public Guid IngredientId { get; set; }

    public Ingredient Ingredient { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Unit is required.")]
    public string Unit { get; set; }
}