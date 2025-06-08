using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Web.ViewModels
{
    public class AllProductsIndexViewModel
    {

        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Price { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }


    }
}
