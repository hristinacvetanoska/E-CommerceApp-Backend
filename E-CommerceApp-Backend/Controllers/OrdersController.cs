using E_CommerceApp_Backend.Authentication;
using E_CommerceApp_Backend.DTOs;
using E_CommerceApp_Backend.Extensions;
using E_CommerceApp_Backend.Models;
using E_CommerceApp_Backend.Models.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApp_Backend.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly ECommerceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ECommerceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            return await _context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name)
                .ToListAsync();
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            return await _context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
                .FirstOrDefaultAsync();
        }

        [AllowAnonymous]
        [HttpGet("bestsellerByDay")]
        public  async Task<ActionResult<Product>> ProductOfDay()
        {
            var date = DateTime.Now.DayOfWeek;
            var bestsellerByDay =  _context.Items
                .Select(p => p.ItemOrdered)
                .AsNoTracking()
                .Where(t=>DateTime.Compare(t.OrderData, DateTime.Today.Date)==0);
            var distinctProductIds = await bestsellerByDay.Select(p => p.ProductId).Distinct().ToListAsync();

            var count = 0;
            
            var productId = 0;
            var duplicates = new List<KeyValuePair<int, int>>();
            if (distinctProductIds.Any())
            {
                foreach (var item in distinctProductIds)
                {
                    var countIds = bestsellerByDay.Where(p => p.ProductId == item).Count();
                    duplicates.Add(new KeyValuePair<int, int>(item, countIds));
                }
                var max = duplicates.MaxBy(c => c.Value).Key;
                return await _context.Products.Where(p => p.Id == max).FirstOrDefaultAsync();
            }
            return null;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
        {

            var basket = await _context.Baskets
                .RetrieveBasketWithItems(User.Identity.Name)
                .FirstOrDefaultAsync();

            if (basket == null) return BadRequest(new ProblemDetails { Title = "Could not locate basket" });
            
            
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _context.Products.FindAsync(item.ProductId);
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl,
                    OrderData = DateTime.Today.Date,
            };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity,
                };
                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 1500 ? 0 : 250;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
            };

            _context.Orders.Add(order);
            _context.Baskets.Remove(basket);

            if (orderDto.SaveAddress)
            {
                var user = await _context.Users
                    .Include(a => a.Address)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
               
                var address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    Address2 = orderDto.ShippingAddress.Address2,
                    City = orderDto.ShippingAddress.City,
                    Zip = orderDto.ShippingAddress.Zip,
                    Country = orderDto.ShippingAddress.Country
                };
                user.Address = address;
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);

            return BadRequest(new ProblemDetails { Title = "Problem creating order" });
        }
    }
}
