using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Application.ViewModels;
using SIRS.Service.API.DTO;
using System;

namespace SIRS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaAppService _reservaAppService;

        public ReservaController(IReservaAppService reservaAppService)
        {
            _reservaAppService = reservaAppService;
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var reserva = _reservaAppService.GetById(id);
            if (reserva == null)
            {
                return NotFound();
            }
            return Ok(reserva);
        }

        [HttpPost("Add")]
        public IActionResult Add(ReservaViewModel reserva)
        {
            _reservaAppService.Add(reserva);
            return CreatedAtAction(nameof(GetById), new { id = reserva.Id }, reserva);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, ReservaViewModel reserva)
        {
            if (id != reserva.Id)
            {
                return BadRequest();
            }
            _reservaAppService.Update(reserva);
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _reservaAppService.Delete(id);
            return NoContent();
        }

        [HttpGet("GetBySalaId/{salaId}")]
        public IActionResult GetBySala(int salaId)
        {
            var reservas = _reservaAppService.GetBySala(salaId);
            return Ok(reservas);
        }

        [HttpGet("GetByUsuario/{usuarioId}")]
        public IActionResult GetByUsuario(int usuarioId)
        {
            var reservas = _reservaAppService.GetByUsuario(usuarioId);
            return Ok(reservas);
        }

        [HttpGet("GetByFecha/{fecha}")]
        public IActionResult GetByFecha([FromQuery] DateTime fecha)
        {
            var reservas = _reservaAppService.GetByFecha(fecha);
            return Ok(reservas);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var reservas = _reservaAppService.GetAll();
            return Ok(reservas);
        }
        [HttpGet("GetReservasByFilters")]
        public IActionResult GetReservasByFilters(int? salaId = null, DateTime? fechaReserva = null, TimeSpan? horaInicio = null)
        {
            var reservas = _reservaAppService.GetReservasByFilters(salaId ?? 0, fechaReserva, horaInicio);
            var reservasDTO = reservas.Select(s => new ReservaDTO
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Observaciones = s.Observaciones,
                NombreSala = s.Sala?.NombreCorto ?? "Sin sala",
                FechaReserva = s.FechaReserva ?? DateTime.MinValue, // Maneja fechas nulas con un valor por defecto
                HoraInicio = s.HoraInicio ?? TimeSpan.Zero, // Maneja tiempos nulos con un valor por defecto
                HoraFin = (s.HoraInicio ?? TimeSpan.Zero).Add(TimeSpan.FromMinutes(s.TiempoTotal ?? 0)) // Calcula HoraFin
            }).ToList();
            return Ok(reservasDTO);
        }

    }
}
