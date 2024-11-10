using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SIRS.Service.API.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/edificio-management")]
    public class EdificioController : ApiController
    {
        private readonly IEdificioAppService _edificioAppService;

        public EdificioController(
            IEdificioAppService edificioAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator)
            : base(notifications, mediator)
        {
            _edificioAppService = edificioAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Response(_edificioAppService.GetAll());
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var edificioViewModel = _edificioAppService.GetById(id);
            return Response(edificioViewModel);
        }

        [HttpPost]
        [Authorize(Policy = "CanWriteEdificioData", Roles = Roles.Admin)]
        public IActionResult Post([FromBody] EdificioViewModel edificioViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(edificioViewModel);
            }

            _edificioAppService.Register(edificioViewModel);
            return Response(edificioViewModel);
        }

        [HttpPut]
        [Authorize(Policy = "CanWriteEdificioData", Roles = Roles.Admin)]
        public IActionResult Put([FromBody] EdificioViewModel edificioViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(edificioViewModel);
            }

            _edificioAppService.Update(edificioViewModel);
            return Response(edificioViewModel);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "CanRemoveEdificioData", Roles = Roles.Admin)]
        public IActionResult Delete(int id)
        {
            _edificioAppService.Remove(id);
            return Response();
        }
    }
}
