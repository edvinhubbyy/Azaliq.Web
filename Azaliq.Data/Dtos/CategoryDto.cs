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
    public class CategoryDto
    {
        public Guid Id { get; set; }

        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;
    }
}
