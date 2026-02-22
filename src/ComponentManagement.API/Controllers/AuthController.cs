using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using ComponentManagement.Application.Users.Commands;
using ComponentManagement.Application.Users.Queries;
using System.Security.Claims;


namespace ComponentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ======================
        // 🔹 REGISTER
        // ======================
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ======================
        // 🔹 LOGIN -> simpan access + refresh token ke cookies
        // ======================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            var accessCookieName = $"access_token_{result.Role}";
            var refreshCookieName = $"refresh_token_{result.Role}";

            // ✅ Simpan ACCESS TOKEN
            Response.Cookies.Append(accessCookieName, result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15),
                Path = "/"
            });

            // ✅ Simpan REFRESH TOKEN (RAW)
            Response.Cookies.Append(refreshCookieName, result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/"
            });

            return Ok(new
            {
                message = "Login success",
                result.Username,
                result.Role
            });
        }


        // ======================
        // 🔹 REFRESH TOKEN ROTATION
        // ======================
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromQuery] string role)
        {
            var refreshCookieName = $"refresh_token_{role}";
            if (!Request.Cookies.TryGetValue(refreshCookieName, out var refreshToken))
                
                return Unauthorized("Refresh token cookie missing");

            var result = await _mediator.Send(new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            });

            // ✅ Update access token & refresh token baru di cookie
            var accessCookieName = $"access_token_{result.Role}";
            Response.Cookies.Append(accessCookieName, result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            Response.Cookies.Append(refreshCookieName, result.NewRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { message = "Token refreshed" });
        }



        // ======================
        // 🔹 LOGOUT -> hapus cookies
        // ======================
        [HttpPost("logout")]
        public IActionResult Logout([FromQuery] string role)
        {
            Response.Cookies.Delete($"access_token_{role}");
            Response.Cookies.Delete($"refresh_token_{role}");
            return Ok(new { message = $"Logout successful for role {role}" });
        }


        // ======================
        // 🔹 GET CURRENT USER (ambil dari JWT)
        // ======================
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var username = User.Identity?.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { message = "User not authenticated" });

            return Ok(new
            {
                username,
                role
            });
        }

        // ======================
        // 🔹 GET ALL USERS (khusus PE_Plant_Engineer)
        // ======================
        [Authorize(Roles = "PE_Plant_Engineer")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllUserQuery
            {
                Username = name,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // ======================
        // 🔹 DELETE USER
        // ======================
        [Authorize(Roles = "PE_Plant_Engineer")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand { UserId = id };
            var result = await _mediator.Send(command);
            if (!result)
                return NotFound(new { message = "User not found" });

            return NoContent();
        }
    }
}
