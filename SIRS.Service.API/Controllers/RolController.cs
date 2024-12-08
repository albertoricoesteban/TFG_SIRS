using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIRS.Domain.Bus;
using SIRS.Domain.Notifications;
using MediatR;
using SIRS.Services.Api.Controllers;
using SIRS.Application.Interfaces;

namespace SIRS.Service.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RolController: ControllerBase
    {
        private readonly IRolAppService _rolAppService;

        public RolController(IRolAppService rolAppService)
        {
            _rolAppService = rolAppService;
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var roles = _rolAppService.GetById(id);
            if (roles == null)
            {
                return NotFound();
            }
            return Ok(roles);
        }

        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _rolAppService.GetAll();
            if (roles == null)
            {
                return NotFound();
            }
            return Ok(roles);
        }
    }
    
}