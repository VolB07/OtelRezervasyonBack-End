using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SupportRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SupportRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupportRequest>>> GetSupportRequests()
        {
            return await _context.SupportRequests
                                 .Include(sr => sr.User) // İlişkili kullanıcı bilgisi dahil edilir
                                 .ToListAsync();
        }

        // GET: api/SupportRequests/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SupportRequest>> GetSupportRequest(int id)
        {
            var supportRequest = await _context.SupportRequests
                                               .Include(sr => sr.User)
                                               .FirstOrDefaultAsync(sr => sr.id == id); 

            if (supportRequest == null)
            {
                return NotFound();
            }

            return supportRequest;
        }

        // POST: api/SupportRequests
[HttpPost]
public async Task<ActionResult<SupportRequest>> CreateSupportRequest(SupportRequest supportRequest)
{
    if (supportRequest == null)
    {
        return BadRequest("SupportRequest cannot be null.");
    }

    // UserId'nin boş olup olmadığını kontrol et
    if (supportRequest.user_id == null || supportRequest.user_id == 0)
    {
        return BadRequest("UserId is required.");
    }

    // Kullanıcı var mı kontrolü yapılabilir
    var user = await _context.Users.FindAsync(supportRequest.user_id);
    if (user == null)
    {
        return BadRequest("User not found.");
    }

    _context.SupportRequests.Add(supportRequest);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetSupportRequest), new { id = supportRequest.id }, supportRequest);
}


        // PUT: api/SupportRequests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupportRequest(int id, SupportRequest updatedSupportRequest)
        {
            if (id != updatedSupportRequest.id)
            {
                return BadRequest("ID mismatch.");
            }

            var existingSupportRequest = await _context.SupportRequests.FindAsync(id);
            if (existingSupportRequest == null)
            {
                return NotFound();
            }

            existingSupportRequest.name = updatedSupportRequest.name;
            existingSupportRequest.subject = updatedSupportRequest.subject;
            existingSupportRequest.email = updatedSupportRequest.email;
            existingSupportRequest.message = updatedSupportRequest.message;
            existingSupportRequest.updated_at = updatedSupportRequest.updated_at;

            _context.Entry(existingSupportRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupportRequestExists(id))
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

        // DELETE: api/SupportRequests/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupportRequest(int id)
        {
            var supportRequest = await _context.SupportRequests.FindAsync(id);
            if (supportRequest == null)
            {
                return NotFound();
            }

            _context.SupportRequests.Remove(supportRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupportRequestExists(int id)
        {
            return _context.SupportRequests.Any(e => e.id == id);
        }
    }
}
