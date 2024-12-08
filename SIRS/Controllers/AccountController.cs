using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using SIRS.Models;
using System.Text.RegularExpressions;

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
        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Loguear(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, redirigimos al login con el error.
                return View("Login", login);
            }
            // Realizar la petición al REST API para validar las credenciales del usuario
            var result = await _apiClientService.PostAsync<LoginResponse>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AuthControloador}Login",login);

            

            // Verificamos que la respuesta del API sea correcta
            if (result == null || string.IsNullOrEmpty(result.Token))
            {
                TempData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
                return View("Login", login);
            }

            // Almacenamos el JWT en la sesión
            HttpContext.Session.SetString("JwtToken", result.Token);

            // Almacenamos los datos del usuario y rol en la sesión para poder usarlos en toda la aplicación
            HttpContext.Session.SetString("Username", result.Username);
            HttpContext.Session.SetString("Nombre", result.Nombre);
            HttpContext.Session.SetString("Apellido1", result.Apellido1);
            HttpContext.Session.SetString("Apellido2", result.Apellido2);
            HttpContext.Session.SetInt32("RolId", result.RolId);

            // Redirigimos a la página principal después de un login exitoso
            return RedirectToAction("Index", "Home");
        }
        // GET: Account/Details/5
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar(UsuarioViewModel usuario)
        {
            // Validar que los campos requeridos no sean nulos
            if (!ModelState.IsValid)
            {
                // TempData["ErrorMessage"] = "Revise que todos los campos estén rellenos y que la contraseña tenga entre 6 y 10 caracteres, con al menos una mayúscula, una minúscula, un número y un caracter especial";
                return View("Register", usuario); // Redirigir a la vista 'Register' si hay errores
            }

            // Verificar si el usuario ya existe 
            var existingUserEmail = await _apiClientService.GetAsync<bool>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControloador}UserExistsByEmail/{usuario.Email}");
            var existingUserUserName = await _apiClientService.GetAsync<bool>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControloador}UserExistsByUsername/{usuario.Username}");
            if (existingUserEmail || existingUserUserName)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al crear el usuario, contacte con el administrador";
                return View("Register", usuario); // Redirigir a la vista 'Register' si hay errores
            }
            string errorMessage;
            bool isValid = ValidarContraseña(usuario.Password, out errorMessage);

            if (isValid)
            {
                var passwordHased = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                usuario.Password = passwordHased;
            }
            else
            {
                Console.WriteLine("Error: " + errorMessage);
            }

            usuario.RolId = 2;
            usuario.FechaRegistro = DateTime.Now;
            // Guardar el nuevo usuario en la base de datos
            await _apiClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControloador}Add", usuario);

            TempData["SuccessMessage"] = "Usuario registrado correctamente";
            return RedirectToAction(nameof(Login)); // Redirigir a 'Login' 

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
        private static bool ValidarContraseña(string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Verificar longitud de la contraseña
            if (password.Length < 6 || password.Length > 10)
            {
                errorMessage = "La contraseña debe tener al menos 6 caracteres y un máximo de 10.";
                return false;
            }

            // Verificar expresión regular para la contraseña
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$");
            if (!regex.IsMatch(password))
            {
                errorMessage = "La contraseña debe incluir una mayúscula, una minúscula, un número y un carácter especial.";
                return false;
            }

            return true;
        }
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _apiClientService.GetAsync<List<RoleViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.RolControloador}GetAllRoles");

                return Json(roles);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener los roles: " + ex.Message;
                return View("Error");
            }
        }
    }
}