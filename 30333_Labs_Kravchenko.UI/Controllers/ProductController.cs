using Microsoft.AspNetCore.Mvc;
using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.UI.Services;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public ProductController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
            if (pageNo == 1 && !HttpContext.Request.Query.ContainsKey("pageno"))
            {
                var queryParams = new Dictionary<string, string?>
                {
                    { "pageno", "1" },
                    { "category", category }
                };
                return RedirectToAction("Index", "Product", queryParams);
            }

            var productResponse = await _productService.GetProductListAsync(category, pageNo);
            if (!productResponse.Success)
            {
                ViewData["Error"] = productResponse.ErrorMessage;
                return View(new ProductListModel<Medication> { Items = new List<Medication>(), CurrentPage = 1, TotalPages = 1 });
            }

            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            ViewData["categories"] = categoriesResponse.Success ? categoriesResponse.Data : new List<Category>();
            ViewData["currentCategory"] = category == null ? "Все" : (categoriesResponse.Success && categoriesResponse.Data != null ? categoriesResponse.Data.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все" : "Все");

            return View(productResponse.Data);
        }
    }
}