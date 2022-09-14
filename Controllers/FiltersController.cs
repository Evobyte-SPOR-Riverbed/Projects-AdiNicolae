using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Drinktionary.Data;
using Drinktionary.Data.Models;
using Drinktionary.Data.Models.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Drinktionary.Data.Pagination;
using Drinktionary.Data.DTOs;
using System.Linq;
using Drinktionary.Misc;
using Drinktionary.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace Drinktionary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IUriService _uriService;

        public FiltersController(DatabaseContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        // GET: api/Filters/Glasses
        [HttpGet("Glasses")]
        public async Task<ActionResult<PagedResponse<List<FilterGlass>>>> GetGlasses([FromQuery] PaginationFilter paginationFilter, [FromQuery] string? searchFilter = null)
        {
            if (_context.Glasses == null)
            {
                return NotFound();
            }

            PaginationFilter validFilter = new(paginationFilter);
            IQueryable<Glass> filteredGlassData = searchFilter != null
                ? _context.Glasses
                    .Where(g => g.Name.Contains(searchFilter) || searchFilter.Contains(g.Name))
                : _context.Glasses;
            List<FilterGlass> pagedGlassData = await filteredGlassData
                .OrderBy(g => g.Name)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Select(g => new FilterGlass(g))
                .ToListAsync();

            int totalRecords = await filteredGlassData.CountAsync();
            string route = searchFilter != null
                ? QueryHelpers.AddQueryString(Request.Path.Value, nameof(searchFilter), searchFilter)
                : Request.Path.Value;
            return Helpers.CreatePagedReponse(pagedGlassData, validFilter, totalRecords, _uriService, route);
        }

        // GET: api/Filters/Ingredients
        [HttpGet("Ingredients")]
        public async Task<ActionResult<PagedResponse<List<FilterIngredient>>>> GetIngredients([FromQuery] PaginationFilter paginationFilter, [FromQuery] string? searchFilter = null)
        {
            if (_context.Ingredients == null)
            {
                return NotFound();
            }

            PaginationFilter validFilter = new(paginationFilter);
            IQueryable<Ingredient> filteredIngredientData = searchFilter != null
                ? _context.Ingredients
                    .Where(i => i.Name.Contains(searchFilter) || searchFilter.Contains(i.Name))
                : _context.Ingredients;
            List<FilterIngredient> pagedIngredientData = await filteredIngredientData
                .OrderBy(i => i.Name)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Select(i => new FilterIngredient(i))
                .ToListAsync();

            int totalRecords = await filteredIngredientData.CountAsync();
            string route = searchFilter != null
                ? QueryHelpers.AddQueryString(Request.Path.Value, nameof(searchFilter), searchFilter)
                : Request.Path.Value;
            return Helpers.CreatePagedReponse(pagedIngredientData, validFilter, totalRecords, _uriService, route);
        }

        // GET: api/Filters/PreparationMethods
        [HttpGet("PreparationMethods")]
        public async Task<ActionResult<PagedResponse<List<FilterPreparationMethod>>>> GetPreparationMethods([FromQuery] PaginationFilter paginationFilter, [FromQuery] string? searchFilter = null)
        {
            if (_context.PreparationMethods == null)
            {
                return NotFound();
            }

            PaginationFilter validFilter = new(paginationFilter);
            IQueryable<PreparationMethod> filteredPreparationMethodData = searchFilter != null
                ? _context.PreparationMethods
                    .Where(pm => pm.Name.Contains(searchFilter) || searchFilter.Contains(pm.Name))
                : _context.PreparationMethods;
            List<FilterPreparationMethod> pagedPreparationMethodData = await filteredPreparationMethodData
                .OrderBy(pm => pm.Name)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Select(pm => new FilterPreparationMethod(pm))
                .ToListAsync();

            int totalRecords = await filteredPreparationMethodData.CountAsync();
            string route = searchFilter != null
                ? QueryHelpers.AddQueryString(Request.Path.Value, nameof(searchFilter), searchFilter)
                : Request.Path.Value;
            return Helpers.CreatePagedReponse(pagedPreparationMethodData, validFilter, totalRecords, _uriService, route);
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

        private static bool TwoWayContains(string firstString, string secondString)
        {
            return firstString.Contains(secondString) || secondString.Contains(firstString);
        }

        private bool UserExists(Guid id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}