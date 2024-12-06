using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using SIRS.Models;

namespace SIRS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiClientService _apiClientService;

        public AccountController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }


        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        // GET: Account/Details/5
        public ActionResult Register(int id)
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel usuario)
        {
            // Validar que los campos requeridos no sean nulos
            if (usuario == null || string.IsNullOrEmpty(usuario.Username) || string.IsNullOrEmpty(usuario.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // Verificar si el usuario ya existe 
            var existingUserEmail = await _apiClientService.GetAsync<bool>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}UserExistsByEmail?email={usuario.Email}");
            var existingUserUserName = await _apiClientService.GetAsync<bool>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}UserExistsByUsername?username={usuario.Username}");
            if (existingUserEmail || existingUserUserName)
            {
                return BadRequest("El usuario ya existe");
            }

            // Hash de la contraseña usando bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

            // Crear el nuevo usuario
            var user = new Usuario
            {
                Username = usuario.Username,
                Password = hashedPassword,
                Nombre = usuario.Nombre,
                Apellido1 = usuario.Apellido1,
                Apellido2 = usuario.Apellido2,
                Email = usuario.Email,
                FechaRegistro = DateTime.UtcNow,
                RolId = 1 // Asumimos rol 1 para un usuario normal
            };

            // Guardar el nuevo usuario en la base de datos
             await _apiClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControloador}Add", user);

            return Ok(new { Message = "Usuario registrado correctamente" });

            /***
             *   try
            {
                if (ModelState.IsValid)
                {
                    edificio.Salas = new List<SalaViewModel>();
                    await _apiClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}Add", edificio);

                    TempData["SuccessMessage"] = "El edificio se ha creado correctamente.";
                    return RedirectToAction(nameof(Add)); // Redirigir a 'Add' para una nueva inserción
                }
                TempData["ErrorMessage"] = "Ocurrió un error al crear el edificio.";
                return View("Add", edificio); // Redirigir a la vista 'Add' si hay errores
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al crear el edificio: " + ex.Message;
                return RedirectToAction(nameof(Add)); // Redirigir a 'Add' en caso de excepción
            }
             ***/
        }
    }
}