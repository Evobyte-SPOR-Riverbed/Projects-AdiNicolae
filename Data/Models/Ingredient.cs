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

    public Ingredient(string name, string description, Uri imageUri)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ImageUri = imageUri;
        CreatedAt = DateTime.Now;
    }

    public Ingredient(string name, string description, Uri imageUri, ICollection<Cocktail> cocktails)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        ImageUri = imageUri;
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

    [Required(ErrorMessage = "Ingredient image uri is required.")]
    public Uri ImageUri { get; set; }

    public DateTime CreatedAt { get; }

    public ICollection<Cocktail> Cocktails { get; set; }
}