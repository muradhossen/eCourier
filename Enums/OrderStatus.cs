using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata.Ecma335;

namespace eCourier.Enums
{
   public class OrderStatus
    {
        public int Value { get; set; }
        public string Text { get; set; }

        public List<SelectListItem> GetStatus()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text = "Pending" },
                new SelectListItem { Value = "2", Text = "Approved" },
                new SelectListItem { Value = "2", Text = "Delivered" },
            };
        }

        public string GetStatusByValue(int value) =>
            GetStatus().Where(c => c.Value == value.ToString()).FirstOrDefault().Text;
    }


}
