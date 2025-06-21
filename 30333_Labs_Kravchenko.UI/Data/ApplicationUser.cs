using Microsoft.AspNetCore.Identity;

namespace _30333_Labs_Kravchenko.UI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? Avatar { get; set; }
        public string MimeType { get; set; } = string.Empty;
    }
}
