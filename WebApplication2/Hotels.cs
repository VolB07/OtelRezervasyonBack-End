using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2
{
    public class Hotels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        [MaxLength(20)]
        public string phone { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        public string description { get; set; }

        [Range(1, 5)]
        public int? star_rating { get; set; }

        [MaxLength(255)]
        public string image_url { get; set; }

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public DateTime updated_at { get; set; } =  DateTime.UtcNow;

        public bool deleted { get; set; } = false;
    }
}
