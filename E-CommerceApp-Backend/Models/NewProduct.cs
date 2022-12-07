using E_CommerceApp_Backend.Authentication;

namespace E_CommerceApp_Backend.Models
{
    public class NewProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string PictureUrl { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int QuantityInStock { get; set; }
        public int UserId { get; set; }
        public string SellerName { get; set; }
        public ApplicationUser User { get; set; }
    }
}
