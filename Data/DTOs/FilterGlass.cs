using Drinktionary.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Drinktionary.Data.DTOs;

public class FilterGlass
{
    public FilterGlass(Guid id, string name, string description, int cocktailNumber)
    {
        Id = id;
        Name = name;
        Description = description;
        CocktailNumber = cocktailNumber;
    }

    public FilterGlass(Glass glass)
    {
        Id = glass.Id;
        Name = glass.Name;
        Description = glass.Description;
        CocktailNumber = glass.Cocktails != null
            ? glass.Cocktails.Count
            : 0;
    }

    [Required(ErrorMessage = "Glass id is required.")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Glass name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Glass description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Cocktail number is required.")]
    public int CocktailNumber { get; set; }
}