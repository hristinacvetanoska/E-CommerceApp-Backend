using E_CommerceApp_Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace E_CommerceApp_Backend.Authentication
{
    public class ApplicationUser : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
        //public Cart Cart { get; set; }
        //public ICollection<Order> Orders { get; set; }
    }
}
