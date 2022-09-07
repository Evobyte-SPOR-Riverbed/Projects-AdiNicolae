using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Drinktionary.Data;
using Drinktionary.Data.Models;
using Drinktionary.Data.Models.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Drinktionary.Controllers
{
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
                return NotFound("User matching this credentials was not found.");
            }

            bool validAuthentication = await _context.Users.AnyAsync(u => u.EmailAddress == userLogin.EmailAddress && u.Password == userLogin.Password);
            if (!validAuthentication)
            {
                return Problem("User's 'Password' does not match.");
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
                    new Claim("Age", (DateTime.Now.Year - user.Birthday.Year).ToString()),
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

            if (userRegister == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid registration request.");
            }

            bool emailExists = await _context.Users.AnyAsync(u => u.EmailAddress == userRegister.EmailAddress);
            if (emailExists)
            {
                return Problem("User with same 'EmailAddress' already exists.");
            }

            User user = new(userRegister);
            bool idExists = await _context.Users.AnyAsync(u => u.Id == user.Id);
            if (idExists) // Is this even possible?
            {
                return Problem("User with same 'Id' already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5 To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            User? emailUser = await _context.Users.FindAsync((User u) => u.Id != id && u.EmailAddress == user.EmailAddress);
            if (emailUser == null)
            {
                return Problem("User with same 'Email' already exists.");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DatabaseContext.Users' is null.");
            }

            User? validIdUser = await _context.Users.FindAsync(user.Id);
            if (validIdUser == null)
            {
                return Problem("User with same 'Id' already exists.");
            }

            User? validEmailUser = await _context.Users.FindAsync(user.EmailAddress);
            if (validEmailUser == null)
            {
                return Problem("User with same 'Email' already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
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
}