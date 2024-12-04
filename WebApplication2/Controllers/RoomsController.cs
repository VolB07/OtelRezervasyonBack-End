using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<rooms>>> GetRooms()
        {
            return await _context.rooms.ToListAsync();
        }

        // Oda ekleme
        [HttpPost("add")]
        public IActionResult AddRoom([FromBody] rooms room)
        {
            if (room == null)
            {
                return BadRequest("Room is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.rooms.Add(room);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetRoom), new { id = room.id }, room);
        }





        // Oda güncelleme
        [HttpPut("update/{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] rooms room)
        {
            if (id != room.id)
            {
                return BadRequest("Room ID mismatch.");
            }

            _context.Entry(room).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // Oda silme
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteRoom(int id)
        {
            var room = _context.rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.rooms.Remove(room);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetRoomsByHotel(int hotelId)
        {
            var rooms = await _context.rooms
                .Where(r => r.hotel_id == hotelId && !r.deleted) // Otel ID'sine ve silinmiş olmayanlara göre filtrele
                .Include(r => r.roomType) // Oda tipini dahil et
                .Select(r => new
                {
                    r.id,
                    r.hotel_id,
                    r.room_type_id,
                    r.base_price,
                    r.status,
                    RoomType = r.roomType != null ? new
                    {
                        r.roomType.name,
                        r.roomType.description,
                        r.roomType.image_url
                    } : null
                })
                .ToListAsync();

            if (rooms == null || !rooms.Any())
            {
                return NotFound("No rooms found for the given hotel.");
            }

            return Ok(rooms);
        }




        // Tek bir odayı alma
        [HttpGet("{id}")]
        public IActionResult GetRoom(int id)
        {
            var room = _context.rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        // Otel ID'sine ve oda ID'sine göre oda bilgilerini al
        [HttpGet("hotel/{hotelId}/room/{roomId}")]
        public IActionResult GetRoomByHotelAndRoomId(int hotelId, int roomId)
        {
            var room = _context.rooms
                .Where(r => r.hotel_id == hotelId && r.id == roomId)    
                .FirstOrDefault();

            if (room == null)
            {
                return NotFound($"Otel ID {hotelId} ve Oda ID {roomId} ile eşleşen oda bulunamadı.");
            }

            return Ok(room);
        }
    }
}
