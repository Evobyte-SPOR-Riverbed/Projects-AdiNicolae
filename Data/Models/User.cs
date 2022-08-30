using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drinktionary.Data.Models;

public enum DrinkerType
{
    SocialDrinker,
    ConformityDrinker,
    EnhancementDrinker,
    CopingDrinker,
    None
}

public enum SexType
{
    Male,
    Female,
    Intersex,
    Other
}

[Table(nameof(User))]
public class User
{
    public User()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public User(string firstName, string lastName, string email, string password, DateTime birthday, SexType sexType, DrinkerType drinkerType, string countryAlpha2)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Birthday = birthday;
        Sex = sexType;
        DrinkerType = drinkerType;
        CountryAlpha2 = countryAlpha2;
        CreatedAt = DateTime.Now;
    }

    public User(string firstName, string lastName, string email, string password, DateTime birthday, SexType sexType, DrinkerType drinkerType, string countryAlpha2, ICollection<Review> reviews, ICollection<Cocktail> favoriteCocktails)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Birthday = birthday;
        Sex = sexType;
        DrinkerType = drinkerType;
        CountryAlpha2 = countryAlpha2;
        Reviews = reviews;
        FavoriteCocktails = favoriteCocktails;
        CreatedAt = DateTime.Now;
    }

    [Key]
    public Guid Id { get; set; }

    [MaxLength(64)]
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; }

    [MaxLength(128)]
    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email address is required.")]
    public string Email { get; set; }

    [MinLength(14, ErrorMessage = "Password must be at least {0} characters long.")]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [MaxLength(204800, ErrorMessage = "Avatar is too big.")]
    public byte[] AvatarBytes { get; set; }

    // Taking only default age limit in consideration, considering to implement limit per country.
    // [Range(18, double.PositiveInfinity, ErrorMessage = "Age must be between {0} and {1}.")]
    // [Required(ErrorMessage = "Age is required.")] public int Age { get; set; }

    [Required(ErrorMessage = "Birthday is required.")]
    public DateTime Birthday { get; set; }

    [Required(ErrorMessage = "Sex is required.")]
    public SexType Sex { get; set; }

    [Required(ErrorMessage = "Drinker type is required.")]
    public DrinkerType DrinkerType { get; set; }

    [StringLength(2, ErrorMessage = "Invalid country code.")]
    [Required(ErrorMessage = "Country code is required.")]
    public string CountryAlpha2 { get; set; }

    public DateTime CreatedAt { get; }

    [InverseProperty(nameof(Review.Author))]
    public ICollection<Review> Reviews { get; set; }

    [InverseProperty(nameof(Cocktail.FavoritedUsers))]
    public ICollection<Cocktail> FavoriteCocktails { get; set; }
}