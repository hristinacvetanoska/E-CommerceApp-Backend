﻿using E_CommerceApp_Backend.Models.OrderAggregate;

namespace E_CommerceApp_Backend.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public long Subtotal { get; set; }
        public long DeliveryFee { get; set; }
        public string OrderStatus { get; set; }
        //public string PaymentIntentId { get; set; }
        public long Total { get; set; }
    }
}
