﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using System.Collections.Generic;

namespace SIRS.Service.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SalaController : ControllerBase
    {
        private readonly ISalaAppService _salaAppService;

        public SalaController(ISalaAppService salaAppService)
        {
            _salaAppService = salaAppService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var sala = _salaAppService.GetById(id);
            if (sala == null)
            {
                return NotFound();
            }
            return Ok(sala);
        }

        [HttpPost]
        public IActionResult Add(SalaViewModel sala)
        {
            _salaAppService.Add(sala);
            return CreatedAtAction(nameof(GetById), new { id = sala.Id }, sala);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, SalaViewModel sala)
        {
            if (id != sala.Id)
            {
                return BadRequest();
            }
            _salaAppService.Update(sala);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _salaAppService.Delete(id);
            return NoContent();
        }

        [HttpGet("search/{descripcion}")]
        public IActionResult SearchByDescripcion(string descripcion)
        {
            var salas = _salaAppService.SearchByDescripcion(descripcion);
            return Ok(salas);
        }

        [HttpGet("estado/{estado}")]
        public IActionResult GetByEstado(string estado)
        {
            var salas = _salaAppService.GetByEstado(estado);
            return Ok(salas);
        }

        [HttpGet]
        public IActionResult GetAllSalas()
        {
            var salas = _salaAppService.GetAllSalas();
            return Ok(salas);
        }
    }
}
