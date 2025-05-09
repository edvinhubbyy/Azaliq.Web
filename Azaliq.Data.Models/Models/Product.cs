using static System.Net.Mime.MediaTypeNames;
using System;

namespace Azaliq.Data.Models.Models
{
    public class Product
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public int StockQuantity { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
            = new HashSet<OrderItem>();

        public ICollection<ApplicationUserProduct> FavoritedBy { get; set; } 
            = new HashSet<ApplicationUserProduct>();


    }
}