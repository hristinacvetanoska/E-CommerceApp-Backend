using E_CommerceApp_Backend.Authentication;

namespace E_CommerceApp_Backend.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
       // public ICollection<Product> Products { get; set; }
    }
}
