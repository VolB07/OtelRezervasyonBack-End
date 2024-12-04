namespace WebApplication2.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;


    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Rezervasyon oluşturma
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] reservations reservation)
        {
            if (reservation == null)
                return BadRequest("Invalid reservation data.");

            // Tarih kontrolü (UTC formatı ile)
            if (reservation.check_in.ToUniversalTime() >= reservation.check_out.ToUniversalTime())
            {
                return BadRequest("Check-out date must be after check-in date.");
            }

            // Oda kontrolü
            var room = await _context.rooms.FindAsync(reservation.room_id);
            if (room == null)
            {
                return BadRequest("Room does not exist.");
            }

            // Kullanıcı kontrolü
            var user = await _context.Users.FindAsync(reservation.user_id);
            if (user == null)
            {
                return BadRequest("User does not exist.");
            }

            reservation.created_at = DateTime.UtcNow;
            reservation.updated_at = DateTime.UtcNow;

            _context.reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }

        // Belirli bir rezervasyonu getir


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetReservationsByUserId(int userId)
        {
            var reservations = await _context.reservations
                .Where(r => r.user_id == userId && !r.deleted)
                .Include(r => r.room) // Room ile ilişki
                .ThenInclude(room => room.roomType) // RoomType ile ilişki
                .Include(r => r.room.hotel) // Room ile Hotel ilişkisini dahil et
                .Select(r => new
                {
                    ReservationId = r.id,
                    UserId = r.user_id,
                    RoomId = r.room_id,
                    HotelName = r.room.hotel.name, // Otel adı
                    RoomTypeName = r.room.roomType != null ? r.room.roomType.name : "N/A", // Oda tipi adı
                    ReservationDate = r.created_at,
                    CheckInDate = r.check_in,
                    CheckOutDate = r.check_out,
                    TotalPrice = r.total_price,
                    Status = r.status
                })
                .ToListAsync();

            if (reservations == null || reservations.Count == 0)
            {
                return NotFound("No reservations found.");
            }

            return Ok(reservations);
        }





    }


}
