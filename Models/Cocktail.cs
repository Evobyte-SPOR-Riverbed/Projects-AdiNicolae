using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestAPIApp.DTOs.Cocktail;

namespace RestAPIApp.Models;

[Table("Cocktails")]
public class Cocktail
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cocktail name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Cocktail instructions are required.")]
    public string Instructions { get; set; }

    [Required(ErrorMessage = "Cocktail ingredients are required.")]
    public string Ingredients { get; set; }

    public DateTime CreatedAt { get; set; }

    public Cocktail()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Cocktail(Guid id, string name, string instructions, string ingredients)
    {
        Id = id;
        Name = name;
        Instructions = instructions;
        Ingredients = ingredients;
        CreatedAt = DateTime.Now;
    }

    public SimpleReadCocktail ToSimpleRead() =>
        new SimpleReadCocktail
        {
            Id = Id,
            Name = Name,
            CreatedAt = CreatedAt
        };

    public ComplexReadCocktail ToComplexRead() =>
        new ComplexReadCocktail
        {
            Id = Id,
            Name = Name,
            Instructions = Instructions,
            Ingredients = Ingredients.Split(',').ToList()
        };
}
