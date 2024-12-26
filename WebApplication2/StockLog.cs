namespace WebApplication2
{
    public class StockLog
    {
        public int Id { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public int MaterialId { get; set; } // Malzeme ID
        public string ActionType { get; set; } = null!; // Örnek: "ekleme", "güncelleme", "tüketim"
        public decimal QuantityChanged { get; set; } // Değişen Miktar
        public int UserId { get; set; } // İşlemi Yapan Kullanıcı ID

        // Navigation Properties
        public materials? Material { get; set; }
    }

}
