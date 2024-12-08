using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using SIRS.Models;
using SIRS.Utilidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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


        public async Task<IActionResult> MiPerfil()
        {
            // Obtener el ID del usuario logueado. Suponiendo que el ID está en el nombre del usuario.
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Llamar al API para obtener el usuario mediante su ID (o correo, dependiendo del sistema).
            var existingUserEmail = await _apiClientService.GetAsync<UsuarioPerfilViewModel>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}GetUserById/{loggedInUserId}");
            return View(existingUserEmail);
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
            var result = await _apiClientService.PostAsync<string>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AuthControlador}Login", login);

            // Verificamos que la respuesta del API sea correcta
            if (string.IsNullOrEmpty(result))
            {
                TempData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
                return View("Login", login);
            }

            // Almacenar el token en la sesión
            HttpContext.Session.SetString("JwtToken", result);

            // Leer y deserializar el token usando JwtSecurityTokenHandler
            var jwtToken = Utilidades.JwtDecoder.DecodeJwtToken(result);

            // Extraer los claims y almacenarlos en la sesión
            foreach (var claim in jwtToken.Claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.UserData:
                        HttpContext.Session.SetString("Username", claim.Value);
                        break;
                    case ClaimTypes.Name:
                        HttpContext.Session.SetString("Nombre", claim.Value);
                        break;
                    case ClaimTypes.Surname:
                        // Si los apellidos vienen separados por guión, los dividimos
                        var apellidos = claim.Value.Split('-');
                        if (apellidos.Length == 2)
                        {
                            HttpContext.Session.SetString("Apellido1", apellidos[0]);
                            HttpContext.Session.SetString("Apellido2", apellidos[1]);
                        }
                        else
                        {
                            // Si no hay dos apellidos, manejamos el caso
                            HttpContext.Session.SetString("Apellido1", claim.Value);
                            HttpContext.Session.SetString("Apellido2", string.Empty);
                        }
                        break;
                    case ClaimTypes.Email:
                        HttpContext.Session.SetString("Email", claim.Value);
                        break;
                    case ClaimTypes.Role:
                        HttpContext.Session.SetString("Rol", claim.Value);
                        break;
                    case ClaimTypes.NameIdentifier:
                        HttpContext.Session.SetString("UserId", claim.Value);
                        break;
                    default:
                        // Manejar otros claims si es necesario
                        break;
                }

            }
            // Crear un ClaimsIdentity con los claims

            var identity = new ClaimsIdentity(jwtToken.Claims, "Jwt");
            // Crear un ClaimsPrincipal
            var principal = new ClaimsPrincipal(identity);

            // Autenticar al usuario
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Verificar si el usuario está autenticado después de almacenar el token
            if (User.Identity.IsAuthenticated)
            {
                // Si el usuario está autenticado, redirigimos a la página principal
                return RedirectToAction("Index", "Home");
            }

            // Si no está autenticado, mostramos el login nuevamente con un mensaje de error.
            TempData["ErrorMessage"] = "No se pudo autenticar el usuario.";
            return View("Login", login);
        }


        // GET: Account/Details/5
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(UsuarioPerfilViewModel model)
        {
            // Verificar que el modelo es válido
            if (!ModelState.IsValid)
            {
                // Si no es válido, devolvemos la misma vista con los errores de validación
                return RedirectToAction(nameof(MiPerfil), model);
            }
            var UserNameInUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
            if (model.Username != UserNameInUser)
            {
                // Verificar si el nombre de usuario ya existe
                var existingUserUserName = await _apiClientService.GetAsync<bool>(
                $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}UserExistsByUsername/{model.Username}");

                if (existingUserUserName)
                {
                    ModelState.AddModelError("Username", "El nombre de usuario ya está en uso.");
                    return RedirectToAction(nameof(MiPerfil), model);
                }
            }

            // Comprobamos si el email es el mismo que el email del usuario logado (desde los claims)
            var loggedInUserEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Si el email recibido no es igual al email de los claims, verificamos si ya existe en la base de datos
            if (model.Email != loggedInUserEmail)
            {
                var existingUserEmail = await _apiClientService.GetAsync<bool>(
                    $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}UserExistsByEmail/{model.Email}");

                if (existingUserEmail)
                {
                    ModelState.AddModelError("Email", "El correo electrónico ya está en uso.");
                    return RedirectToAction(nameof(MiPerfil), model);
                }
            }

            try
            {
                // Llamar al método del servicio REST API para actualizar el usuario
                var result = await _apiClientService.PutAsync<UsuarioPerfilViewModel>(
                    $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}UpdateUsuarioPerfil/{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value}", model);

                // Verificamos si la actualización fue exitosa
                if (result != null)
                {

                    TempData["SuccessMessage"] = "Datos actualizados correctamente.";
                    return RedirectToAction(nameof(MiPerfil), model);
                }
                else
                {

                    TempData["ErrorMessage"] = "Hubo un error al actualizar los datos.";
                    return RedirectToAction(nameof(MiPerfil), model);
                }
            }
            catch (Exception ex)
            {
                // Si hay un error inesperado en la llamada al API

                TempData["ErrorMessage"] = $"Hubo un error al actualizar los datos del usuario: {ex.Message}";
                return RedirectToAction(nameof(MiPerfil), model);
            }

            // Volver a la misma vista con los mensajes de éxito o error y el modelo actualizado
            return RedirectToAction(nameof(MiPerfil)); // Redirigir a 'Login' 

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
            var existingUserEmail = await _apiClientService.GetAsync<bool>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}UserExistsByEmail/{usuario.Email}");
            var existingUserUserName = await _apiClientService.GetAsync<bool>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}UserExistsByUsername/{usuario.Username}");
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
            await _apiClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}Add", usuario);

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
                var roles = await _apiClientService.GetAsync<List<RoleViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.RolControlador}GetAllRoles");

                return Json(roles);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener los roles: " + ex.Message;
                return View("Error");
            }
        }
        public IActionResult CerrarSesion()
        {
            // Limpiar toda la información de la sesión
            HttpContext.Session.Clear();

            // Opcionalmente, cerrar la autenticación (si usas cookies)
            HttpContext.SignOutAsync();

            // Redirigir al usuario a la página de inicio de sesión
            return RedirectToAction("Login", "Account");
        }

        public IActionResult UnAuthorized()
        {
            return View();
        }
    }
}