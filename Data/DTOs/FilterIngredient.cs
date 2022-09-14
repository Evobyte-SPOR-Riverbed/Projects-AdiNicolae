using Drinktionary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Data.DTOs;

public class FilterIngredient
{
    public FilterIngredient(Guid id, string name, string description, int cocktailNumber)
    {
        Id = id;
        Name = name;
        Description = description;
        CocktailNumber = cocktailNumber;
    }

    public FilterIngredient(Ingredient ingredient)
    {
        Id = ingredient.Id;
        Name = ingredient.Name;
        Description = ingredient.Description;
        CocktailNumber = ingredient.Cocktails != null
            ? ingredient.Cocktails.Count
            : 0;
    }

    [Required(ErrorMessage = "Ingredient id is required.")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Ingredient name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Ingredient description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Cocktail number is required.")]
    public int CocktailNumber { get; set; }
}