using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Models;

[Table("Ingredient")]
public class Ingredient
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Ingredient name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Ingredient description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Ingredient image is required.")]
    public Uri Image { get; set; }

    public DateTime CreatedAt { get; private set; }

    public ICollection<Cocktail> Cocktails { get; set; }
}