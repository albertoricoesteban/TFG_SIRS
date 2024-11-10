using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using System.Collections.Generic;

namespace SIRS.Service.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoSalaController : ControllerBase
    {
        private readonly IEstadoSalaAppService _estadoSalaAppService;

        public EstadoSalaController(IEstadoSalaAppService estadoSalaAppService)
        {
            _estadoSalaAppService = estadoSalaAppService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var estadoSala = _estadoSalaAppService.GetById(id);
            if (estadoSala == null)
            {
                return NotFound();
            }
            return Ok(estadoSala);
        }

        [HttpPost]
        public IActionResult Add(EstadoSalaViewModel estadoSala)
        {
            _estadoSalaAppService.Add(estadoSala);
            return CreatedAtAction(nameof(GetById), new { id = estadoSala.Id }, estadoSala);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, EstadoSalaViewModel estadoSala)
        {
            if (id != estadoSala.Id)
            {
                return BadRequest();
            }
            _estadoSalaAppService.Update(estadoSala);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _estadoSalaAppService.Delete(id);
            return NoContent();
        }

        [HttpGet]
        public IActionResult GetAllEstados()
        {
            var estadosSala = _estadoSalaAppService.GetAllEstados();
            return Ok(estadosSala);
        }
    }
}
