using E_CommerceApp_Backend.Authentication;
using Microsoft.AspNetCore.Identity;

namespace E_CommerceApp_Backend.Models
{
    public static class DbInitializer
    {
        public static async Task Initialize(ECommerceContext context, UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "bob",
                    Email = "bob@test.com"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");

                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] { "Member", "Admin" });
            }

            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Men's Plaid Shirt",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Price = 1990,
                    PictureUrl = "/images/products/product6.jpg",
                    Brand = "Men Fashion",
                    Type = "Shirt",
                    QuantityInStock = 10
                },
                new Product
                {
                    Name = "Striped Shirt",
                    Description =
                        "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                    Price = 1800,
                    PictureUrl = "/images/products/product7.jpg",
                    Brand = "Men Fashion",
                    Type = "Shirt",
                    QuantityInStock = 25
                },
                new Product
                {
                    Name = "Cotton Shirt for Man",
                    Description =
                        "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Price = 1450,
                    PictureUrl = "/images/products/product10.jpg",
                    Brand = "Men Fashion",
                    Type = "Shirt",
                    QuantityInStock = 15
                },
                new Product
                {
                    Name = "Metal Frame Sunglasses",
                    Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
                    Price = 1190,
                    PictureUrl = "/images/products/product8.jpg",
                    Brand = "Bags & Accessories",
                    Type = "Accessories",
                    QuantityInStock = 5
                },
                new Product
                {
                    Name = "Soft Coat",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Price = 6000,
                    PictureUrl = "/images/products/product11.png",
                    Brand = "Women Fashion",
                    Type = "Coats/Trench Coats",
                    QuantityInStock = 5
                },
                new Product
                {
                    Name = "Nike Air Max Sneakers Running",
                    Description =
                        "Suspendisse dui purus, scelerisque at, vulputate vitae, pretium mattis, nunc. Mauris eget neque at sem venenatis eleifend. Ut nonummy.",
                    Price = 2590,
                    PictureUrl = "/images/products/product1.png",
                    Brand = "Nike",
                    Type = "Shoes",
                    QuantityInStock = 100
                },
                new Product
                {
                    Name = "Basketball Sneakers",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
                    Price = 2999,
                    PictureUrl = "/images/products/product2.png",
                    Brand = "Nike",
                    Type = "Shoes",
                    QuantityInStock = 100
                },
                new Product
                {
                    Name = "Nike Air Max",
                    Description =
                        "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
                    Price = 1990,
                    PictureUrl = "/images/products/product3.png",
                    Brand = "Nike",
                    Type = "Shoes",
                    QuantityInStock = 100
                },
                new Product
                {
                    Name = "City Rider Sneakers",
                    Description = "Aenean nec lorem. In porttitor. Donec laoreet nonummy augue.",
                    Price = 2540,
                    PictureUrl = "/images/products/product4.jpg",
                    Brand = "Puma",
                    Type = "Shoes",
                    QuantityInStock = 100
                },
                new Product
                {
                    Name = "Suede Classix Women's Sneakers",
                    Description =
                        "The Suede hit the scene in 1968 and has been changing the game ever since.",
                    Price = 1890,
                    PictureUrl = "/images/products/product5.jpg",
                    Brand = "Puma",
                    Type = "Shoes",
                    QuantityInStock = 100
                },
            };

            foreach (var product in products)
            {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}