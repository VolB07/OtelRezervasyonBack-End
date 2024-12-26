namespace WebApplication2.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WebApplication2.Models; // Model sınıfının bulunduğu namespace


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
                .Where(r => r.user_id == userId && !r.deleted) // Filtreleme
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
                .ToListAsync(); // Verileri asenkron al


            if (reservations == null || reservations.Count == 0)
            {
                return NotFound("No reservations found.");
            }

            return Ok(reservations);
        }
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetReservationsByHotelId(int hotelId)
        {
            var reservations = await _context.reservations
                .Include(r => r.user) // User tablosunu dahil ediyoruz
                .Include(r => r.room) // Room ilişkisini dahil ediyoruz
                .ThenInclude(room => room.roomType) // RoomType ilişkisini dahil ediyoruz
                .Where(r => r.room.hotel_id == hotelId) // Belirli bir otelin rezervasyonlarını almak için filtreleme
                .Select(r => new
                {
                    ReservationId = r.id,
                    UserId = r.user_id,
                    RoomId = r.room_id,
                    HotelName = r.room.hotel.name,
                    RoomTypeName = r.room.roomType != null ? r.room.roomType.name : "N/A", // RoomType null kontrolü
                    ReservationDate = r.created_at,
                    check_in = r.check_in,
                    check_out = r.check_out,
                    total_price = r.total_price,
                    Status = r.status,
                    name = r.user != null ? r.user.name : "Bilinmeyen Kullanıcı" // Kullanıcı null kontrolü
                })
                .ToListAsync(); // Veriyi asenkron olarak almak

            if (reservations == null || !reservations.Any()) // Boş veya null kontrolü
            {
                return NotFound("No reservations found for this hotel.");
            }

            return Ok(reservations);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationStatus(int id, [FromBody] reservations updatedReservation)
        {
            var reservation = await _context.reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound("Rezervasyon bulunamadı.");
            }

            if (string.IsNullOrEmpty(updatedReservation.status))
            {
                return BadRequest("Durum bilgisi geçersiz.");
            }

            var statusMap = new Dictionary<string, string>
    {
        { "Beklemede", "pending" },
        { "İptal Edildi", "cancelled" },
        { "Onaylandı", "confirmed" }
    };

            if (statusMap.ContainsKey(updatedReservation.status))
            {
                updatedReservation.status = statusMap[updatedReservation.status];
            }

            if (!new[] { "confirmed", "pending", "cancelled" }.Contains(updatedReservation.status))
            {
                return BadRequest("Geçersiz durum.");
            }

            reservation.status = updatedReservation.status;
            reservation.updated_at = DateTime.UtcNow;

            if (updatedReservation.status == "confirmed" && updatedReservation.room_id.HasValue)
            {
                var room = await _context.rooms.FindAsync(updatedReservation.room_id);
                if (room != null)
                {
                    room.status = "occupied"; // Odayı dolu olarak güncelle
                }
                else
                {
                    return BadRequest("Oda bulunamadı.");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Rezervasyon durumu başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }





    }

}
