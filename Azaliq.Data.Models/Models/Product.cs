using static System.Net.Mime.MediaTypeNames;
using System;

namespace Azaliq.Data.Models.Models
{
    public class Product
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double Price { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
            = new HashSet<OrderItem>();

        public ICollection<ApplicationUserProduct> FavoritedBy { get; set; } 
            = new HashSet<ApplicationUserProduct>();


    }
}