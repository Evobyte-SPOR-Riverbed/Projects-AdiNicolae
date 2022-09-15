using static Drinktionary.Misc.Enums;
using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Data.DTOs
{
    public class SettingsUser
    {
        public SettingsUser(string firstName, string lastName, string emailAddress, string password, string countryAlpha2, DateTime birthday, SexType sex, DrinkerType drinkerType, string actualPassword)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            Password = password;
            CountryAlpha2 = countryAlpha2;
            Birthday = birthday;
            Sex = sex;
            DrinkerType = drinkerType;
            ActualPassword = actualPassword;
        }

        [MaxLength(64)]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [MaxLength(128)]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        [StringLength(2, ErrorMessage = "Invalid country code.")]
        [Required(ErrorMessage = "Country code is required.")]
        public string CountryAlpha2 { get; set; }

        // Taking only default age limit in consideration, considering to implement limit per
        // country. [Range(18, double.PositiveInfinity, ErrorMessage = "Age must be between {0} and
        // {1}.")] [Required(ErrorMessage = "Age is required.")] public int Age { get; set; }
        [Required(ErrorMessage = "Birthday is required.")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Sex is required.")]
        public SexType Sex { get; set; }

        [Required(ErrorMessage = "Drinker type is required.")]
        public DrinkerType DrinkerType { get; set; }

        [Required(ErrorMessage = "Actual password is required.")]
        public string ActualPassword { get; set; }
    }
}