using Microsoft.AspNetCore.Mvc;
using Drinktionary.Data;
using Drinktionary.Data.Models;
using Drinktionary.Data.Models.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Drinktionary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly DatabaseContext _context;

    public AuthenticationController(DatabaseContext context)
    {
        _context = context;
    }

    // POST: api/Authentication
    [HttpPost]
    public async Task<ActionResult<LoginResponse>> Login(UserLogin userLogin)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'DatabaseContext.Users' is null.");
        }

        if (userLogin == null)
        {
            return BadRequest("Invalid login request.");
        }

        User? validEmailUser = await _context.Users.FindAsync(userLogin.EmailAddress);
        if (validEmailUser == null)
        {
            return NotFound();
        }

        User? validLoginUser = await _context.Users.FindAsync((User u) => u.EmailAddress == userLogin.EmailAddress && u.Password == userLogin.Password);
        if (validLoginUser == null)
        {
            return Problem("User's 'Password' does not match.");
        }

        DateTime expirationDate = userLogin.RememberBrowser ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(1);
        SymmetricSecurityKey secretKey = new(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
        SigningCredentials signinCredentials = new(secretKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken tokenOptions = new(
            issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
            audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
            claims: new List<Claim>
            {
                new Claim(nameof(validLoginUser.FirstName), validLoginUser.FirstName),
                new Claim(nameof(validLoginUser.LastName), validLoginUser.LastName),
                new Claim(nameof(validLoginUser.EmailAddress), validLoginUser.EmailAddress),
                new Claim(nameof(validLoginUser.CountryAlpha2), validLoginUser.CountryAlpha2),
                new Claim("Age", (validLoginUser.Birthday.Year - DateTime.Now.Year).ToString()),
                new Claim(nameof(validLoginUser.Sex), validLoginUser.Sex.ToString()),
                new Claim(nameof(validLoginUser.DrinkerType), validLoginUser.DrinkerType.ToString())
            },
            expires: expirationDate,
            signingCredentials: signinCredentials);
        string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return new LoginResponse(tokenString);
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegister userRegister)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'DatabaseContext.Users' is null.");
        }

        User? validEmailUser = await _context.Users.FindAsync(userRegister.EmailAddress);
        if (validEmailUser == null)
        {
            return Problem("User with same 'EmailAddress' already exists.");
        }

        User user = new(userRegister);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully.");
    }
}