using Azaliq.Data.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Azaliq.Data
{
    public class AzaliqDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        // This constructor is for debugging purposes
        public AzaliqDbContext()
        {
        }

        public AzaliqDbContext(DbContextOptions<AzaliqDbContext> options)
            : base(options)
        {
        }

        // Db Sets
        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;

        public DbSet<ApplicationUserProduct> ApplicationUserProducts { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
