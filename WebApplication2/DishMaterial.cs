namespace WebApplication2
{
    public class DishMaterial
    {
        public int Id { get; set; }
        public int DishId { get; set; } // Yemek ID
        public int MaterialId { get; set; } // Malzeme ID
        public decimal QuantityRequired { get; set; } // Gerekli Miktar
        public string Unit { get; set; } = null!; // Örnek: "kg", "litre"

        // Navigation Properties
        public Dish? Dish { get; set; }
        public materials? Material { get; set; }
    }

}
