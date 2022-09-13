using E_CommerceApp_Backend.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApp_Backend.Models
{
    public class ECommerceContext : IdentityDbContext<ApplicationUser>
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options) : base(options) { }
        public DbSet<Cart> Carts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ApplicationUser>()
            //    .HasOne(p => p.Cart)
            //    .WithOne(b => b.ApplicationUser)
            //    .HasForeignKey<Cart>(c => c.UserId);
        }
        }
}
