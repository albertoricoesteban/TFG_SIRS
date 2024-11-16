using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using System;

namespace SIRS.Service.API.Controllers
{
    [Authorize]
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
    }
}
