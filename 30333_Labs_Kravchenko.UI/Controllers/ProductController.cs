//using _30333_Labs_Kravchenko.UI.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace _30333_Labs_Kravchenko.UI.Controllers
//{
//    public class ProductController(ICategoryService categoryService, IProductService productService) : Controller
//    {
//        public async Task<IActionResult> Index()
//        {
//            var productResponse = await productService.GetProductListAsync(null);
//            if (!productResponse.Success)
//                return NotFound(productResponse.ErrorMessage);
//            return View(productResponse.Data.Items);
//        }
//    }

//}

using _30333_Labs_Kravchenko.UI.Services;
using Microsoft.AspNetCore.Mvc;
using _30333_Labs_Kravchenko.Domain.Models;
using _30333_Labs_Kravchenko.Domain.Entities;

namespace _30333_Labs_Kravchenko.UI.Controllers
{
    public class ProductController(ICategoryService categoryService, IProductService productService) : Controller
    {
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IProductService _productService = productService;

        public async Task<IActionResult> Index(string? category, int pageNo = 1)
        {
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