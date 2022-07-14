using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Models;

[Table("Review")]
public class Review
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Review content is required.")]
    public string Content { get; set; }

    [Required(ErrorMessage = "Review rating is required.")]
    [Range(0f, 5f, ErrorMessage = "Rating must be between 0 and 5.")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Review author is required.")]
    public string Author { get; set; }

    public DateTime CreatedAt { get; private set; }

    public Cocktail Cocktail { get; set; }
}