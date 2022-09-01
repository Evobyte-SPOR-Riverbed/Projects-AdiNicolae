using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Data.Models.Authentication;

public class UserLogin
{
    public UserLogin(string emailAddress, string password, bool rememberBrowser)
    {
        EmailAddress = emailAddress;
        Password = password;
        RememberBrowser = rememberBrowser;
    }

    [Required(ErrorMessage = "Email address is required.")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Browser remembering is required.")]
    public bool RememberBrowser { get; set; }
}