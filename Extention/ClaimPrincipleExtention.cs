using System.Security.Claims;

namespace eCourier.Extention
{
    public static class ClaimPrincipleExtention
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
          return Convert.ToInt32(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        
    }
}
