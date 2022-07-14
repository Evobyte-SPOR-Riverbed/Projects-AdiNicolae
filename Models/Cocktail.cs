using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Models;

[Table("Cocktail")]
public class Cocktail
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cocktail name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Cocktail description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Cocktail image is required.")]
    public Uri Image { get; set; }

    [Required(ErrorMessage = "Cocktail ingredients are required.")]
    public ICollection<Ingredient> Ingredients { get; set; }

    [Required(ErrorMessage = "Cocktail instructions are required.")]
    public string Instructions { get; set; }

    public Uri InstructionsVideo { get; set; }

    [Required(ErrorMessage = "Cocktail glass type is required.")]
    public Enums.GlassType GlassType { get; set; }

    [Required(ErrorMessage = "Cocktail alcoholicity is required.")]
    public bool Alcoholic { get; set; }

    public string Garnish { get; set; }
    public string Method { get; set; }
    public float Rating => (float)Reviews.Average((r) => r.Rating);
    public int RatingCount => Reviews.Count;
    public ICollection<Review> Reviews { get; set; }
}