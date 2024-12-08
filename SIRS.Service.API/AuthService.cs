using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIRS.Data.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIRS.Service.API
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var usuario = await _context.Usuario
                .Include(u => u.Rol)  // Incluir el rol
                .FirstOrDefaultAsync(u => u.Username == username);
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, usuario.Password);
            if (usuario != null && isPasswordValid) // Aquí debes usar un hash seguro de la contraseña
            {
                // Crear el token JWT
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, usuario.Username),
                 new Claim(ClaimTypes.Surname, usuario.Apellido1 + "-" + usuario.Apellido2),
                 new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return null; // Si las credenciales son incorrectas
        }
    }

}
