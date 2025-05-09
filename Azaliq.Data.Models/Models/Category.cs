using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Data.Models.Models
{
    public class Category
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public ICollection<Product> Products { get; set; }
            = new HashSet<Product>();

    }
}