using Drinktionary.Data.Models.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Drinktionary.Misc.Enums;

namespace Drinktionary.Data.Models;

[Table(nameof(User))]
public class User
{
    public User()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public User(UserRegister userRegister)
    {
        Id = Guid.NewGuid();
        FirstName = userRegister.FirstName;
        LastName = userRegister.LastName;
        EmailAddress = userRegister.EmailAddress;
        Password = userRegister.Password;
        CountryAlpha2 = userRegister.CountryAlpha2;
        Birthday = userRegister.Birthday;
        Sex = userRegister.Sex;
        DrinkerType = userRegister.DrinkerType;
        CreatedAt = DateTime.Now;
    }

    public User(string firstName, string lastName, string emailAddress, string password, string countryAlpha2, DateTime birthday, SexType sexType, DrinkerType drinkerType)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        Password = password;
        CountryAlpha2 = countryAlpha2;
        Birthday = birthday;
        Sex = sexType;
        DrinkerType = drinkerType;
        CreatedAt = DateTime.Now;
    }

    public User(string firstName, string lastName, string emailAddress, string password, string countryAlpha2, DateTime birthday, SexType sexType, DrinkerType drinkerType, ICollection<Review> reviews, ICollection<Cocktail> favoriteCocktails)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        Password = password;
        CountryAlpha2 = countryAlpha2;
        Birthday = birthday;
        Sex = sexType;
        DrinkerType = drinkerType;
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
    public string EmailAddress { get; set; }

    [MinLength(14, ErrorMessage = "Password must be at least {0} characters long.")]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [MaxLength(204800, ErrorMessage = "Avatar is too big.")]
    public byte[] AvatarBytes { get; set; }

    [StringLength(2, ErrorMessage = "Invalid country code.")]
    [Required(ErrorMessage = "Country code is required.")]
    public string CountryAlpha2 { get; set; }

    // Taking only default age limit in consideration, considering to implement limit per country.
    // [Range(18, double.PositiveInfinity, ErrorMessage = "Age must be between {0} and {1}.")]
    // [Required(ErrorMessage = "Age is required.")] public int Age { get; set; }
    [Required(ErrorMessage = "Birthday is required.")]
    public DateTime Birthday { get; set; }

    [Required(ErrorMessage = "Sex is required.")]
    public SexType Sex { get; set; }

    [Required(ErrorMessage = "Drinker type is required.")]
    public DrinkerType DrinkerType { get; set; }

    public DateTime CreatedAt { get; }

    [InverseProperty(nameof(Review.Author))]
    public ICollection<Review> Reviews { get; set; }

    [InverseProperty(nameof(Cocktail.FavoritedUsers))]
    public ICollection<Cocktail> FavoriteCocktails { get; set; }
}