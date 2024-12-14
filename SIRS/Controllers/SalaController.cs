using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;

namespace SIRS.Controllers
{
    [Authorize]  // Solo accesible para usuarios autenticados
    public class SalaController : Controller
    {
        private readonly ApiClientService _apiSalaClientService;

        public SalaController(ApiClientService apiSalaClientService)
        {
            _apiSalaClientService = apiSalaClientService;
        }
        public ActionResult Index()
        {
            ViewBag.UserRole = User.IsInRole("Solicitante") ? "Solicitante" : "Otro";

            return View();
        }

        public ActionResult Add()
        {
            var model = new SalaViewModel();
            return View(model);
        }


        public async Task<IActionResult> GetAll()
        {
            try
            {
                var salas = await _apiSalaClientService.GetAsync<List<SalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetAll");

                return Json(salas);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener las salas: " + ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEdificios()
        {
            // Llama a la API REST y obtiene la lista de edificios.
            var edificios = await _apiSalaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetAll");
            return Json(edificios); // Devuelve la lista en formato JSON.
        }

        [HttpGet]
        public async Task<IActionResult> GetEstadosSala()
        {
            var estadosSala = await _apiSalaClientService.GetAsync<List<EstadoSalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EstadoSalaControlador}GetAllEstados");
            return Json(estadosSala); // Devuelve la lista en formato JSON
        }

        [HttpPost]
        public async Task<IActionResult> Create(SalaViewModel model)
        {
            // Preparar los datos para enviar a la API
            var nuevaSala = new SalaViewModel
            {
                NombreCorto = model.NombreCorto,
                Descripcion = model.Descripcion,
                Capacidad = model.Capacidad,
                EstadoSalaId = model.EstadoSalaId,
                EdificioId = model.EdificioId,
                Reservas = new List<ReservaViewModel>()
            };

            // Enviar los datos a la API

            if (ModelState.IsValid)
            { 
                await _apiSalaClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}Add", nuevaSala);

                TempData["SuccessMessage"] = "La sala se ha creado correctamente en el edificio seleccionado.";
                return RedirectToAction(nameof(Add)); // Redirigir a 'Add' para una nueva inserción
            }
            TempData["ErrorMessage"] = "Ocurrió un error al crear la sala en el edificio.";
            return View("Add", nuevaSala); // Redirigir a la vista 'Add' si hay errores
        }

        // POST: /Edificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(SalaViewModel sala)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sala.Reservas = new List<ReservaViewModel>();
                    var esAdmin = User.IsInRole("Administrador");

                    var urlEdicion = $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}Update/{sala.Id}";
                    await _apiSalaClientService.PutAsync(urlEdicion, sala);

                    TempData["SuccessMessage"] = "El edificio se ha actualizado correctamente.";
                    return RedirectToAction(nameof(Update), new { id = sala.Id }); // Redirigir a 'Update' en caso de excepción
                }
                TempData["ErrorMessage"] = "Ocurrió un error al actualizar el edificio.";
                return View("Update", sala); // Redirigir a la vista 'Update' si hay errores
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar el edificio: " + ex.Message;
                return RedirectToAction(nameof(Update), new { id = sala.Id }); // Redirigir a 'Update' en caso de excepción
            }
        }

        [HttpGet]
        public async Task<IActionResult> TieneReservas(int salaId)
        {
            try
            {
                // Realizamos la llamada al servicio REST para obtener las reservas de la sala por su ID
                var reservas = await _apiSalaClientService.GetAsync<List<ReservaViewModel>>(
                    $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}GetBySalaId/{salaId}"
                );

                // Verificamos si hay alguna reserva asociada a la sala
                bool tieneReservas = reservas != null && reservas.Any();

                // Devolvemos el resultado como JSON (true si tiene reservas, false si no)
                return Json(new { tieneReservas });
            }
            catch (Exception ex)
            {
                // En caso de error, retornar un JSON con un error
                return Json(new { tieneReservas = false, error = ex.Message });
            }
        }

        // Método para borrar una sala
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrar(int id)
        {
            try
            {
                // Llamada al método DELETE del API para eliminar la sala
                await _apiSalaClientService.DeleteAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}Delete/{id}");

                // Mostrar mensaje de éxito
                TempData["SuccessMessage"] = "La sala ha sido eliminada con éxito.";

                // Redirigir a la página principal de gestión de salas (o donde desees mostrar la lista de salas)
                return RedirectToAction(nameof(Index)); // Asegúrate de que 'Index' es el nombre correcto de la acción para la vista principal
            }
            catch (Exception ex)
            {
                // Manejar el error y mostrar un mensaje de error
                TempData["ErrorMessage"] = "Error al eliminar la sala: " + ex.Message;

                // Redirigir de nuevo a la vista principal en caso de error
                return RedirectToAction(nameof(Index)); // Asegúrate de que 'Index' es el nombre correcto de la acción para la vista principal
            }
        }
        // GET: /Edificio/GetByFilter 
        public async Task<IActionResult> GetByFilter(string nombre, int capacidad, int edificioId)
        {
            try
            {
                var query = $"nombreCorto={(nombre ?? string.Empty)}" +
            $"&capacidad={(capacidad > 0 ? capacidad.ToString() : "0")}" +
            $"&edificioId={(edificioId > 0 ? edificioId.ToString() : "0")}";

                var salas = await _apiSalaClientService.GetAsync<List<SalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetSalasByFilter?{query}");
                return Json(salas);
            }
            catch (Exception ex)
            { // Manejo de errores

                TempData["ErrorMessage"] = "Error al buscar el edificio con el filtro indicado: " + ex.Message;
                return RedirectToAction(nameof(Index)); // Redirigir a 'Add' en caso de excepción
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            SalaViewModel sala;

            if (id > 0)
            {
                sala = _apiSalaClientService
                    .GetAsync<SalaViewModel>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetById/{id}")
                    .Result;
                //se cargan los edificios y estados en los viewbag

                var edificios = _apiSalaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetAll").Result;

                if (edificios != null && edificios.Any())
                {
                    // Convertir la respuesta en una lista de SelectListItem para usar en la vista
                    ViewBag.Edificios = edificios.Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),  // El valor será el Id del Edificio
                        Text = e.Descripcion       // El texto será la descripción del Edificio
                    }).ToList();
                }
                else
                {
                    // Si no hay resultados de la API, asignar una lista vacía
                    ViewBag.Edificios = new List<SelectListItem>();
                }

                var estadosSala = _apiSalaClientService.GetAsync<List<EstadoSalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EstadoSalaControlador}GetAllEstados").Result;
                if (estadosSala != null && estadosSala.Any())
                {
                    // Convertir la respuesta en una lista de SelectListItem para usar en la vista
                    ViewBag.EstadosSala = estadosSala.Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),  // El valor será el Id del EstadoSala
                        Text = e.Descripcion       // El texto será la descripción del EstadoSala
                    }).ToList();
                }
                else
                {
                    // Si no hay resultados de la API, asignar una lista vacía
                    ViewBag.EstadosSala = new List<SelectListItem>();
                }
                if (sala == null)
                {
                    return NotFound();
                }
            }
            else
            {
                sala = new SalaViewModel();
            }

            return View(sala);
        }
        [HttpGet]
        public async Task<IActionResult> GetSalasByFilterWithEdificioNombre()
        {
            var salasConEdificio = await _apiSalaClientService.GetAsync<List<SalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetSalasByFilterWithEdificioNombre");
            return Json(salasConEdificio); // Devuelve la lista en formato JSON
        }
    }
}
