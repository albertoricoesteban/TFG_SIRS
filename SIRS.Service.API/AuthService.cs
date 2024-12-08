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
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    return null;
                }

                var usuario = await _context.Usuario
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.Password))
                {
                    var claims = new[]
                    {
                new Claim(ClaimTypes.UserData, usuario.Username),
                new Claim(ClaimTypes.Name, usuario.Nombre),
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

                return null;
            }
            catch (Exception ex)
            {
                // Log o manejar la excepción según corresponda
                return null;
            }
        }

    }

}
