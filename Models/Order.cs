using System.ComponentModel.DataAnnotations;

namespace eCourier.Models
{
    public class Order
    {
        private double _dueAmount;

        public int Id { get; set; }
        [Required]
        public string ConsignmentNumber { get; set; }
        public DateTime PlaceDate { get; set; }
        public DateTime ReachDate { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Customer Recipient { get; set; }
        public int RecipientId { get; set; }
        public int Status { get; set; }
        public double TotalAmount { get; set; }
        public double PaidAmount { get; set; }
        public double DueAmount
        {
            get => _dueAmount; set
            {
                _dueAmount = TotalAmount - PaidAmount;
            }
        }
        public ICollection<Product> Products { get; set; }

    }
}
