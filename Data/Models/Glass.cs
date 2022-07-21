using System;
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

    public Glass(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.Now;
    }

    public Glass(string name, ICollection<Cocktail> cocktails)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.Now;
        Cocktails = cocktails;
    }

    [Key]
    public Guid Id { get; set; }

    [MaxLength(256)]
    [Required(ErrorMessage = "Glass name is required.")]
    public string Name { get; set; }

    [InverseProperty(nameof(Cocktail.GlassType))]
    public ICollection<Cocktail> Cocktails { get; set; }

    public DateTime CreatedAt { get; }
}