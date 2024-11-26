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
            var model = new SalaViewModel();
            return View(model);
        }


        public async Task<IActionResult> GetAll()
        {
            var salas = _apiSalaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetAll");
            return Json(salas);
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
            // Validar que el modelo está completo
            //    if (!ModelState.IsValid)
            //    {
            //        // Si el modelo no es válido, cargar datos necesarios y devolver la vista
            //        var salas = await _apiSalaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}Add");
            //        ViewBag.Edificios = salas.Select(e => new SelectListItem
            //        {
            //            Value = e.Id.ToString(),
            //            Text = e.Descripcion
            //        }).ToList();

            //        ViewBag.Estados = new List<SelectListItem>
            //{
            //    new SelectListItem { Value = "Disponible", Text = "Disponible" },
            //    new SelectListItem { Value = "No Disponible", Text = "No Disponible" },
            //    new SelectListItem { Value = "Mantenimiento", Text = "En Mantenimiento" }
            //};

            //        return View(model); // Retornar el formulario con los errores
            //    }

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
    }
}
