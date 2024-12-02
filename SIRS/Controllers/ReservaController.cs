using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using SIRS.Service.API.DTO;

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
            //todo quitar este usuario para coger el que está logado
            reserva.UsuarioId = 2;
            if (ModelState.IsValid)
            {
                await _apiReservaClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}Add", reserva);

                TempData["SuccessMessage"] = "La reserva se ha creado correctamente.";
                return RedirectToAction(nameof(Add)); // Redirigir a 'Add' para una nueva inserción
            }
            TempData["ErrorMessage"] = "Ocurrió un error al crear la reserva.";
            return View("Add", reserva); // Redirigir a la vista 'Add' si hay errores
        }


        public async Task<IActionResult> GetReservasByFilters(int edificioId, int salaId, DateTime? fechaReserva, TimeSpan? horaInicio)
        {
            try
            {
                var query = new List<string>();

                if (edificioId > 0)
                    query.Add($"edificioId={edificioId}");

                if (salaId > 0)
                    query.Add($"salaId={salaId}");

                if (fechaReserva.HasValue)
                    query.Add($"fechaReserva={fechaReserva.Value.ToString("yyyy-MM-dd")}");

                if (horaInicio.HasValue)
                    query.Add($"horaInicio={horaInicio.Value.ToString(@"hh\:mm\:ss")}");

                var queryString = string.Join("&", query);


                var reservas = await _apiReservaClientService.GetAsync<List<ReservaDTO>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}GetReservasByFilters?{query}");
                return Json(reservas);
            }
            catch (Exception ex)
            { // Manejo de errores

                TempData["ErrorMessage"] = "Error al buscar el edificio con el filtro indicado: " + ex.Message;
                return RedirectToAction(nameof(Index)); // Redirigir a 'Add' en caso de excepción
            }
        }
    }
}
