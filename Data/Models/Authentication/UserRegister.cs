using System.ComponentModel.DataAnnotations;
using static Drinktionary.Misc.Enums;

namespace Drinktionary.Data.Models.Authentication;

public class UserRegister
{
    public UserRegister(string firstName, string lastName, string emailAddress, string password, string countryAlpha2, DateTime birthday, SexType sex, DrinkerType drinkerType)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        Password = password;
        CountryAlpha2 = countryAlpha2;
        Birthday = birthday;
        Sex = sex;
        DrinkerType = drinkerType;
    }

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

    [StringLength(2, ErrorMessage = "Invalid country code.")]
    [Required(ErrorMessage = "Country code is required.")]
    public string CountryAlpha2 { get; set; }

    [Required(ErrorMessage = "Birthday is required.")]
    public DateTime Birthday { get; set; }

    [Required(ErrorMessage = "Sex is required.")]
    public SexType Sex { get; set; }

    [Required(ErrorMessage = "Drinker type is required.")]
    public DrinkerType DrinkerType { get; set; }
}