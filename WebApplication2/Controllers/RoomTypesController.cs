using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomTypesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetRoomTypes()
        {
            var roomTypes = await _context.roomtypes
                .Where(rt => rt.deleted == false)
                .ToListAsync();

            // Hangi verinin döndüğünü kontrol et
            if (!roomTypes.Any())
            {
                return NotFound("Oda tipleri bulunamadı.");
            }

            return Ok(roomTypes);
        }

        [HttpGet("{id}")]
        public IActionResult GetRoomTypeById(int id)
        {
            var roomType = _context.roomtypes.FirstOrDefault(rt => rt.id == id && !rt.deleted);
            if (roomType == null)
            {
                return NotFound();
            }
            return Ok(roomType);
        }
    }

}

        
