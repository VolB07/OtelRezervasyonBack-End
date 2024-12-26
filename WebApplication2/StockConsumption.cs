namespace WebApplication2
{
    public class StockConsumption
    {
        public int Id { get; set; }
        public int DishId { get; set; } // Yemek ID
        public int MaterialId { get; set; } // Malzeme ID
        public DateTime ConsumptionDate { get; set; } // Tüketim Tarihi
        public decimal QuantityConsumed { get; set; } // Tüketilen Miktar
        public decimal RemainingStock { get; set; } // Kalan Stok
        public int UserId { get; set; } // İşlemi Yapan Kullanıcı ID

        // Navigation Properties
        public Dish? Dish { get; set; }
        public materials? Material { get; set; }
    }

}
