using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Data.Models.Authentication;

public class LoginResponse
{
    public LoginResponse(string token)
    {
        AccessToken = token;
    }

    [Required(ErrorMessage = "Access token is required.")]
    public string AccessToken { get; set; }
}