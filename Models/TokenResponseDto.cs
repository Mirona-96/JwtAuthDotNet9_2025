using System.Runtime.CompilerServices;

namespace JwtAuthDotNet9_2025.Models
{
    public class TokenResponseDto
    {
        public required string AcessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
