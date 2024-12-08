﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Application.ViewModels;

namespace SIRS.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly IUsuarioAppService _usuarioAppService;

        public AccountController(IUsuarioAppService usuarioAppService)
        {
            _usuarioAppService = usuarioAppService;
        }
        [HttpGet("UserExistsByUsername/{username}")]
        public bool UserExistsByUsername(string username)
        {
            var usuarios = _usuarioAppService.UserExistsByUsername(username);
            return usuarios;
        }
        [HttpGet("UserExistsByEmail/{email}")]
        public bool UserExistsByEmail(string email)
        {
            var usuarios = _usuarioAppService.UserExistsByEmail(email);
            return usuarios;
        }
        [HttpPost("Add")]
        public IActionResult Add(UsuarioViewModel usuario)
        {
            if (usuario == null)
            {
                // Si el modelo es nulo, devolvemos un error 400 (Bad Request).
                return BadRequest(new { message = "El usuario no puede ser nulo." });
            }

            try
            {
                // Intentamos agregar el usuario llamando al servicio
                _usuarioAppService.Add(usuario);

                // Si todo fue bien, devolvemos un mensaje de éxito con el estado 200 (OK).
                return Ok(new { message = "Usuario creado exitosamente." });
            }
            catch (Exception ex)
            {
                // Si hay un error inesperado, devolvemos un error 500 con el mensaje de excepción.
                return BadRequest(new { message = "No se pudo crear el usuario. Verifique los datos e intente nuevamente." });
            }
        }
    }
}
