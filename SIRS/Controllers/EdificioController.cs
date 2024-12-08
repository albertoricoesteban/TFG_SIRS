using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIRS.Controllers
{
    [Authorize]  // Solo accesible para usuarios autenticados
    public class EdificioController : Controller
    {
        private readonly ApiClientService _apiClientService;

        public EdificioController(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            var model = new EdificioViewModel();
            return View(model);
        }

        // POST: /Edificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EdificioViewModel edificio)
        {
            try
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
        }

        // POST: /Edificio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Actualizar(EdificioViewModel edificio)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    edificio.Salas = new List<SalaViewModel>();
                    await _apiClientService.PutAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}Update/{edificio.Id}", edificio);

                    TempData["SuccessMessage"] = "El edificio se ha actualizado correctamente.";
                    return RedirectToAction(nameof(Update), new { id = edificio.Id }); // Redirigir a 'Update' en caso de excepción
                }
                TempData["ErrorMessage"] = "Ocurrió un error al actualizar el edificio.";
                return View("Update", edificio); // Redirigir a la vista 'Update' si hay errores
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar el edificio: " + ex.Message;
                return RedirectToAction(nameof(Update), new { id = edificio.Id }); // Redirigir a 'Update' en caso de excepción
            }
        }

        // GET: /Edificio/GetAll
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var edificios = await _apiClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetAll");

                return Json(edificios);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener los edificios: " + ex.Message;
                return View("Error");
            }
        }

        // GET: /Edificio/GetByFilter 
        public async Task<IActionResult> GetByFilter(string nombre, string direccion)
        {
            try
            {
                var query = $"nombre={nombre ?? string.Empty}&direccion={direccion ?? string.Empty}";
                var edificios = await _apiClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetEdificiosByFilter?{query}");
                return Json(edificios);
            }
            catch (Exception ex)
            { // Manejo de errores

                TempData["ErrorMessage"] = "Error al buscar el edificio con el filtro indicado: " + ex.Message;
                return RedirectToAction(nameof(Index)); // Redirigir a 'Add' en caso de excepción
            }
        }

        // Método para mostrar la vista 'Add' con el parámetro 'id'
        [HttpGet]
        [Route("Edificio/Update/{id}")] // Ruta para el método con parámetro 'id'
        public ActionResult Update(int id)
        {
            EdificioViewModel edificio;

            if (id > 0)
            {
                edificio = _apiClientService
                    .GetAsync<EdificioViewModel>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetById/{id}")
                    .Result;

                if (edificio == null)
                {
                    return NotFound();
                }
            }
            else
            {
                edificio = new EdificioViewModel();
            }

            return View(edificio);
        }

        // Método para borrar un edificio
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrar(int id)
        {
            try
            {
                // Llamada al método DELETE del API
                await _apiClientService.DeleteAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}Delete/{id}");

                // Mostrar mensaje de éxito
                TempData["SuccessMessage"] = "El edificio ha sido eliminado con éxito.";
                return RedirectToAction(nameof(Index)); // Redirigir a la página principal o donde corresponda
            }
            catch (Exception ex)
            {
                // Manejar el error y mostrar un mensaje de error
                TempData["ErrorMessage"] = "Error al eliminar el edificio: " + ex.Message;
                return RedirectToAction(nameof(Index)); // Redirigir a la página principal o donde corresponda
            }
        }

    }
}
