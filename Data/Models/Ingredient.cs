using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Data.Models;

[Table(nameof(Ingredient))]
public class Ingredient
{
    public Ingredient()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Ingredient(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = DateTime.Now;
    }

    public Ingredient(string name, string description, ICollection<Cocktail> cocktails)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = DateTime.Now;
        Cocktails = cocktails;
    }

    [Key]
    public Guid Id { get; set; }

    [MaxLength(256)]
    [Required(ErrorMessage = "Ingredient name is required.")]
    public string Name { get; set; }

    [MaxLength(1024)]
    [Required(ErrorMessage = "Ingredient description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Creation date is required.")]
    public DateTime CreatedAt { get; set; }

    public ICollection<Cocktail> Cocktails { get; set; }

    public List<CocktailIngredient> CocktailIngredients { get; set; }
}