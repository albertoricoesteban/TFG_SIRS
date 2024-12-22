using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using SIRS.Service.API.DTO;
using System.Collections.Generic;

namespace SIRS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalasController : ControllerBase
    {
        private readonly ISalaAppService _salaAppService;
        private readonly IEstadoSalaAppService _estadoSalaAppService;

        public SalasController(ISalaAppService salaAppService, IEstadoSalaAppService estadoSalaAppService)
        {
            _salaAppService = salaAppService;
            _estadoSalaAppService = estadoSalaAppService;
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var sala = _salaAppService.GetById(id);
            if (sala == null)
            {
                return NotFound();
            }
            return Ok(sala);
        }

        [HttpPost("Add")]
        public IActionResult Add(SalaViewModel sala)
        {
            _salaAppService.Add(sala);
            return CreatedAtAction(nameof(GetById), new { id = sala.Id }, sala);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, SalaViewModel sala)
        {
            if (id != sala.Id)
            {
                return BadRequest();
            }
            _salaAppService.Update(sala);
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _salaAppService.Delete(id);
            return NoContent();
        }

        [HttpGet("SearchByDescripcion/{descripcion}")]
        public IActionResult SearchByDescripcion(string descripcion)
        {
            var salas = _salaAppService.SearchByDescripcion(descripcion);
            return Ok(salas);
        }

        [HttpGet("GetByEstado/{estado}")]
        public IActionResult GetByEstado(string estado)
        {
            var salas = _salaAppService.GetByEstado(estado);
            return Ok(salas);
        }

        [HttpGet("GetSalasByFilterWithEdificioNombre")]
        public IActionResult GetSalasByFilterWithEdificioNombre()
        {
            var salas = _salaAppService.GetSalasByFilterWithEdificioNombre();
            return Ok(salas);
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var salas = _salaAppService.GetAll().Select(s => new SalaDTO
            {
                Id = s.Id,
                NombreCorto = s.NombreCorto,
                Descripcion = s.Descripcion,
                Capacidad = s.Capacidad,
                EstadoSalaId = s.EstadoSalaId,
            }).ToList();

            var estados = _estadoSalaAppService.GetAllEstados().ToList();

            foreach (var item in salas)
            {
                // Encuentra el estado correspondiente por ID
                var estado = estados.FirstOrDefault(a => a.Id == item.EstadoSalaId);
                if (estado != null)
                {
                    item.EstadoSala = estado.Descripcion; // Asigna la descripción del estado
                }
            }
            return Ok(salas);
        }
        [HttpGet("GetSalasByFilter")]
        public IActionResult GetSalasByFilter(string? nombreCorto, int capacidad, int edificioId)
        {
            var salas = _salaAppService.GetSalasByFilter(nombreCorto,capacidad,edificioId).Select(s => new SalaDTO
            {
                Id = s.Id,
                NombreCorto = s.NombreCorto,
                Descripcion = s.Descripcion,
                Capacidad = s.Capacidad,
                EstadoSalaId = s.EstadoSalaId,
            }).ToList();
            var estados = _estadoSalaAppService.GetAllEstados().ToList();

            foreach (var item in salas)
            {
                // Encuentra el estado correspondiente por ID
                var estado = estados.FirstOrDefault(a => a.Id == item.EstadoSalaId);
                if (estado != null)
                {
                    item.EstadoSala = estado.Descripcion; // Asigna la descripción del estado
                }
            }
            return Ok(salas);
        }

        [HttpGet("GetByEdificioId/{edificioId}")]
        public IActionResult GetByEdificioId(int edificioId)
        {
            var salas = _salaAppService.GetByEdificioId(edificioId);
            return Ok(salas);
        }

    }
}
