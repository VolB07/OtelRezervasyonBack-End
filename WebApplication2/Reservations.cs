using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2;

public class reservations
{
    [Key]
    public int id { get; set; }

    [Required]
    [ForeignKey("user")]
    public int user_id { get; set; }

    [Required]
    [ForeignKey("room")]
    public int room_id { get; set; }

    [Required]
    public DateTime check_in { get; set; }

    [Required]
    public DateTime check_out { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal total_price { get; set; }

    [Required]
    [MaxLength(20)]
    public string status { get; set; } = "pending";

    public DateTime created_at { get; set; } = DateTime.UtcNow;

    public DateTime updated_at { get; set; } = DateTime.UtcNow;

    public bool deleted { get; set; } = false;

    // Navigation Properties
    public Users? user { get; set; }
    public rooms? room { get; set; }
}
