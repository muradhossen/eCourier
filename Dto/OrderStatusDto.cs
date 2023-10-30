namespace eCourier.Dto
{
    public class OrderStatusDto
    {
        public string SenderName { get; set; }
        public string RecipientName { get; set; }
        public DateTime ReachDate { get; set; }
        public double DueAmount { get; set; }
        public int Status { get; set; }
    }
}
