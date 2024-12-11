using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using System.Collections.Generic;

namespace SIRS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoSalasController : ControllerBase
    {
        private readonly IEstadoSalaAppService _estadoSalaAppService;

        public EstadoSalasController(IEstadoSalaAppService estadoSalaAppService)
        {
            _estadoSalaAppService = estadoSalaAppService;
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var estadoSala = _estadoSalaAppService.GetById(id);
            if (estadoSala == null)
            {
                return NotFound();
            }
            return Ok(estadoSala);
        }

        [HttpPost("Add")]
        public IActionResult Add(EstadoSalaViewModel estadoSala)
        {
            _estadoSalaAppService.Add(estadoSala);
            return CreatedAtAction(nameof(GetById), new { id = estadoSala.Id }, estadoSala);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, EstadoSalaViewModel estadoSala)
        {
            if (id != estadoSala.Id)
            {
                return BadRequest();
            }
            _estadoSalaAppService.Update(estadoSala);
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _estadoSalaAppService.Delete(id);
            return NoContent();
        }

        [HttpGet("GetAllEstados")]
        public IActionResult GetAllEstados()
        {
            var estadosSala = _estadoSalaAppService.GetAllEstados();
            return Ok(estadosSala);
        }
    }
}
