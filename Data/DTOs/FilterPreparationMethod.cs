using Drinktionary.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Data.DTOs;

public class FilterPreparationMethod
{
    public FilterPreparationMethod(Guid id, string name, string description, int cocktailNumber)
    {
        Id = id;
        Name = name;
        Description = description;
        CocktailNumber = cocktailNumber;
    }

    public FilterPreparationMethod(PreparationMethod preparationMethod)
    {
        Id = preparationMethod.Id;
        Name = preparationMethod.Name;
        Description = preparationMethod.Description;
        CocktailNumber = preparationMethod.Cocktails != null
            ? preparationMethod.Cocktails.Count
            : 0;
    }

    [Required(ErrorMessage = "Preparation method id is required.")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Preparation method name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Preparation method description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Cocktail number is required.")]
    public int CocktailNumber { get; set; }
}