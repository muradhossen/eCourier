namespace eCourier.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double? Height { get; set; }
        public double? Width { get; set; }
        public double? Weight { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
    }
}
