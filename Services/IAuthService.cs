using JwtAuthDotNet9_2025.Entities;
using JwtAuthDotNet9_2025.Models;

namespace JwtAuthDotNet9_2025.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<User?> ChangeUserRoleAsync(Guid userId, string newRole);
    }
}
