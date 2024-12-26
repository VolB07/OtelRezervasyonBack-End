namespace WebApplication2
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!; // Örnek: "ana-yemek", "tatlı"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<DishMaterial>? DishMaterials { get; set; }
        public ICollection<StockConsumption>? StockConsumptions { get; set; }
    }

}
