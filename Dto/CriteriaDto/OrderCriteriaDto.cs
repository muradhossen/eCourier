using eCourier.Helper;

namespace eCourier.Dto.CriteriaDto
{
    public class OrderCriteriaDto : PaginationParams
    {
        public string ConsignmentNumber { get; set; }
        public int MyProperty { get; set; }
    }
}
