using Azaliq.Data.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azaliq.Web.ViewModels.Product
{
    public class ProductFormInputModel
    {

        public string Id { get; set; } = string.Empty;  // for Edit, empty for Add

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double Price { get; set; }

        public Guid CategoryId { get; set; }

        // Remove Category navigation property from input model
        // public Category Category { get; set; } = null!;

        public int StockQuantity { get; set; }

        // Used to upload image in the form
        public IFormFile? ImageFile { get; set; }

        // The current image URL if editing (optional)
        public string? ImageUrl { get; set; }

        // For dropdown list of categories in the form
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

    }
}
