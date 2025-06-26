using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _30333_Labs_Kravchenko.API.Data;
using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
            Console.WriteLine("CategoriesController initialized");
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<IEnumerable<Category>>>> GetCategories()
        {
            var categories = await _context.Categories
                .ToListAsync();
            Console.WriteLine($"Categories retrieved: {categories.Count}");

            var response = new ResponseData<IEnumerable<Category>>
            {
                Data = categories,
                Success = categories.Any(),
                ErrorMessage = categories.Any() ? null : "No categories found in the database"
            };
            return Ok(response);
        }
    }
}