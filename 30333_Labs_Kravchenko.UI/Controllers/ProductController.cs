using _30333_Labs_Kravchenko.UI.Services;
using Microsoft.AspNetCore.Mvc;
using _30333_Labs_Kravchenko.Domain.Models;
using _30333_Labs_Kravchenko.Domain.Entities;

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
            // Если pageNo = 1 и параметр отсутствует в запросе, перенаправить на URL с ?pageno=1
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
                return View(new ProductListModel<Medication>());
            }

            var categoriesResponse = await _categoryService.GetCategoryListAsync();
            ViewData["categories"] = categoriesResponse.Data;
            ViewData["currentCategory"] = category == null ? "Все" : categoriesResponse.Data.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";

            return View(productResponse.Data);
        }
    }
}