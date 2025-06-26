using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _30333_Labs_Kravchenko.API.Data;
using _30333_Labs_Kravchenko.Domain.Entities;
using _30333_Labs_Kravchenko.Domain.Models;

namespace _30333_Labs_Kravchenko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MedicationsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            Console.WriteLine("MedicationsController initialized");
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<ProductListModel<Medication>>>> GetMedications(
            string? category, int pageNo = 1, int pageSize = 3)
        {
            var result = new ResponseData<ProductListModel<Medication>>();

            var data = _context.Medications
                .Include(m => m.Category)
                .Where(m => string.IsNullOrEmpty(category) || m.Category.NormalizedName.Equals(category))
                .Where(m => m.Category.NormalizedName != "soups");

            int totalPages = (int)Math.Ceiling((double)data.Count() / pageSize);

            if (pageNo > totalPages)
                pageNo = totalPages;

            var listData = new ProductListModel<Medication>
            {
                Items = await data
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(),
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            result.Data = listData;

            if (!data.Any())
            {
                result.Success = false;
                result.ErrorMessage = "Нет препаратов в выбранной категории";
            }
            else
            {
                result.Success = true;
            }

            Console.WriteLine($"Medications retrieved: {listData.Items.Count} on page {pageNo}");

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medication>> GetMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);

            if (medication == null)
            {
                return NotFound();
            }

            return medication;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedication(int id, Medication medication)
        {
            if (id != medication.Id)
            {
                return BadRequest();
            }

            _context.Entry(medication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Medication>> PostMedication(Medication medication)
        {
            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedication", new { id = medication.Id }, medication);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SaveImage(int id, IFormFile image)
        {
            Console.WriteLine("SaveImage called");
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            var imagesPath = Path.Combine(_env.WebRootPath, "Images");
            var randomName = Path.GetRandomFileName();
            var extension = Path.GetExtension(image.FileName);
            var fileName = Path.ChangeExtension(randomName, extension);
            var filePath = Path.Combine(imagesPath, fileName);

            using var stream = System.IO.File.OpenWrite(filePath);
            await image.CopyToAsync(stream);

            var host = "https://" + Request.Host;
            var url = $"{host}/Images/{fileName}";
            medication.Image = url;
            Console.WriteLine($"Saved URL: {url}");
            await _context.SaveChangesAsync();
            _context.Entry(medication).Reload();

            return Ok();
        }

        [HttpPost("UpdateImage/{id}")]
        public async Task<IActionResult> UpdateImage(int id, IFormFile image)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            var imagesPath = Path.Combine(_env.WebRootPath, "Images");

            if (!string.IsNullOrEmpty(medication.Image))
            {
                var oldImagePath = Path.Combine(imagesPath, Path.GetFileName(medication.Image));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            var randomName = Path.GetRandomFileName();
            var extension = Path.GetExtension(image.FileName);
            var fileName = Path.ChangeExtension(randomName, extension);
            var filePath = Path.Combine(imagesPath, fileName);

            using var stream = System.IO.File.OpenWrite(filePath);
            await image.CopyToAsync(stream);

            var host = "https://" + Request.Host;
            medication.Image = $"{host}/Images/{fileName}";
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(medication.Image))
            {
                var imagePath = Path.Combine(_env.WebRootPath, "Images", Path.GetFileName(medication.Image));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicationExists(int id)
        {
            return _context.Medications.Any(e => e.Id == id);
        }
    }
}