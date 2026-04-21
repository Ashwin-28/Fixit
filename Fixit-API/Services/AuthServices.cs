using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fixit_API.Data;
using Fixit_API.Dtos;
using Fixit_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Fixit_API.Services
{
    public class AuthServices
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public AuthServices(UserManager<User> userManager, IConfiguration configuration, AppDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<(bool Success, string? Token, string? Error)> Register(RegisterDTO registerDTO)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (existingUser != null)
            {
                return (false, null, "User with this email already exists.");
            }

            var user = new User
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FullName = registerDTO.FullName,
                Address = registerDTO.Address,
                Role = registerDTO.Role
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                var error = string.Join(" ", result.Errors.Select(e => e.Description));
                return (false, null, error);
            }

            var token = GenerateToken(user);
            return (true, token, null);
        }

        public async Task<(bool Success, string? Token, string? Error)> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return (false, null, "Invalid email or password.");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!passwordValid)
            {
                return (false, null, "Invalid email or password.");
            }

            var token = GenerateToken(user);
            return (true, token, null);
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out var minutes) ? minutes : 60;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Role, user.Role.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
