using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Data.Models;

public class PreparationMethod
{
    public PreparationMethod()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public PreparationMethod(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = DateTime.Now;
    }

    public PreparationMethod(string name, string description, ICollection<Cocktail> cocktails)
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
    [Required(ErrorMessage = "Preparation method name is required.")]
    public string Name { get; set; }

    [MaxLength(1024)]
    [Required(ErrorMessage = "Preparation method description is required.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Creation date is required.")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty(nameof(Cocktail.Method))]
    public ICollection<Cocktail> Cocktails { get; set; }
}