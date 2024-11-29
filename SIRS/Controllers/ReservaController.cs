﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;

namespace SIRS.Controllers
{
    public class ReservaController : Controller
    {
        private readonly ApiClientService _apiReservaClientService;

        public ReservaController(ApiClientService apiReservaClientService)
        {
            _apiReservaClientService = apiReservaClientService;
        }
        // GET: ReservaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReservaController/Details/5
        public ActionResult Calendar(int id)
        {
            return View();
        }

        // GET: ReservaController/Create
        public ActionResult Add()
        {

            var edificios = _apiReservaClientService.GetAsync<List<EdificioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.EdificioControlador}GetAll").Result;

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

            return View();
        }

        // GET: ReservaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReservaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
         
        [HttpGet]
        public IActionResult GetSalasByEdificio(int edificioId)
        {
            var edificios = _apiReservaClientService.GetAsync<List<SalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetByEdificioId/{edificioId}").Result;
            return Json(edificios); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReservaViewModel reserva)
        {
            

            // Enviar los datos a la API

            if (ModelState.IsValid)
            {
                await _apiReservaClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}Add", reserva);

                TempData["SuccessMessage"] = "La reserva se ha creado correctamente.";
                return RedirectToAction(nameof(Add)); // Redirigir a 'Add' para una nueva inserción
            }
            TempData["ErrorMessage"] = "Ocurrió un error al crear la reserva.";
            return View("Add", reserva); // Redirigir a la vista 'Add' si hay errores
        }
    }
}
