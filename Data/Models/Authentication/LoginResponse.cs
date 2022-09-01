using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Data.Models.Authentication;

public class LoginResponse
{
    public LoginResponse(string token)
    {
        Token = token;
    }

    [Required(ErrorMessage = "Token is required.")]
    public string Token { get; set; }
}