using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using SIRS.Domain.Models;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using SIRS.ApliClient;
using System.Security.Claims;
using SIRS.Utilidades;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;

namespace SIRS.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly ApiClientService _apiUsuarioClientService;

        public UsuarioController(ApiClientService apiUsuarioClientService)
        {
            _apiUsuarioClientService = apiUsuarioClientService;
        }

        // Vista para la lista de usuarios (Index)
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var usuarios = await _apiUsuarioClientService.GetAsync<List<UsuarioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.UsuarioControlador}GetAll");

                return Json(usuarios);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener las salas: " + ex.Message;
                return View("Error");
            }
        }
        // Vista para agregar un nuevo usuario
        public async Task<IActionResult> Add()
        {
            var usuario = new UsuarioViewModel();
            return View(usuario);
        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            // Llama a la API REST y obtiene la lista de edificios.
            var roles = await _apiUsuarioClientService.GetAsync<List<RoleViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.RolControlador}GetAllRoles");
            return Json(roles); // Devuelve la lista en formato JSON.
        }
        // Acción para agregar un nuevo usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UsuarioViewModel usuario)
        {
            try
            {
                if (!Utilities.EsNIFNIEValido(usuario.Username))
                {
                    TempData["ErrorMessage"] = "El NIF introducido no es correcto.";
                    return View(nameof(Add), usuario);
                }

                // Verificar si el nombre de usuario ya existe
                var existingUserUserName = await _apiUsuarioClientService.GetAsync<bool>(
            $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}UserExistsByUsername/{usuario.Username}");

                if (existingUserUserName)
                {
                    TempData["ErrorMessage"] = "El nombre de usuario ya está en uso.";
                    return View(nameof(Add), usuario);
                }


                // Comprobamos si el email es el mismo que el email del usuario logado (desde los claims)
                var loggedInUserEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Si el email recibido no es igual al email de los claims, verificamos si ya existe en la base de datos

                var existingUserEmail = await _apiUsuarioClientService.GetAsync<bool>(
                    $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}UserExistsByEmail/{usuario.Email}");

                if (existingUserEmail)
                {
                    TempData["ErrorMessage"] = "El correo electrónico ya está en uso.";
                    return RedirectToAction(nameof(Add), usuario);
                }

                // Ejemplo de datos del usuario
                string nombre = usuario.Nombre;
                string apellido1 = usuario.Apellido1;

                // Transformar el nombre para generar la contraseña base
                string basePassword;

                // Si el nombre tiene menos de 5 caracteres
                if (nombre.Length < 5)
                {
                    // Completar con caracteres del primer apellido
                    string complemento = apellido1.Substring(0, Math.Min(5 - nombre.Length, apellido1.Length));
                    basePassword = nombre + complemento;
                }
                else
                {
                    // Tomar los primeros 5 caracteres del nombre
                    basePassword = nombre.Substring(0, 5);
                }

                // Asegurar que el primer carácter sea mayúscula y el resto minúscula
                basePassword = basePassword[0].ToString().ToUpper() + basePassword.Substring(1).ToLower();

                // Agregar el sufijo y generar el hash
                string passwordFinal = basePassword + "*1234";
                var passwordHashed = BCrypt.Net.BCrypt.HashPassword(passwordFinal);

                // Salida de ejemplo (opcional)
                usuario.Password = passwordHashed;

                if (!string.IsNullOrEmpty(usuario.Password))
                {
                    ModelState.Remove("Password");
                }
                if (ModelState.IsValid)
                {
                    usuario.FechaRegistro = DateTime.Now;
                    // Guardar el nuevo usuario en la base de datos
                    await _apiUsuarioClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}Add", usuario);




                    TempData["SuccessMessage"] = "Usuario agregado con éxito.";
                    return RedirectToAction("Index");



                }

                // Si no es válido, volvemos a mostrar el formulario con los errores
                var roles = await _apiUsuarioClientService.GetAsync<List<RoleViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.RolControlador}GetAllRoles");
                ViewBag.Roles = new SelectList(roles, "Id", "Nombre");
                TempData["ErrorMessage"] = "Error al agregar el usuario. Verifique los datos.";
                return View(usuario);
            }
            catch (Exception ex)
            {
                // Si no es válido, volvemos a mostrar el formulario con los errores
                var roles = await _apiUsuarioClientService.GetAsync<List<RoleViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.RolControlador}GetAllRoles");
                ViewBag.Roles = new SelectList(roles, "Id", "Nombre");
                TempData["ErrorMessage"] = "Error al agregar el usuario. Verifique los datos.";
                return View(usuario);
            }

        }

        // Vista para editar un usuario (modificar)
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _apiUsuarioClientService.GetAsync<UsuarioViewModel>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}GetUserById/{id}");

            if (usuario == null)
            {
                return NotFound();
            }

            var roles = await _apiUsuarioClientService.GetAsync<List<RoleViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.RolControlador}GetAllRoles");
            ViewBag.Roles = new SelectList(roles, "Id", "Nombre");
            return View(usuario);
        }

        // Acción para actualizar un usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioViewModel usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            // Quitamos la validación de la contraseña porque no es necesaria.
            ModelState.Remove("Password");

            // Verificar que el modelo es válido
            if (!ModelState.IsValid)
            {
                // Si no es válido, devolvemos la misma vista con los errores de validación
                return RedirectToAction(nameof(Edit), usuario);
            }

            // Obtener el nombre del usuario actual desde las claims
            var UserNameInUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;

            try
            {
                // 1. Solicitar todos los usuarios que tienen el mismo DNI
                var urlDni = $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.UsuarioControlador}SearchByFilters?username={usuario.Username}";
                var usuariosConMismoDni = await _apiUsuarioClientService.GetAsync<List<UsuarioViewModel>>(urlDni);

                // 2. Solicitar todos los usuarios que tienen el mismo Email
                var urlEmail = $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.UsuarioControlador}SearchByFilters?email={usuario.Email}";
                var usuariosConMismoEmail = await _apiUsuarioClientService.GetAsync<List<UsuarioViewModel>>(urlEmail);

                // Validar si existe algún conflicto con el DNI
                var conflictoDni = usuariosConMismoDni?.Any(u => u.Id != usuario.Id);
                if (conflictoDni == true)
                {
                    TempData["ErrorMessage"] = "El DNI ya está registrado por otro usuario.";
                    return RedirectToAction(nameof(Edit), usuario);
                }

                // Validar si existe algún conflicto con el Email
                var conflictoEmail = usuariosConMismoEmail?.Any(u => u.Id != usuario.Id);
                if (conflictoEmail == true)
                {
                    TempData["ErrorMessage"] = "El Email ya está registrado por otro usuario.";
                    return RedirectToAction(nameof(Edit), usuario);
                }
                // Si no hay conflictos, convertir a UsuarioPerfilViewModel
                var usuarioPerfil = new UsuarioPerfilViewModel
                {
                    Id = usuario.Id,
                    Username = usuario.Username,
                    Nombre = usuario.Nombre,
                    Apellido1 = usuario.Apellido1,
                    Apellido2 = usuario.Apellido2,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    RolId = usuario.RolId
                };
                // Si no hay conflictos, proceder con la actualización del usuario
                var esAdmin = User.IsInRole("Administrador");
                var urlEdicion = $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.AccountControlador}UpdateUsuarioPerfil/{id}/{esAdmin}";

                // Llamar al método del servicio REST API para actualizar el usuario
                var result = await _apiUsuarioClientService.PutAsync<UsuarioPerfilViewModel>(urlEdicion, usuarioPerfil);

                // Verificamos si la actualización fue exitosa
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Datos actualizados correctamente.";
                    return RedirectToAction(nameof(Edit), usuario);
                }
                else
                {
                    TempData["ErrorMessage"] = "Hubo un error al actualizar los datos.";
                    return RedirectToAction(nameof(Edit), usuario);
                }
            }
            catch (Exception ex)
            {
                // Si hay un error inesperado en la llamada al API
                TempData["ErrorMessage"] = $"Hubo un error al actualizar los datos del usuario: {ex.Message}";
                return RedirectToAction(nameof(Edit), usuario);
            }
        }

        // Acción para eliminar un usuario (dar de baja)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (loggedInUserId == id)
                {
                    TempData["ErrorMessage"] = "No puede dar de baja su propio usuario";
                }
                else
                {
                    // Cancelar la reserva
                    await _apiUsuarioClientService.DeleteAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.UsuarioControlador}Delete/{id}");
                    TempData["SuccessMessage"] = "Usuario dado de baja con éxito.";
                }
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = "Error al dar de baja el usuario.";

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetUsuariosByFilters(string username, string nombre, string apellido1, string apellido2)
        {
            try
            {
                var query = $"username={(username ?? string.Empty)}" +
            $"&nombre={(nombre ?? string.Empty)}" +
            $"&apellido1={(apellido1 ?? string.Empty)}" +
            $"&apellido2={(apellido2 ?? string.Empty)}";

                var usuarios = await _apiUsuarioClientService.GetAsync<List<UsuarioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.UsuarioControlador}GetUsuariosByFilter?{query}");
                return Json(usuarios);
            }
            catch (Exception ex)
            { // Manejo de errores

                TempData["ErrorMessage"] = "Error al buscar los usuarios: " + ex.Message;
                return RedirectToAction(nameof(Index)); // Redirigir a 'Add' en caso de excepción
            }
        }
    }

}
