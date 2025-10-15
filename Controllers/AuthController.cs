using JwtAuthDotNet9_2025.Entities;
using JwtAuthDotNet9_2025.Models;
using JwtAuthDotNet9_2025.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthDotNet9_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService): ControllerBase
    {
     //   public static User user = new();

        [HttpPost("register")]
        public async Task<ActionResult<User>>Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);

            if (user == null)
                return BadRequest("Nome de Usuario existente.");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result == null)
                return BadRequest("Nome do usuario ou palavra passe errada");

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefrshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);
            if (result is null || result.AcessToken is null || result.RefreshToken is null)
                return Unauthorized("Token invalido");

            return Ok(result);
        }

        //Securing endpoints
        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("autenticação com sucesso!");
        }

        //roles
        [Authorize(Roles = "Admin, User")]
        [HttpGet("perfil")]
        public IActionResult PerfilDoUtilizadorEndpoint()
        {
            if (User.IsInRole("Admin"))
                return Ok("Autenticado como Admin.");

            if (User.IsInRole("User"))
                return Ok("Autenticado como User.");

            return Forbid(); // nunca deverá ocorrer, mas é uma salvaguarda
        }

        //alterar role do usuario
        [Authorize(Roles = "Admin")]
        [HttpPut("alterar-role/{userId}/role")]
        public async Task<IActionResult> ChangeUserRole(Guid userId, [FromBody] UserDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Role) || 
                (request.Role != "Admin" && request.Role != "User"))
                return BadRequest("Role deve ser 'Admin' ou 'User'.");

            var updatedUser = await authService.ChangeUserRoleAsync(userId, request.Role);
            if (updatedUser == null)
                return NotFound("Utilizador nao encontrado.");

            return Ok(new
            {
                message = $"Role actualizado com sucesso para {updatedUser.Role}",
                user = new {updatedUser.Id, updatedUser.Username, updatedUser.Role}
            });
        }
    }
}
