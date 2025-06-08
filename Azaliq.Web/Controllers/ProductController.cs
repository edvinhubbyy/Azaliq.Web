using Azaliq.Services.Services.Interfaces;
using Azaliq.Web.ViewModels;
using Azaliq.Web.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Azaliq.Common.Constants.EntityConstants.Services;

namespace Azaliq.Web.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AllProductsIndexViewModel> allProducts = await this._productService.GetAllProductsAsync();

            return View(allProducts);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(ProductFormInputModel inputModel)
        {

            if (!ModelState.IsValid)
            {
                return View(inputModel);

            }

            try
            {
                await this._productService.AddProductAsync(inputModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                this.ModelState.AddModelError(string.Empty, ServiceCreatingError);
                return this.View(inputModel);
            }



        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await _productService.GetAllCategoriesAsync();

            ViewBag.Categories = categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
                .ToList();

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {

            try
            {
                ProductDetailsViewModel? movieDetails = await this._productService.GetProductDetailsByIdAsync(id);

                if (movieDetails == null)
                {
                    // TODO: Custom 404 page
                    return this.RedirectToAction(nameof(Index));
                }

                return View(movieDetails);
            }
            catch (Exception e)
            {
                // TODO: Implement a logger to log the error
                // TODO:  Add JS bars to indicate that the movie was not found
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index));
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                ProductFormInputModel? editableProduct = await this._productService.GetProductForEditAsync(id);

                if (editableProduct == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Add this:
                var categories = await _productService.GetAllCategoriesAsync();
                ViewBag.Categories = categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id,
                        Selected = c.Id == editableProduct.CategoryId.ToString()

                    })
                    .ToList();

                return View(editableProduct);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductFormInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _productService.GetAllCategoriesAsync();
                ViewBag.Categories = categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id,
                        Selected = c.Id == inputModel.CategoryId.ToString()
                    })
                    .ToList();

                return View(inputModel);
            }

            try
            {
                bool editSuccess = await _productService.EditProductAsync(inputModel);

                if (!editSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Confirm Delete page
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(Index));
            }

            var productForEdit = await _productService.GetProductForEditAsync(id);
            if (productForEdit == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productForEdit);
        }

        // POST: Actually delete the product
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(Index));
            }

            bool deleted = await _productService.DeleteProductAsync(id);

            if (!deleted)
            {
                // Optionally add an error message here
            }

            return RedirectToAction(nameof(Index));
        }


    }
}
