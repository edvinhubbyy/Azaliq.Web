using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Data.Models.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Foreign key to ApplicationUser (represents the relationship)
        public Guid ApplicationUserId { get; set; }

        // Navigation property to ApplicationUser
        public ApplicationUser ApplicationUser { get; set; } = null!;

        // Other navigation properties, such as the related OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } 
            = new HashSet<OrderItem>();
    }

}