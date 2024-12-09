using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using SIRS.Service.API.DTO;
using System.Security.Claims;

namespace SIRS.Controllers
{
    [Authorize]  // Solo accesible para usuarios autenticados
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
        [HttpGet]
        [Route("Reserva/Update/{id}")] // Ruta para el método con parámetro 'id'
        public ActionResult Update(int id)
        {
            ReservaDTO reserva;

            if (id > 0)
            {
                reserva = _apiReservaClientService
                    .GetAsync<ReservaDTO>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}GetById/{id}")
                    .Result;
                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
                if (reserva == null || (reserva.UsuarioId != int.Parse(loggedInUserId.ToString())))
                {
                    TempData["ErrorMessage"] = "No puede editar una reserva que no es suya o que no coincide con la reserva solicitada.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                reserva = new ReservaDTO();
            }
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
            var salas = _apiReservaClientService.GetAsync<List<SalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetAll").Result;

            if (salas != null && salas.Any())
            {
                // Convertir la respuesta en una lista de SelectListItem para usar en la vista
                ViewBag.Salas = salas.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),  // El valor será el Id del Edificio
                    Text = e.NombreCorto       // El texto será la descripción del Edificio
                }).ToList();
            }
            else
            {
                // Si no hay resultados de la API, asignar una lista vacía
                ViewBag.Salas = new List<SelectListItem>();
            }
            return View(reserva);
        }
        [HttpPost]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            try
            {
                // Obtener la reserva para verificar el UsuarioId
                var reserva = await _apiReservaClientService.GetAsync<ReservaDTO>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}GetById/{id}");

                if (reserva == null)
                {
                    return Json(new { success = false, message = "La reserva no existe." });
                }

                // Verificar si el usuario logueado es el propietario de la reserva
                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (User.IsInRole("Solicitante") && reserva.UsuarioId != int.Parse(loggedInUserId.ToString()))
                {
                    return Json(new { success = false, message = "No puede cancelar una reserva que no es suya." });
                }

                // Cancelar la reserva
                await _apiReservaClientService.PostAsyncWithId($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}CancelarReserva", id);

                return Json(new { success = true, message = "Reserva cancelada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hubo un error al cancelar la reserva: {ex.Message}" });
            }
        }

        // POST: ReservaController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(ReservaDTO reserva)
        {
            try
            {
                var reservaBd = _apiReservaClientService
      .GetAsync<ReservaDTO>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}GetById/{reserva.Id}")
      .Result;

                // Verificar si el usuario logueado es el propietario de la reserva, solo para solicitantes
                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (User.IsInRole("Solicitante") && (reservaBd.UsuarioId != int.Parse(loggedInUserId.ToString()) || reservaBd.Id != reserva.Id))
                {
                    TempData["ErrorMessage"] = "No puede editar una reserva que no es suya o que no coincide con la reserva solicitada.";
                    return RedirectToAction(nameof(Update), new { id = reserva.Id });
                }

                // Combinamos la fecha de reserva y la hora de inicio
                DateTime fechaHoraReserva = reserva.FechaReserva.Date.Add(reserva.HoraInicio);

                // Comprobamos si la fecha y hora de la reserva es menor que la fecha y hora actuales
                if (fechaHoraReserva < DateTime.Now)
                {
                    TempData["ErrorMessage"] = "La fecha y hora de la reserva no puede ser anterior al momento actual.";
                    return RedirectToAction(nameof(Update), new { id = reserva.Id });
                }

                if (ModelState.IsValid)
                {
                    ReservaViewModel reser = new ReservaViewModel()
                    {
                        Id = reserva.Id,
                        Aprobada = reserva.Aprobada,
                        SalaId = reserva.SalaId,
                        Nombre = reserva.Nombre,
                        Observaciones = reserva.Observaciones,
                        FechaReserva = reserva.FechaReserva,
                        HoraInicio = reserva.HoraInicio,
                        TiempoTotal = reserva.TiempoTotal,
                        UsuarioId = int.Parse(loggedInUserId.ToString())  // Usamos el Usuario logueado para la actualización
                    };
                    await _apiReservaClientService.PutAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}Update/{reserva.Id}", reser);

                    TempData["SuccessMessage"] = "La reserva se ha actualizado correctamente.";
                    return RedirectToAction(nameof(Update), new { id = reserva.Id });
                }

                TempData["ErrorMessage"] = "Ocurrió un error al actualizar la reserva.";
                return RedirectToAction(nameof(Update), new { id = reserva.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar la reserva: " + ex.Message;
                return RedirectToAction(nameof(Update), new { id = reserva.Id });
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
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reserva.UsuarioId = int.Parse(loggedInUserId.ToString());
            if (User.IsInRole("Solicitante"))
            {
                reserva.Aprobada = null;
            }
            else
            {
                reserva.Aprobada = true;
            }

            if (ModelState.IsValid)
            {
                await _apiReservaClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}Add", reserva);

                TempData["SuccessMessage"] = "La reserva se ha creado correctamente.";
                return RedirectToAction(nameof(Add)); // Redirigir a 'Add' para una nueva inserción
            }
            TempData["ErrorMessage"] = "Ocurrió un error al crear la reserva.";
            return View("Add", reserva); // Redirigir a la vista 'Add' si hay errores
        }


        public async Task<IActionResult> GetReservasByFilters(int salaId, DateTime? fechaReserva, TimeSpan? horaInicio)
        {
            try
            {
                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var query = new List<string>();
                if (salaId > 0)
                    query.Add($"salaId={salaId}");

                if (fechaReserva.HasValue)
                    query.Add($"fechaReserva={fechaReserva.Value.ToString("yyyy-MM-dd")}");

                if (horaInicio.HasValue)
                    query.Add($"horaInicio={horaInicio.Value.ToString(@"hh\:mm\:ss")}");

                if (!User.IsInRole("Administrador"))
                {
                    query.Add($"usuarioId={loggedInUserId}");
                }

                var queryString = string.Join("&", query);


                var reservas = await _apiReservaClientService.GetAsync<List<ReservaDTO>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}GetReservasByFilters?{queryString}");
                return Json(reservas);
            }
            catch (Exception ex)
            { // Manejo de errores

                TempData["ErrorMessage"] = "Error al buscar el edificio con el filtro indicado: " + ex.Message;
                return RedirectToAction(nameof(Index)); // Redirigir a 'Add' en caso de excepción
            }
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerReservasCalendario(DateTime start, DateTime end)
        {
            try
            {

                var query = new List<string>();

                query.Add($"fechaInicio={start.ToString("yyyy-MM-dd")}");


                query.Add($"fechaFin= {end.ToString("yyyy-MM-dd")}");



                var queryString = string.Join("&", query);

                var reservas = await _apiReservaClientService.GetAsync<List<ReservaDTO>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}ObtenerReservasCalendario?{queryString}");
                var eventosCalendario = reservas.Select(r => new
                {
                    title = r.Nombre, // Título del evento
                    start = r.FechaReserva.Add(r.HoraInicio).ToString("yyyy-MM-ddTHH:mm:ss"), // Fecha y hora de inicio en formato ISO
                    end = r.FechaReserva.Add(r.HoraFin).ToString("yyyy-MM-ddTHH:mm:ss"), // Fecha y hora de fin en formato ISO
                    allDay = r.HoraInicio == TimeSpan.Zero && r.HoraFin == TimeSpan.Zero, // Si es todo el día (ejemplo, cuando no hay horas específicas)
                    color = r.Aprobada == null ? "#E5AE25" : (r.Aprobada.Value ? "#08E631" : "#E53024"), // Naranja si nulo, verde si true, rojo si false
                    description = r.Observaciones // Observaciones como descripción
                }).ToList();

                // Devolver los datos en formato JSON
                return Json(eventosCalendario);
            }
            catch (Exception ex)
            { // Manejo de errores

                TempData["ErrorMessage"] = "Error al buscar el edificio con el filtro indicado: " + ex.Message;
                return RedirectToAction(nameof(Index)); // Redirigir a 'Add' en caso de excepción
            }
        }
    }
}
