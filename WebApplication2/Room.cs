using System;
using System.ComponentModel.DataAnnotations;
namespace WebApplication2
{
    public class rooms
    {
        public int id { get; set; }

        [Required]
        public int hotel_id { get; set; }

        [Required]
        public int room_type_id { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal base_price { get; set; }

        [Required]
        [StringLength(20)]
        public string status { get; set; } // "available", "occupied", "maintenance"

        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
        public bool deleted { get; set; } = false;

        // Optional olarak değiştirildi
        public virtual Hotels? hotel { get; set; }
        public virtual roomtypes? roomType { get; set; }
    }

}
