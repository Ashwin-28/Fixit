using Fixit_API.Models.Enums;

namespace Fixit_API.Dtos
{
    public class RegisterDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Tenant;
    }
}