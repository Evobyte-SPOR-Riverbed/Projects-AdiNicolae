using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Drinktionary.Data;
using Drinktionary.Data.Models;
using Drinktionary.Data.Models.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Drinktionary.Data.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Drinktionary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _context;

    public UsersController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpPost("Authenticate")]
    public async Task<ActionResult<LoginResponse>> Authenticate([FromBody] UserLogin userLogin)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'DatabaseContext.Users' is null.");
        }

        if (userLogin == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid authentication request.");
        }

        bool emailExists = await _context.Users.AnyAsync(u => u.EmailAddress == userLogin.EmailAddress);
        if (!emailExists)
        {
            return BadRequest("User matching this credentials was not found.");
        }

        bool validAuthentication = await _context.Users.AnyAsync(u => u.EmailAddress == userLogin.EmailAddress && u.Password == userLogin.Password);
        if (!validAuthentication)
        {
            return BadRequest("User's 'Password' does not match.");
        }

        User user = await _context.Users.FirstAsync(u => u.EmailAddress == userLogin.EmailAddress && u.Password == userLogin.Password);
        DateTime expirationDate = userLogin.RememberBrowser ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(1);
        SymmetricSecurityKey secretKey = new(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
        SigningCredentials signinCredentials = new(secretKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken tokenOptions = new(
            issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"],
            audience: ConfigurationManager.AppSetting["JWT:ValidAudience"],
            claims: new List<Claim>
            {
                new Claim(nameof(user.Id), user.Id.ToString()),
                new Claim(nameof(user.FirstName), user.FirstName),
                new Claim(nameof(user.LastName), user.LastName),
                new Claim(nameof(user.EmailAddress), user.EmailAddress),
                new Claim(nameof(user.CountryAlpha2), user.CountryAlpha2),
                new Claim(nameof(user.Birthday), user.Birthday.ToShortDateString()),
                new Claim(nameof(user.Sex), user.Sex.ToString()),
                new Claim(nameof(user.DrinkerType), user.DrinkerType.ToString())
            },
            expires: expirationDate,
            signingCredentials: signinCredentials);
        string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return new LoginResponse(tokenString);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'DatabaseContext.Users' is null.");
        }

        if (userRegister == null)
        {
            return BadRequest("Invalid registration request.");
        }

        bool emailExists = await _context.Users.AnyAsync(u => u.EmailAddress == userRegister.EmailAddress);
        if (emailExists)
        {
            return BadRequest("User with same 'EmailAddress' already exists.");
        }

        User user = new(userRegister);
        bool idExists = await _context.Users.AnyAsync(u => u.Id == user.Id);
        if (idExists) // Is this even possible? Better safe I guess, lol
        {
            return BadRequest("User with same 'Id' already exists.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully.");
    }

    // PUT: api/Users/5 To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, [FromBody] SettingsUser settingsUser)
    {
        if (HttpContext.User.Identity is not ClaimsIdentity identity
        || !Guid.TryParse(identity.FindFirst("Id").Value, out Guid identityId)
        || identityId != id)
        {
            return BadRequest("Cannot modify another user's information.");
        }

        User? user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return BadRequest("User with specified 'Id' does not exist.");
        }

        if (user.Password != settingsUser.ActualPassword)
        {
            return BadRequest("User's 'Password' does not match.");
        }

        bool emailExists = await _context.Users.AnyAsync(u => u.Id != id && u.EmailAddress == settingsUser.EmailAddress);
        if (emailExists)
        {
            return BadRequest("User with same 'Email' already exists.");
        }

        user.FirstName = settingsUser.FirstName;
        user.LastName = settingsUser.LastName;
        user.EmailAddress = settingsUser.EmailAddress;
        user.Password = settingsUser.Password?.Length == 0
            ? user.Password
            : settingsUser.Password;
        user.CountryAlpha2 = settingsUser.CountryAlpha2;
        user.Birthday = settingsUser.Birthday;
        user.Sex = settingsUser.Sex;
        user.DrinkerType = settingsUser.DrinkerType;

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!UserExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/Users/5
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'DatabaseContext.Users' is null.");
        }

        if (HttpContext.User.Identity is not ClaimsIdentity identity
        || !Guid.TryParse(identity.FindFirst("Id").Value, out Guid identityId)
        || identityId != id)
        {
            return BadRequest("Cannot delete another user's account.");
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return BadRequest("User with specified 'Id' not found.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(Guid id)
    {
        return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}