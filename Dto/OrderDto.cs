using eCourier.Models;

namespace eCourier.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime PlaceDate { get; set; } = DateTime.UtcNow;
        public DateTime ReachDate { get; set; } = DateTime.UtcNow.AddDays(1);
        public CustomerDto Customer { get; set; }
        public int CustomerId { get; set; }
        public CustomerDto Recipient { get; set; }
        public int RecipientId { get; set; }
        public int Status { get; set; }
        public double TotalAmount { get; set; }
        public double DueAmount { get; set; }
        public double PaidAmount { get; set; }
        public ICollection<ProductDto> Products { get; set; }
        public string ConsignmentNumber { get; set; }
    }
}
