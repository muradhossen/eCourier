using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;

namespace eCourier.Helper
{
    public class OrderStatus
    {

        public const int DefaultOrderStatus = 1;

        public IEnumerable<SelectListItem> GetStatus()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Pending" },
                new SelectListItem { Value = "2", Text = "Approved" },
                new SelectListItem { Value = "2", Text = "Delivered" },
            };
        }

        public string GetStatusByValue(int value) =>
            GetStatus().Where(c => c.Value == value.ToString()).FirstOrDefault()?.Text;
    }


}
