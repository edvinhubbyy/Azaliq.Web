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
    public class OrderItemDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        [Range(0, 100)]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
