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
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public Guid ApplicationUserId { get; set; }

    }

}