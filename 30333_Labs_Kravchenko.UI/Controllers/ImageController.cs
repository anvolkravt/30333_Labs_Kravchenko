using _30333_Labs_Kravchenko.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace _30333_Labs_Kravchenko.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ImageController(
            ILogger<ImageController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogError("No file uploaded");
                return BadRequest("No file uploaded");
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                _logger.LogError("User email claim not found");
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"User not found: {email}");
                return NotFound("User not found");
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                user.Avatar = memoryStream.ToArray();
                user.MimeType = file.ContentType;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation($"Avatar updated for user: {email}");
                return Ok("Avatar uploaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to upload avatar: {ex.Message}");
                return StatusCode(500, "Error saving avatar");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAvatar()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                _logger.LogError("User email claim not found");
                return Unauthorized();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError($"User not found: {email}");
                return NotFound();
            }

            if (user.Avatar != null && !string.IsNullOrEmpty(user.MimeType))
            {
                _logger.LogInformation($"Serving avatar for {email}");
                return File(user.Avatar, user.MimeType);
            }

            // fallback: serve default avatar image from wwwroot
            var fallbackPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "user.png");
            if (System.IO.File.Exists(fallbackPath))
            {
                var fallbackBytes = await System.IO.File.ReadAllBytesAsync(fallbackPath);
                return File(fallbackBytes, "image/png");
            }

            _logger.LogWarning("No avatar and no fallback found");
            return NotFound("No avatar found");
        }
    }
}
