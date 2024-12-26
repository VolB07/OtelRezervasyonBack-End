namespace WebApplication2
{
    public class materials
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!; // Malzeme Kodu
        public decimal StockQuantity { get; set; }
        public string Unit { get; set; } = null!; // Örnek: "kg", "litre"
        public DateTime? ExpiryDate { get; set; } // Son Kullanma Tarihi
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<StockLog>? StockLogs { get; set; }
        public ICollection<DishMaterial>? DishMaterials { get; set; }
    }

}
