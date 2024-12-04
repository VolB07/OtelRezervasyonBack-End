using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class SupportRequest
{
    [Key]
    public int id { get; set; }

    [Required]
    public int user_id { get; set; }

    [Required]
    [StringLength(255)]
    public string name { get; set; }

    [Required]
    [StringLength(255)]
    public string subject { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string email { get; set; }

    [Required]
    public string message { get; set; }

    public bool deleted { get; set; }

    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public virtual Users? User { get; set; }  // Kullanıcıyı JSON çıktısından dışla
}
