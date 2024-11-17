using Microsoft.AspNetCore.Mvc;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIRS.Controllers
{
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
                    await _apiClientService.PostAsync("http://localhost:5237/api/Edificio/Add", edificio);

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

        // GET: /Edificio/GetAll
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var edificios = await _apiClientService.GetAsync<List<EdificioViewModel>>("edificio/GetAll");
                return View(edificios);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al obtener los edificios: " + ex.Message;
                return View("Error");
            }
        }

        // Otros métodos del controlador...
    }
}
