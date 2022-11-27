using E_CommerceApp_Backend.Authentication;
using E_CommerceApp_Backend.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity;
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
                new Role {Id=1, Name="Member", NormalizedName="MEMBER"},
                new Role {Id=2, Name = "Admin", NormalizedName = "ADMIN" });
            //modelBuilder.Entity<ApplicationUser>()
            //    .HasOne(p => p.Cart)
            //    .WithOne(b => b.ApplicationUser)
            //    .HasForeignKey<Cart>(c => c.UserId);
        }
        }
}
