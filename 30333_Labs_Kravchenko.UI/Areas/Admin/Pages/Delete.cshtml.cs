using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _30333_Labs_Kravchenko.UI.Services;
using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.UI.Areas.Admin.Pages
{
    [Authorize(Policy = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IProductService productService, ILogger<DeleteModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [BindProperty]
        public Medication Medication { get; set; }

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _productService.DeleteProductAsync(id.Value);
                _logger.LogInformation($"Deleted medication: {id}");
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting medication {id}: {ex.Message}");
                return NotFound();
            }
        }
    }
}