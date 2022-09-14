using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Data.Models;

[Table(nameof(Glass))]
public class Glass
{
    public Glass()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Glass(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = DateTime.Now;
    }

    public Glass(string name, string description, ICollection<Cocktail> cocktails)
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
    [Required(ErrorMessage = "Glass name is required.")]
    public string Name { get; set; }

    [MaxLength(1024)]
    [Required(ErrorMessage = "Glass description is required.")]
    public string Description { get; set; }

    [InverseProperty(nameof(Cocktail.GlassType))]
    public ICollection<Cocktail> Cocktails { get; set; }

    [Required(ErrorMessage = "Creation date is required.")]
    public DateTime CreatedAt { get; set; }
}