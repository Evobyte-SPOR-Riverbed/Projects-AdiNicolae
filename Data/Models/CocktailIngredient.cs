using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Data.Models;

[Table(nameof(CocktailIngredient))]
public class CocktailIngredient
{
    public Guid CocktailId { get; set; }

    [ForeignKey(nameof(CocktailId))]
    public Cocktail Cocktail { get; set; }

    public Guid IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public Ingredient Ingredient { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Unit is required.")]
    public string Unit { get; set; }
}