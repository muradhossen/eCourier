using Microsoft.AspNetCore.Identity;

namespace eCourier.Models
{
    public class AppUser : IdentityUser<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
