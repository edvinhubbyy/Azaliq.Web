using Azaliq.Data.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azaliq.Common.Constants.EntityConstants;
using static Azaliq.Common.Constants.EntityConstants.ProductDto;

namespace Azaliq.Data.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }  // Change Id to Guid
        public Guid CustomerId { get; set; }  // GUID for customer ID
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Foreign key for ApplicationUser
        public Guid ApplicationUserId { get; set; }  // Foreign key to ApplicationUser

        // Navigation property to ApplicationUser
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }

}