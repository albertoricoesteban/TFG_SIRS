using Microsoft.AspNetCore.Mvc;
using SIRS.Application.ViewModels;

namespace SIRS.Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var token = await _authService.LoginAsync(login.Username, login.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            return Ok(new { Token = token });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
