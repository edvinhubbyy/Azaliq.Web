﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Data.Models.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Other navigation properties, such as the related OrderItems
        public ICollection<OrderItem> OrderItems { get; set; } 
            = new HashSet<OrderItem>();

        public Guid ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; } = null!;
    }

}