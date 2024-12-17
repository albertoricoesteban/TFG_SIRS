using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIRS.ApliClient;
using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using SIRS.Service.API.DTO;
using System.Globalization;
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
        public ActionResult Add(ReservaViewModel reserva)
        {

            if (reserva == null)
            {
                reserva = new ReservaViewModel();
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
            if (User.IsInRole("Administrador"))
            {
                var usuarios = _apiReservaClientService.GetAsync<List<UsuarioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.UsuarioControlador}GetAll").Result;
                if (usuarios != null && usuarios.Any())
                {
                    // Convertir la respuesta en una lista de SelectListItem para usar en la vista
                    ViewBag.Usuarios = usuarios.Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),  // El valor será el Id del Edificio
                        Text = $"{e.Nombre} {e.Apellido1} {e.Apellido2}"       // El texto será la descripción del Edificio
                    }).ToList();
                }
                else
                {
                    // Si no hay resultados de la API, asignar una lista vacía
                    ViewBag.Usuarios = new List<SelectListItem>();
                }
            }
            return View(reserva);
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

                if (User.IsInRole("Administrador"))
                {
                    var usuarios = _apiReservaClientService.GetAsync<List<UsuarioViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.UsuarioControlador}GetAll").Result;
                    if (usuarios != null && usuarios.Any())
                    {
                        // Convertir la respuesta en una lista de SelectListItem para usar en la vista
                        ViewBag.Usuarios = usuarios.Select(e => new SelectListItem
                        {
                            Value = e.Id.ToString(),  // El valor será el Id del Edificio
                            Text = $"{e.Nombre} {e.Apellido1} {e.Apellido2}"       // El texto será la descripción del Edificio
                        }).ToList();
                    }
                    else
                    {
                        // Si no hay resultados de la API, asignar una lista vacía
                        ViewBag.Usuarios = new List<SelectListItem>();
                    }
                }
                if (reserva == null || (User.IsInRole("Solicitante") && (reserva.UsuarioId != int.Parse(loggedInUserId.ToString()))))
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
                var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (User.IsInRole("Solicitante") && reserva.UsuarioId != loggedInUserId)
                {
                    return Json(new { success = false, message = "No puede cancelar una reserva que no es suya." });
                }

                // Cancelar la reserva
                await _apiReservaClientService.PostAsyncWithId($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}CancelarReserva/{id}/{loggedInUserId}", id, loggedInUserId);

                return Json(new { success = true, message = "Reserva cancelada correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hubo un error al cancelar la reserva: {ex.Message}" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AprobarReserva(int id)
        {
            try
            {
                if (User.IsInRole("Administrador"))
                {
                    // Obtener la reserva para verificar el UsuarioId
                    var reserva = await _apiReservaClientService.GetAsync<ReservaDTO>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}GetById/{id}");

                    if (reserva == null)
                    {
                        return Json(new { success = false, message = "La reserva no existe." });
                    }

                    // Verificar si el usuario logueado es el propietario de la reserva
                    var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    // Cancelar la reserva
                    await _apiReservaClientService.PostAsyncWithId($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}AprobarReserva/{id}/{loggedInUserId}", id, loggedInUserId);

                    return Json(new { success = true, message = "Reserva aprobada correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No tiene permisos para realziar esta acción." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hubo un error al aprobada la reserva: {ex.Message}" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ReactivarReserva(int id)
        {
            try
            {
                if (User.IsInRole("Administrador"))
                {
                    // Obtener la reserva para verificar el UsuarioId
                    var reserva = await _apiReservaClientService.GetAsync<ReservaDTO>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}GetById/{id}");

                    if (reserva == null)
                    {
                        return Json(new { success = false, message = "La reserva no existe." });
                    }

                    // Verificar si el usuario logueado es el propietario de la reserva
                    var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    await _apiReservaClientService.PostAsyncWithId($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}ReactivarReserva/{id}/{loggedInUserId}", id, loggedInUserId);

                    return Json(new { success = true, message = "Reserva reactivada correctamente." });
                }
                else
                {
                    return Json(new { success = false, message = "No tiene permisos para realizar esta acción." });
                }
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
                if (User.IsInRole("Administrador"))
                {
                    if (reserva.UsuarioId <= 0)
                    {
                        TempData["ErrorMessage"] = "Debe seleccionar un usuario válido.";
                        return RedirectToAction(nameof(Update), new { id = reserva.Id });
                    }
                }
                else
                {
                    // Si es un solicitante, usamos el usuario logueado
                    reserva.UsuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                }
                ModelState.Remove("Aprobada");
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
                        UsuarioId = User.IsInRole("Administrador") ? reserva.UsuarioId : int.Parse(loggedInUserId.ToString()),  // Usamos el Usuario logueado para la actualización
                        UsuarioGestionId = int.Parse(loggedInUserId.ToString())
                    };
                    var esAdmin = User.IsInRole("Administrador");

                    var urlEdicion = $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}Update/{reserva.Id}";
                    await _apiReservaClientService.PutAsync(urlEdicion, reser);

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

        [HttpGet]
        public IActionResult GetSalasByEdificio(int edificioId)
        {
            var salas = _apiReservaClientService.GetAsync<List<SalaViewModel>>($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.SalaControlador}GetByEdificioId/{edificioId}").Result;
            return Json(salas);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReservaViewModel reserva)
        {


            // Enviar los datos a la API
            //todo quitar este usuario para coger el que está logado
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.IsInRole("Solicitante"))
            {
                reserva.UsuarioId = int.Parse(loggedInUserId.ToString());
            }
            reserva.UsuarioGestionId = int.Parse(loggedInUserId.ToString());
            if (User.IsInRole("Solicitante"))
            {
                reserva.Aprobada = null;
            }
            else
            {
                reserva.Aprobada = true;
            }
            DateTime fechaHoraInicio;
            try
            {
                // Combinar FechaReserva y HoraInicio y analizar con ParseExact
                var fechaHoraTexto = $"{reserva.FechaReserva:dd/MM/yyyy} {reserva.HoraInicio}";
                fechaHoraInicio = DateTime.Parse(fechaHoraTexto);
            }
            catch (FormatException ex)
            {
                TempData["ErrorMessage"] = $"El formato de la fecha o la hora de inicio es incorrecto. {ex.Message}";
                return RedirectToAction(nameof(Add), reserva);
            }

            // Validar que la fecha y hora de inicio no sean anteriores a la actual
            if (fechaHoraInicio < DateTime.Now)
            {
                TempData["ErrorMessage"] = "La fecha y hora de inicio no pueden ser anteriores a la fecha y hora actual.";
                return RedirectToAction(nameof(Add), reserva);
            }
            if (ModelState.IsValid)
            {
                await _apiReservaClientService.PostAsync($"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}Add", reserva);

                TempData["SuccessMessage"] = "La reserva se ha creado correctamente.";
                return RedirectToAction(nameof(Add)); // Redirigir a 'Add' para una nueva inserción
            }
            TempData["ErrorMessage"] = "Ocurrió un error al crear la reserva.";
            return RedirectToAction(nameof(Add), reserva); // Redirigir a la vista 'Add' si hay errores
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
        public async Task<IActionResult> ObtenerReservasCalendario(DateTime start, DateTime end, int? edificioId = null, int? salaId = null)
        {
            try
            {
                var query = new List<string>
        {
            $"fechaInicio={start:yyyy-MM-dd}",
            $"fechaFin={end:yyyy-MM-dd}"
        };

                if (edificioId.HasValue)
                    query.Add($"edificioId={edificioId.Value}");

                if (salaId.HasValue)
                    query.Add($"salaId={salaId.Value}");

                var queryString = string.Join("&", query);

                var reservas = await _apiReservaClientService.GetAsync<List<ReservaDTO>>(
                    $"{Constantes.Constantes.ApiBaseUrl}{Constantes.Constantes.ReservaControlador}ObtenerReservasCalendario?{queryString}");

                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var eventosCalendario = reservas.Select(r => new
                {
                    title = User.IsInRole("Solicitante") && int.Parse(r.UsuarioId.ToString()) != int.Parse(usuarioId.ToString())
                        ? (r.Aprobada == true ? "Reservada" : "Pendiente de aprobar")
                        : r.Nombre,
                    start = r.FechaReserva.Add(r.HoraInicio).ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = r.FechaReserva.Add(r.HoraFin).ToString("yyyy-MM-ddTHH:mm:ss"),
                    allDay = r.HoraInicio == TimeSpan.Zero && r.HoraFin == TimeSpan.Zero,
                    color = r.Aprobada == null ? "#E5AE25" : (r.Aprobada.Value ? "#08E631" : "#E53024"),
                    description = r.Observaciones,
                    textColor = User.IsInRole("Solicitante") && int.Parse(r.UsuarioId.ToString()) != int.Parse(usuarioId.ToString())
        ? "#FF0000" // Texto rojo si no es propio
        : "#000000", // Texto negro para eventos propios
                    id = r.Id,
                    // Agregar el usuarioId en extendedProps para poder usarlo en el frontend
                    extendedProps = new
                    {
                        usuarioId = r.UsuarioId
                    }
                }).ToList();

                return Json(eventosCalendario);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar el calendario: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
