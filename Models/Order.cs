using System.ComponentModel.DataAnnotations;

namespace eCourier.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string ConsignmentNumber { get; set; }
        public DateTime PlaceDate { get; set; }
        public DateTime ReachDate { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public Customer Recipient { get; set; }
        public int RecipientId { get; set; }
        public int Status { get; set; }
        public double TotalAmount { get; set; }
        public double DueAmount { get; set; }
        public double PaidAmount { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
