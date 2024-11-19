using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Application.ViewModels;
using SIRS.Domain.Bus;
using SIRS.Domain.Notifications;
using SIRS.Services.Api.Controllers;

namespace SIRS.Service.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EdificioController : ControllerBase
    {
        private readonly IEdificioAppService _edificioAppService;

        public EdificioController(IEdificioAppService edificioAppService)
        {
            _edificioAppService = edificioAppService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var edificios = _edificioAppService.GetAll();
            return Ok(edificios);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var edificio = _edificioAppService.GetById(id);
            if (edificio == null)
            {
                return NotFound();
            }
            return Ok(edificio);
        }

        [HttpPost("Add")]
        public IActionResult Add(EdificioViewModel edificio)
        {
            _edificioAppService.Add(edificio);
            return CreatedAtAction(nameof(GetById), new { id = edificio.Id }, edificio);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] EdificioViewModel edificio)
        {
            if (id != edificio.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del objeto.");
            }

            // Actualizar el recurso usando la capa de servicio
            _edificioAppService.Update(edificio);

            // Respuesta 204 No Content, que es apropiada para una actualización exitosa sin contenido adicional
            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _edificioAppService.Delete(id);
            return NoContent();
        }

        [HttpGet("Searchbyname/{name}")]
        public IActionResult SearchByName(string name)
        {
            var edificios = _edificioAppService.SearchByName(name);
            if (edificios == null || !edificios.Any())
            {
                return NotFound();
            }
            return Ok(edificios);
        }


        [HttpGet("GetEdificiosByFilter")]
        public IActionResult GetEdificiosByFilter(string nombre=null, string direccion=null)
        {
            var edificios = _edificioAppService.GetEdificiosByFilter(nombre, direccion);
            if (edificios == null || !edificios.Any())
            {
                return NotFound();
            }
            return Ok(edificios);
        }
    }
}
