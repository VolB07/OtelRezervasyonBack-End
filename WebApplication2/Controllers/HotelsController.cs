using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HotelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("filter")]
        public async Task<IActionResult> GetFilteredHotels([FromBody] HotelFilterModel filter)
        {
            var query = _context.Hotels.AsQueryable();

            if (!string.IsNullOrEmpty(filter.address))
            {
                query = query.Where(h => h.address.Contains(filter.address));
            }
            if (filter.star_rating.HasValue)
            {
                query = query.Where(h => h.star_rating == filter.star_rating);
            }

            var hotels = await query.ToListAsync();
            return Ok(hotels);
        }



        // GET: api/Hotels - Tüm otelleri getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotels>>> GetHotels()
        {
            return await _context.Hotels.ToListAsync();
        }

        // GET: api/Hotels/5 - Belirli bir oteli ID'ye göre getir
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotels>> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return hotel;
        }

        // POST: api/Hotels - Yeni bir otel ekle
        [HttpPost]
        public async Task<ActionResult<Hotels>> CreateHotel(Hotels hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHotel", new { id = hotel.id }, hotel);
        }

        // PUT: api/Hotels/5 - Bir otelin bilgilerini güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, Hotels hotel)
        {
            if (id != hotel.id) // Eğer gelen id ile URL'deki id eşleşmiyorsa
            {
                return BadRequest("ID uyuşmuyor.");
            }

            var existingHotel = await _context.Hotels.FindAsync(id);
            if (existingHotel == null) // Güncellenmek istenen otel mevcut değilse
            {
                return NotFound("Otel bulunamadı.");
            }

            // Güncelleme işlemi
            existingHotel.name = hotel.name;
            existingHotel.address = hotel.address;
            existingHotel.phone = hotel.phone;
            existingHotel.email = hotel.email;
            existingHotel.star_rating = hotel.star_rating;
            existingHotel.image_url = hotel.image_url;
            existingHotel.description = hotel.description;
            existingHotel.updated_at = DateTime.UtcNow; // Güncellenme zamanını ayarlama

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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

        // DELETE: api/Hotels/5 - Bir oteli sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.id == id);
        }
    }
}
