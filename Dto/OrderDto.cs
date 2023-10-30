using eCourier.Models;
using System.ComponentModel;

namespace eCourier.Dto
{
    public class OrderDto
    {
        private double _paidAmount; 

        public int Id { get; set; }
        [DisplayName("Place Date")]
        public DateTime PlaceDate { get; set; } = DateTime.UtcNow;
        [DisplayName("Reach Date")]
        public DateTime ReachDate { get; set; }
        public CustomerDto Recipient { get; set; }
        public int RecipientId { get; set; }
        public int Status { get; set; } 
        [DisplayName("Total Amount")]
        public double TotalAmount { get; set; }
        [DisplayName("Due Amount")]
        public double DueAmount { get; set; }
        [DisplayName("Paid Amount")]
        public double PaidAmount
        {
            get => _paidAmount; set
            {
                if (value > TotalAmount)
                {
                    throw new Exception($"Cannot pay more then {TotalAmount}");
                }

                _paidAmount = value;
            }
        }
        public ICollection<ProductDto> Products { get; set; }
        [DisplayName("Consignment No")]
        public string ConsignmentNumber { get; set; }
        public int AppUserId { get; set; }
        [DisplayName("Name")]
        public string AppUserUserName { get; set; }
        [DisplayName("Email")]
        public string AppUserEmail { get; set; }
    }
}
