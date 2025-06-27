//using _30333_Labs_Kravchenko.UI.Data;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace _30333_Labs_Kravchenko.UI.Controllers
//{
//    public class ImageController(UserManager<ApplicationUser> userManager) : Controller
//    {
//        public async Task<IActionResult> GetAvatar()
//        {
//            var email = User.FindFirst(ClaimTypes.Email)!.Value;
//            var user = await userManager.FindByEmailAsync(email);
//            if (user == null)
//            {
//                return NotFound();
//            }
//            if (user.Avatar != null)
//                return File(user.Avatar, user.MimeType);
//            var imagePath = Path.Combine("Images", "user.png");
//            return File(imagePath, "image/png");
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;

namespace _30333_Labs_Kravchenko.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly string _imagePath;
        private readonly ILogger<ImageController> _logger;

        public ImageController(IWebHostEnvironment env, ILogger<ImageController> logger)
        {
            _imagePath = Path.Combine(env.WebRootPath, "Images");
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogError("No file uploaded");
                return BadRequest("No file uploaded");
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_imagePath, fileName);

            try
            {
                Directory.CreateDirectory(_imagePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                _logger.LogInformation($"Image saved: {fileName}");
                return Ok(fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save image: {ex.Message}");
                return StatusCode(500, $"Failed to save image: {ex.Message}");
            }
        }

        [HttpGet("{fileName}")]
        public IActionResult GetAvatar(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogError($"Image not found: {filePath}");
                return NotFound();
            }
            var mimeType = Path.GetExtension(fileName).ToLower() switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
            _logger.LogInformation($"Serving image: {fileName}");
            return PhysicalFile(filePath, mimeType);
        }
    }
}