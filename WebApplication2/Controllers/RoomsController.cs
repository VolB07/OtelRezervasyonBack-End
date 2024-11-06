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

        // Belirli bir otel için odaları listeleme
        [HttpGet("hotel/{hotel_id}")]
        public IActionResult GetRoomsByHotel(int hotel_id)
        {
            var rooms = _context.rooms.Where(r => r.hotel_id == hotel_id && !r.deleted).ToList();
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
    }
}
