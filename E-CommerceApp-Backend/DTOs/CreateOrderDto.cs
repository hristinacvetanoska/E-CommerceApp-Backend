using E_CommerceApp_Backend.Models.OrderAggregate;

namespace E_CommerceApp_Backend.DTOs
{
    public class CreateOrderDto
    {
        public bool SaveAddress { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
    }
}
