using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azaliq.Web.ViewModels;
using Azaliq.Web.ViewModels.Product;

namespace Azaliq.Services.Services.Interfaces
{
    public interface IProductService
    {

        Task<IEnumerable<AllProductsIndexViewModel>> GetAllProductsAsync();

        Task AddProductAsync(ProductFormInputModel inputModel);

        Task<ProductDetailsViewModel?> GetProductDetailsByIdAsync(string? id);

        Task<ProductFormInputModel?> GetProductForEditAsync(string id);

        Task<bool> EditProductAsync(ProductFormInputModel inputModel);

        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

        Task<bool> DeleteProductAsync(string id);


    }
}
