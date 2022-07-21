using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Data.Models;

[Table(nameof(Cocktail))]
public class Cocktail
{
    public Cocktail()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    [Key]
    public Guid Id { get; set; }

    [MaxLength(256)]
    [Required(ErrorMessage = "Cocktail name is required.")]
    public string Name { get; set; }

    [MaxLength(1024)]
    [Required(ErrorMessage = "Cocktail description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Cocktail image is required.")]
    public Uri Image { get; set; }

    [Required(ErrorMessage = "Cocktail ingredients are required.")]
    public ICollection<Ingredient> Ingredients { get; set; }

    [MaxLength(1024)]
    [Required(ErrorMessage = "Cocktail instructions are required.")]
    public string Instructions { get; set; }

    public Uri InstructionsVideo { get; set; }

    public DateTime CreatedAt { get; }

    public Guid GlassId { get; set; }

    [ForeignKey(nameof(GlassId))]
    [Required(ErrorMessage = "Cocktail glass type is required.")]
    public Glass GlassType { get; set; }

    [Required(ErrorMessage = "Cocktail alcoholicity is required.")]
    public bool Alcoholic { get; set; }

    public Guid MethodId { get; set; }

    [ForeignKey(nameof(MethodId))]
    [Required(ErrorMessage = "Cocktail preparation method is required.")]
    public PreparationMethod Method { get; set; }

    [InverseProperty("Cocktail")]
    public ICollection<Review> Reviews { get; set; }
}