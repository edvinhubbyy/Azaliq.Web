using Azaliq.Services.Services.Interfaces;
using Azaliq.Web.ViewModels;
using Azaliq.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azaliq.Data;
using Azaliq.Data.Models.Models;
using static Azaliq.Common.Constants.EntityConstants;

namespace Azaliq.Services.Services
{
    public class ProductService : IProductService
    {

        private readonly AzaliqDbContext _dbContext;

        public ProductService(AzaliqDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<AllProductsIndexViewModel>> GetAllProductsAsync()
        {

            IEnumerable<AllProductsIndexViewModel> allProducts = await this._dbContext
                .Products
                .AsNoTracking()
                .Select(p => new AllProductsIndexViewModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price.ToString("F2"),
                    CategoryName = p.Category.Name,
                    StockQuantity = p.StockQuantity,
                    ImageUrl = p.ImageUrl ?? $"~/images/{NoImageUrl}"
                })
                .ToListAsync();

            return allProducts;
        }

        public async Task AddProductAsync(ProductFormInputModel inputModel)
        {
            Product newProduct = new Product
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                Price = inputModel.Price,
                CategoryId = inputModel.CategoryId,
                StockQuantity = inputModel.StockQuantity,
                ImageUrl = inputModel.ImageUrl // Set this only if you have a valid URL or after handling upload
            };

            await _dbContext.Products.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<ProductDetailsViewModel?> GetProductDetailsByIdAsync(string? id)
        {
            ProductDetailsViewModel? productDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid productId);

            if (isIdValidGuid)
            {
                productDetails = await this._dbContext
                    .Products
                    .AsNoTracking()
                    .Where(p => p.Id == productId)
                    .Select(p => new ProductDetailsViewModel
                    {
                        Id = p.Id.ToString(),
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        CategoryName = p.Category.Name,
                        StockQuantity = p.StockQuantity,
                        ImageUrl = p.ImageUrl ?? $"~/images/{NoImageUrl}"

                    })
                    .SingleOrDefaultAsync();
            }

            return productDetails;
        }

        public async Task<ProductFormInputModel?> GetProductForEditAsync(string id)
        {
            ProductFormInputModel? editableProduct = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid productId);

            if (isIdValidGuid)
            {
                editableProduct = await this._dbContext
                    .Products
                    .AsNoTracking()
                    .Where(p => p.Id == productId)
                    .Select(p => new ProductFormInputModel
                    {
                        Id = p.Id.ToString(),               // add Id too if you want to edit
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        CategoryId = p.CategoryId,
                        StockQuantity = p.StockQuantity,
                        ImageUrl = p.ImageUrl ?? $"~/images/{NoImageUrl}"
                    })
                    .SingleOrDefaultAsync();
            }

            return editableProduct;
        }


        public async Task<bool> EditProductAsync(ProductFormInputModel inputModel)
        {
            Product? editableProduct = await this._dbContext
                .Products
                .SingleOrDefaultAsync(m => m.Id.ToString() == inputModel.Id);

            if (editableProduct == null)
            {
                return false;
            }

            editableProduct.Name = inputModel.Name;
            editableProduct.Description = inputModel.Description;
            editableProduct.Price = inputModel.Price;
            editableProduct.CategoryId = inputModel.CategoryId;
            // Do NOT set editableProduct.Category = inputModel.Category;
            editableProduct.StockQuantity = inputModel.StockQuantity;

            // Only update ImageUrl if inputModel.ImageUrl is not null or empty
            if (!string.IsNullOrEmpty(inputModel.ImageUrl))
            {
                editableProduct.ImageUrl = inputModel.ImageUrl;
            }

            await this._dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            return await this._dbContext.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id.ToString(),
                    Name = c.Name
                })
                .ToListAsync();
        }


        public async Task<bool> DeleteProductAsync(string id)
        {
            bool isValidGuid = Guid.TryParse(id, out Guid productId);

            if (!isValidGuid)
            {
                return false;
            }

            var product = await _dbContext.Products.FindAsync(productId);

            if (product == null)
            {
                return false;
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return true;
        }





    }
}
