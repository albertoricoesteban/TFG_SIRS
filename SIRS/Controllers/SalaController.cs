using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIRS.Controllers
{
    public class SalaController : Controller
    {
        private readonly ApiClientService _apiSalaClientService;

        public SalaController(ApiClientService apiSalaClientService)
        {
            _apiSalaClientService = apiSalaClientService;
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


        public async Task<IActionResult> GetAll()
        {
            var salas = _apiSalaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetAll");
            return Json(salas);
        }

        // GET: /Sala/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                // Llamada al servicio para obtener la lista de edificios
                var edificios = await _apiSalaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetAll");

                // Pasar la lista de edificios a la vista
                ViewBag.Edificios = edificios.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Descripcion
                }).ToList();

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener los edificios: " + ex.Message;
                return View("Error");
            }
        }

        // POST: /Sala/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalaViewModel sala)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Aquí iría el código para guardar la sala, por ejemplo llamando a un servicio para guardar la sala
                     //await _apiClientService.PostAsync(...);

                    TempData["SuccessMessage"] = "La sala ha sido creada correctamente.";
                    return RedirectToAction(nameof(Create)); // Redirigir a la vista 'Create' para una nueva inserción
                }

                // Si hay errores en el modelo, devolver la vista con los datos actuales
                TempData["ErrorMessage"] = "Ocurrió un error al crear la sala.";
                return View(sala);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al crear la sala: " + ex.Message;
                return View(sala); // Redirigir a la vista 'Create' en caso de excepción
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEdificios()
        {
            // Llama a la API REST y obtiene la lista de edificios.
            var edificios = await _apiSalaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetAll");
            return Json(edificios); // Devuelve la lista en formato JSON.
        }
    }
}
