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
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(ICategoryService categoryService, IProductService productService, ILogger<EditModel> logger)
        {
            _categoryService = categoryService;
            _productService = productService;
            _logger = logger;
        }

        [BindProperty]
        public Medication Medication { get; set; } = new();
        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (!response.Success)
            {
                _logger.LogError($"Error retrieving medication {id}: {response.ErrorMessage}");
                return NotFound();
            }

            Medication = response.Data;
            var categoryListData = await _categoryService.GetCategoryListAsync();
            if (categoryListData.Success)
            {
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name", Medication.CategoryId);
            }
            else
            {
                _logger.LogError($"Error retrieving categories: {categoryListData.ErrorMessage}");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categoryListData = await _categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name", Medication.CategoryId);
                return Page();
            }

            try
            {
                await _productService.UpdateProductAsync(Medication.Id, Medication, Image);
                _logger.LogInformation($"Updated medication: {Medication.Id}");
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating medication {Medication.Id}: {ex.Message}");
                ModelState.AddModelError(string.Empty, ex.Message);
                var categoryListData = await _categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name", Medication.CategoryId);
                return Page();
            }
        }
    }
}