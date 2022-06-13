using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPIApp.Data;
using RestAPIApp.DTOs.Cocktail;
using RestAPIApp.Models;

namespace RestAPIApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CocktailsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CocktailsController(DatabaseContext context)
        {
            _context = context;

            _context.Cocktails.AddRange(
                new Cocktail(
                    Guid.NewGuid(), // Guid.ParseExact("a8220427-6edb-4fd5-ad90-1bc84cdf0d5e", "D"),
                    "Kool First Aid",
                    "Add Kool Aid to a double shot glass, and top with rum. Slam and shoot.",
                    "1/2 oz of Rum,1/2 tsp of Kool Aid"
                ),
                new Cocktail(
                    Guid.NewGuid(), // Guid.ParseExact("854e0bc6-d3c2-418c-8967-31e33fa2f6e2", "D"),
                    "Quaker's Cocktail",
                    "Shake all ingredients with ice, strain into a cocktail glass, and serve.",
                    "3/4 oz of Light Rum,3/4 oz of Brandy,1/4 Lemon Juice,2 tsp of Raspberry Syrup"
                ),
                new Cocktail(
                    Guid.NewGuid(), // Guid.ParseExact("e627ad46-5e12-42e8-96d3-1ddf63b2a8b5", "D"),
                    "Alabama Slammer",
                    "Pour all ingredients (except for lemon juice) over ice in a highball glass. Stir, add a dash of lemon juice, and serve.",
                    "1 oz of Southern Comfort,1 oz of Amaretto,1/2 oz of Sloe Gin,1 dash of Lemon Juice"
                ),
                new Cocktail(
                    Guid.NewGuid(), // Guid.ParseExact("5bf7ad70-0f08-4314-9b41-f1453a39afba", "D"),
                    "Microwave Hot Cocoa",
                    "Combine sugar, cocoa, salt and hot water in 1-quart micro-proof measuring cup (or coffee mug). Microwave at HIGH (100%) for 1 to 1 1/2 minutes or until boiling. Add milk, stir and microwave an additonal 1 1/2 to 2 minutes or until hot. Stir in vanilla, blend well.",
                    "5 tblsp of Sugar,3 tblsp of Cocoa,1/2 dash of Salt,3 tblsp of Hot Water,2 cups of Milk,1/4 tsp of Vanilla"
                ),
                new Cocktail(
                    Guid.NewGuid(), // Guid.ParseExact("36a4c245-c9cc-4354-b695-6fdf18b1a05f", "D"),
                    "Shot-gun",
                    "Pour one part Jack Daniels and one part Jim Beam into a shot glass then float Wild Turkey on top.",
                    "1 part Jack Daniels,1 part of Jim Beam,1 oz of Wild Turkey"
                ),
                new Cocktail(
                    Guid.NewGuid(), // Guid.ParseExact("757ab812-0487-4ae2-a86e-2a97c8f404b5", "D"),
                    "New York Sour",
                    "Shake blended whiskey, juice of lemon, and powdered sugar with ice and strain into a whiskey sour glass. Float claret on top. Decorate with the half-slice of lemon and the cherry and serve.",
                    "2 oz of Blended Whiskey,1/2 Lemon Juice,1 tsp of Powdered Sugar,1 Cherry,1 slice of Lemon,1 oz of Claret"
                )
            );

            _context.SaveChanges();
        }

        // GET: api/Cocktails
        [HttpGet]
        [HttpGet("Complex")]
        public async Task<ActionResult<IEnumerable<ComplexReadCocktail>>> GetComplexCocktails()
        {
            if (_context.Cocktails == null)
                return NotFound();

            return await _context.Cocktails.Select((c) => c.ToComplexRead()).ToListAsync();
        }

        [HttpGet("Simple")]
        public async Task<ActionResult<IEnumerable<SimpleReadCocktail>>> GetSimpleCocktails()
        {
            if (_context.Cocktails == null)
                return NotFound();

            return await _context.Cocktails.Select((c) => c.ToSimpleRead()).ToListAsync();
        }

        // GET: api/Cocktails/5
        [HttpGet("{id:guid}")]
        [HttpGet("Complex/{id:guid}")]
        public async Task<ActionResult<ComplexReadCocktail>> GetComplexCocktail(Guid id)
        {
            if (_context.Cocktails == null)
                return NotFound();

            Cocktail? cocktail = await _context.Cocktails.FindAsync(id);
            if (cocktail == null)
                return NotFound();

            return cocktail.ToComplexRead();
        }

        [HttpGet("Simple/{id:guid}")]
        public async Task<ActionResult<SimpleReadCocktail>> GetSimpleCocktail(Guid id)
        {
            if (_context.Cocktails == null)
                return NotFound();

            Cocktail? cocktail = await _context.Cocktails.FindAsync(id);
            if (cocktail == null)
                return NotFound();

            return cocktail.ToSimpleRead();
        }

        [HttpGet("Ingredients/{id:guid}")]
        public async Task<ActionResult<List<string>>> GetCocktailIngredients(Guid id)
        {
            if (_context.Cocktails == null)
                return NotFound();

            Cocktail? cocktail = await _context.Cocktails.FindAsync(id);
            if (cocktail == null)
                return NotFound();

            return cocktail.ToComplexRead().Ingredients;
        }

        // PUT: api/Cocktails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutCocktail(Guid id, CreateUpdateCocktail cocktail)
        {
            Cocktail newCocktail = cocktail.ToCocktail();
            newCocktail.Id = id;
            _context.Entry(newCocktail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CocktailExists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // PUT: api/Cocktails/Ingredients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Ingredients/{id:guid}")]
        public async Task<IActionResult> PutCocktailIngredients(Guid id, List<string> ingredients)
        {
            Cocktail? cocktail = await _context.Cocktails.FindAsync(id);
            if (cocktail == null)
                return NotFound();

            cocktail.Ingredients = String.Join(",", ingredients);
            _context.Entry(cocktail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CocktailExists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // POST: api/Cocktails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cocktail>> PostCocktail(CreateUpdateCocktail cocktail)
        {
            if (_context.Cocktails == null)
                return Problem("Entity set 'CocktailContext.Cocktails' is null.");

            Cocktail newCocktail = cocktail.ToCocktail(true);
            _context.Cocktails.Add(newCocktail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCocktail", new { id = newCocktail.Id }, newCocktail);
        }

        // DELETE: api/Cocktails/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCocktail(Guid id)
        {
            if (_context.Cocktails == null)
                return NotFound();

            Cocktail? cocktail = await _context.Cocktails.FindAsync(id);
            if (cocktail == null)
                return NotFound();

            _context.Cocktails.Remove(cocktail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CocktailExists(Guid id) =>
            (_context.Cocktails?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
