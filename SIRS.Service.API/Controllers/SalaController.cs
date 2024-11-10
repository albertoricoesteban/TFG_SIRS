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
    [Route("api/v{version:apiVersion}/sala-management")]
    public class SalaController : ApiController
    {
        private readonly ISalaAppService _salaAppService;

        public SalaController(
            ISalaAppService salaAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator)
            : base(notifications, mediator)
        {
            _salaAppService = salaAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Response(_salaAppService.GetAll());
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var salaViewModel = _salaAppService.GetById(id);
            return Response(salaViewModel);
        }

        [HttpPost]
        [Authorize(Policy = "CanWriteSalaData", Roles = Roles.Admin)]
        public IActionResult Post([FromBody] SalaViewModel salaViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(salaViewModel);
            }

            _salaAppService.Register(salaViewModel);
            return Response(salaViewModel);
        }

        [HttpPut]
        [Authorize(Policy = "CanWriteSalaData", Roles = Roles.Admin)]
        public IActionResult Put([FromBody] SalaViewModel salaViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(salaViewModel);
            }

            _salaAppService.Update(salaViewModel);
            return Response(salaViewModel);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "CanRemoveSalaData", Roles = Roles.Admin)]
        public IActionResult Delete(int id)
        {
            _salaAppService.Remove(id);
            return Response();
        }
    }
}
