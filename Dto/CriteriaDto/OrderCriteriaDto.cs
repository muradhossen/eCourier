using eCourier.Helper;

namespace eCourier.Dto.CriteriaDto
{
    public class OrderCriteriaDto : PaginationParams
    {
        public string ConsignmentNumber { get; set; }
        public int? AppUserId { get; set; } = null;
    }
}
