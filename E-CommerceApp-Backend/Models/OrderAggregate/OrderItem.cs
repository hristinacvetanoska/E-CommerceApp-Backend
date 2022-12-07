using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceApp_Backend.Models.OrderAggregate
{
    [Table("OrderItems")]
    public class OrderItem
    {
        public int Id { get; set; }
        public ProductItemOrdered ItemOrdered { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
