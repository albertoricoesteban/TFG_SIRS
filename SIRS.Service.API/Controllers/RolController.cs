using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIRS.Domain.Bus;
using SIRS.Domain.Notifications;
using MediatR;
using SIRS.Services.Api.Controllers;

namespace SIRS.Service.API.Controllers
{

        [Authorize]
        [Route("api/v1/[controller]")]
        public class RolController : ApiController
        {
            private readonly IRolAppService _rolAppService;

            public RolController(IRolAppService rolAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
                : base(notifications, mediator)
            {
                _rolAppService = rolAppService;
            }

            [HttpGet]
            [AllowAnonymous]
            [Route("all")]
            public IActionResult GetAllRoles()
            {
                var roles = _rolAppService.GetAll();
                return Response(roles);
            }

            [HttpGet]
            [AllowAnonymous]
            [Route("{id:int}")]
            public IActionResult GetRoleById(int id)
            {
                var role = _rolAppService.GetById(id);
                return Response(role);
            }

            [HttpPost]
            [Authorize(Policy = "CanWriteRoleData", Roles = "Admin")]
            [Route("create")]
            public IActionResult CreateRole([FromBody] RolViewModel rolViewModel)
            {
                if (!ModelState.IsValid)
                {
                    NotifyModelStateErrors();
                    return Response(rolViewModel);
                }

                _rolAppService.Create(rolViewModel);
                return Response(rolViewModel);
            }

            [HttpPut]
            [Authorize(Policy = "CanWriteRoleData", Roles = "Admin")]
            [Route("update")]
            public IActionResult UpdateRole([FromBody] RolViewModel rolViewModel)
            {
                if (!ModelState.IsValid)
                {
                    NotifyModelStateErrors();
                    return Response(rolViewModel);
                }

                _rolAppService.Update(rolViewModel);
                return Response(rolViewModel);
            }

            [HttpDelete]
            [Authorize(Policy = "CanRemoveRoleData", Roles = "Admin")]
            [Route("delete/{id:int}")]
            public IActionResult DeleteRole(int id)
            {
                _rolAppService.Delete(id);
                return Response();
            }
        }
    }
