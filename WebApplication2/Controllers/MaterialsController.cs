namespace WebApplication2.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor for injecting the DbContext
        public MaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<materials>>> GetMaterials()
        {
            // Fetch all materials from the database
            return await _context.Materials.ToListAsync();
        }

        // GET: api/materials/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<materials>> GetMaterial(int id)
        {
            // Find the material by its ID
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return material;
        }

        // POST: api/materials
        [HttpPost]
        public async Task<ActionResult<materials>> PostMaterial(materials material)
        {
            // Add the new material to the database
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            // Return the created material with its new location
            return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, material);
        }

        // PUT: api/materials/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(int id, materials material)
        {
            if (id != material.Id)
            {
                return BadRequest(); // Return 400 if IDs do not match
            }

            // Mark the entity as modified
            _context.Entry(material).State = EntityState.Modified;

            try
            {
                // Attempt to save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
                {
                    return NotFound(); // Return 404 if the material no longer exists
                }
                else
                {
                    throw; // Rethrow the exception if it's an unexpected issue
                }
            }

            return NoContent(); // Return 204 No Content on success
        }

        // DELETE: api/materials/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            // Find the material by its ID
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound(); // Return 404 if not found
            }

            // Remove the material from the database
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content on success
        }

        // Helper method to check if a material exists
        private bool MaterialExists(int id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }
    }
}
