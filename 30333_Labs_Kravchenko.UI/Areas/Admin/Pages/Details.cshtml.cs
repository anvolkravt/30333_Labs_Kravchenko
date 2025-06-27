using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _30333_Labs_Kravchenko.UI.Services;
using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.UI.Areas.Admin.Pages
{
    [Authorize(Policy = "admin")]
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(IProductService productService, ILogger<DetailsModel> logger)
        {
            _productService = productService;
            _logger = logger;
        }

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
    }
}