using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Data.Dtos
{
    public class ApplicationUserProductDto
    {
        public Guid Id { get; set; }

        public Guid ApplicationUserId { get; set; }

        public Guid ProductId { get; set; }
    }
}
