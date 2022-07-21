using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Data.Models;

[Table(nameof(Review))]
public class Review
{
    public Review()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public Review(string content, int rating, string author)
    {
        Id = Guid.NewGuid();
        Content = content;
        Rating = rating;
        Author = author;
        CreatedAt = DateTime.Now;
    }

    public Review(string content, int rating, string author, Cocktail cocktail)
    {
        Id = Guid.NewGuid();
        Content = content;
        Rating = rating;
        Author = author;
        CreatedAt = DateTime.Now;
        Cocktail = cocktail;
    }

    [Key]
    public Guid Id { get; set; }

    [MaxLength(1024)]
    [Required(ErrorMessage = "Review content is required.")]
    public string Content { get; set; }

    [Required(ErrorMessage = "Review rating is required.")]
    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
    public int Rating { get; set; }

    [MaxLength(768)]
    [Required(ErrorMessage = "Review author is required.")]
    public string Author { get; set; }

    public DateTime CreatedAt { get; }

    public Guid CocktailId { get; set; }

    [ForeignKey(nameof(CocktailId))]
    public Cocktail Cocktail { get; set; }
}