using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                    await _apiSalaClientService.PutAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}Update/{sala.Id}", sala);

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
        // Método para mostrar la vista 'Add' con el parámetro 'id'
        [HttpGet]
        [Route("Sala/Update/{id}")] // Ruta para el método con parámetro 'id'
        public ActionResult Update(int id)
        {
            SalaViewModel sala;

            if (id > 0)
            {
                sala = _apiSalaClientService
                    .GetAsync<SalaViewModel>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetById/{id}")
                    .Result;
                //se cargan los edificios y estados en los viewbag

                ViewBag.Edificios = _apiSalaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetAll");
                ViewBag.EstadosSala = _apiSalaClientService.GetAsync<List<EstadoSalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EstadoSalaControlador}GetAllEstados");

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
    }
}
