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
    public class ProductDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }
    }
}
