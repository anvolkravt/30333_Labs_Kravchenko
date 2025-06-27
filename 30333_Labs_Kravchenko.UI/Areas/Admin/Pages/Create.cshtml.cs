using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _30333_Labs_Kravchenko.UI.Services;
using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.UI.Areas.Admin.Pages
{
    [Authorize(Policy = "admin")]
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ICategoryService categoryService, IProductService productService, ILogger<CreateModel> logger)
        {
            _categoryService = categoryService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var categoryListData = await _categoryService.GetCategoryListAsync();
            if (categoryListData.Success)
            {
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
            }
            else
            {
                _logger.LogError($"Error retrieving categories: {categoryListData.ErrorMessage}");
            }
            return Page();
        }

        [BindProperty]
        public Medication Medication { get; set; } = new();
        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categoryListData = await _categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
                return Page();
            }

            var response = await _productService.CreateProductAsync(Medication, Image);
            if (!response.Success)
            {
                _logger.LogError($"Error creating medication: {response.ErrorMessage}");
                ModelState.AddModelError(string.Empty, response.ErrorMessage);
                var categoryListData = await _categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
                return Page();
            }

            _logger.LogInformation($"Created medication: {Medication.Name}");
            return RedirectToPage("./Index");
        }
    }
}