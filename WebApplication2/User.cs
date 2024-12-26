using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApplication2;
using WebApplication2.Models; 


public class Users
{
    public int id { get; set; }
    public string name { get; set; }

    [Required]
    [EmailAddress]
    public string email { get; set; }
    public string password { get; set; }
    public string phone { get; set; }
    public int age { get; set; } = 0;
    public string gender { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
    public bool deleted { get; set; } = false;

    [JsonIgnore]
    public virtual ICollection<SupportRequest> SupportRequests { get; set; }
    public ICollection<reservations> reservations { get; set; }


    // Yeni Role sütunu
    [Required]
    [MaxLength(50)]
    public string role { get; set; } = "user"; // Varsayılan rol "user"
}
