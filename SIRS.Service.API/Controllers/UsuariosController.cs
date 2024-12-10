using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Application.ViewModels;
using System.Collections.Generic;

namespace SIRS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioAppService _usuarioAppService;

        public UsuariosController(IUsuarioAppService usuarioAppService)
        {
            _usuarioAppService = usuarioAppService;
        }

        // Obtener usuario por ID
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var usuario = _usuarioAppService.GetById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        // Agregar un nuevo usuario
        [HttpPost("Add")]
        public IActionResult Add(UsuarioViewModel usuarioViewModel)
        {
            // Verificar que el modelo sea válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Llamar al servicio de aplicación para agregar el usuario
            _usuarioAppService.Add(usuarioViewModel);

            // Retornar un código de estado 201 Created
            return CreatedAtAction(nameof(GetById), new { id = usuarioViewModel.Id }, usuarioViewModel);
        }

        // Actualizar usuario existente
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, UsuarioViewModel usuarioViewModel)
        {
            if (id != usuarioViewModel.Id)
            {
                return BadRequest();
            }

            // Actualizar usuario
            _usuarioAppService.Update(usuarioViewModel);

            return NoContent(); // Retornar 204 No Content
        }

        // Eliminar usuario
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            // Llamar al servicio para eliminar el usuario
            _usuarioAppService.Delete(id);

            return NoContent(); // Retornar 204 No Content
        }

        // Buscar usuarios por nombre
        [HttpGet("SearchByName/{name}")]
        public IActionResult SearchByName(string name)
        {
            var usuarios = _usuarioAppService.SearchByName(name);
            return Ok(usuarios);
        }

        // Buscar usuarios con filtros
        [HttpGet("SearchByFilters")]
        public IActionResult SearchByFilters(
            string? username = null,
            string? nombre = null,
            string? apellido1 = null,
            string? apellido2 = null,
            string? email = null,
            DateTime? fechaRegistro = null,
            int? rolId = null)
        {
            var usuarios = _usuarioAppService.SearchByFilters(username, nombre, apellido1, apellido2, email, fechaRegistro, rolId);
            return Ok(usuarios);
        }

        // Obtener todos los usuarios
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var usuarios = _usuarioAppService.GetAllUsuarios();
            return Ok(usuarios);
        }

        // Obtener usuarios por rol
        [HttpGet("GetByRol/{rolNombre}")]
        public IActionResult GetByRol(string rolNombre)
        {
            var usuarios = _usuarioAppService.GetByRol(rolNombre);
            return Ok(usuarios);
        }

        // Verificar si el usuario existe por nombre de usuario
        [HttpGet("UserExistsByUsername/{username}")]
        public IActionResult UserExistsByUsername(string username)
        {
            var exists = _usuarioAppService.UserExistsByUsername(username);
            return Ok(exists);
        }

        // Verificar si el usuario existe por correo electrónico
        [HttpGet("UserExistsByEmail/{email}")]
        public IActionResult UserExistsByEmail(string email)
        {
            var exists = _usuarioAppService.UserExistsByEmail(email);
            return Ok(exists);
        }

        // Actualizar perfil de usuario
        [HttpPut("UpdatePerfil/{id}")]
        public IActionResult UpdatePerfil(int id, UsuarioPerfilViewModel model)
        {
            _usuarioAppService.UpdateUsuarioPerfil(id, model);
            return NoContent();
        }

        [HttpGet("GetUsuariosByFilter")]
        public IActionResult GetUsuariosByFilter(string? username, string? nombre, string? apellido1, string? apellido2)
        {
            var salas = _usuarioAppService.GetUsuariosByFilter(username, nombre, apellido1, apellido2);
            return Ok(salas);
        }
    }
}
