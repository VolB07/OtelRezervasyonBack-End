using System.ComponentModel.DataAnnotations;

namespace WebApplication2
{
    public class roomtypes
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public string description { get; set; }
        public string image_url { get; set; }


        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
        public bool deleted { get; set; } = false;
    }
}
