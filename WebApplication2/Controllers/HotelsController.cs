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

        // GET: api/Hotels
        // Belirli bir kullanıcıya ait otelleri listelemek için userId parametresini kullan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotels>>> GetHotels(int? userId)
        {
            if (userId.HasValue)
            {
                // Sadece ilgili kullanıcıya ait otelleri getir
                return await _context.Hotels.Where(h => h.user_id == userId.Value).ToListAsync();
            }
            return await _context.Hotels.ToListAsync(); // Tüm otelleri getir
        }

        // POST: api/Hotels
        [HttpPost]
        public async Task<ActionResult<Hotels>> CreateHotel(Hotels hotel)
        {
            if (hotel.user_id == null || hotel.user_id <= 0)
            {
                return BadRequest("Geçerli bir user_id gereklidir.");
            }

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHotel", new { id = hotel.id }, hotel);
        }

        // PUT: api/Hotels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, Hotels hotel)
        {
            if (id != hotel.id)
            {
                return BadRequest("ID uyuşmuyor.");
            }

            var existingHotel = await _context.Hotels.FindAsync(id);
            if (existingHotel == null)
            {
                return NotFound("Otel bulunamadı.");
            }

            // Kullanıcı ID'si değiştirilemez
            if (existingHotel.user_id != hotel.user_id)
            {
                return BadRequest("user_id değiştirilemez.");
            }


            // Otel bilgilerini güncelle
            existingHotel.name = hotel.name;
            existingHotel.address = hotel.address;
            existingHotel.phone = hotel.phone;
            existingHotel.email = hotel.email;
            existingHotel.star_rating = hotel.star_rating;
            existingHotel.image_url = hotel.image_url;
            existingHotel.description = hotel.description;
            existingHotel.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Hotels/5
        // Belirli bir kullanıcıya ait otelleri kontrol etmek için user_id doğrulaması eklenebilir
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotels>> GetHotel(int id, int? userId)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            if (userId.HasValue && hotel.user_id != userId.Value)
            {
                return Unauthorized("Bu otel belirtilen kullanıcıya ait değil.");
            }

            return hotel;
        }

        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.id == id);
        }
    }
}
