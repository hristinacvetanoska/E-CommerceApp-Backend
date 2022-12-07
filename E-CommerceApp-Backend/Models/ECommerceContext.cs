using E_CommerceApp_Backend.Authentication;
using E_CommerceApp_Backend.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApp_Backend.Models
{
    public class ECommerceContext : IdentityDbContext<ApplicationUser, Role, int>
    {
        public ECommerceContext(DbContextOptions options) : base(options) { }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> Items { get; set; }
        public DbSet<NewProduct> NewProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Address)
                .WithOne()
                .HasForeignKey<UserAddress>(a => a.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>()
                .HasData(
                new Role {Id=1, Name = "Admin", NormalizedName = "ADMIN" }, 
                new Role { Id = 2, Name = "Seller", NormalizedName = "SELLER" }, 
                new Role { Id = 3, Name = "Buyer", NormalizedName = "BUYER" });

            modelBuilder.Entity<NewProduct>()
                .HasOne(n => n.User)
                .WithMany(p => p.NewProducts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        }
}
