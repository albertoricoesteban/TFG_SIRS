using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SIRS.Service.API.Controllers
{
    public class EstadoSalaController : Controller
    {
        [Authorize]
        [ApiVersion("1.0")]
        [Route("api/v1/[controller]")]
        public class EstadoSalaController : ApiController
        {
            private readonly IEstadoSalaAppService _estadoSalaAppService;

            public EstadoSalaController(IEstadoSalaAppService estadoSalaAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
                : base(notifications, mediator)
            {
                _estadoSalaAppService = estadoSalaAppService;
            }

            [HttpGet]
            [AllowAnonymous]
            [Route("all")]
            public IActionResult GetAllEstadoSala()
            {
                var estados = _estadoSalaAppService.GetAll();
                return Response(estados);
            }

            [HttpGet]
            [AllowAnonymous]
            [Route("{id:int}")]
            public IActionResult GetEstadoSalaById(int id)
            {
                var estado = _estadoSalaAppService.GetById(id);
                return Response(estado);
            }

            [HttpPost]
            [Authorize(Policy = "CanWriteEstadoSalaData", Roles = "Admin")]
            [Route("create")]
            public IActionResult CreateEstadoSala([FromBody] EstadoSalaViewModel estadoSalaViewModel)
            {
                if (!ModelState.IsValid)
                {
                    NotifyModelStateErrors();
                    return Response(estadoSalaViewModel);
                }

                _estadoSalaAppService.Create(estadoSalaViewModel);
                return Response(estadoSalaViewModel);
            }

            [HttpPut]
            [Authorize(Policy = "CanWriteEstadoSalaData", Roles = "Admin")]
            [Route("update")]
            public IActionResult UpdateEstadoSala([FromBody] EstadoSalaViewModel estadoSalaViewModel)
            {
                if (!ModelState.IsValid)
                {
                    NotifyModelStateErrors();
                    return Response(estadoSalaViewModel);
                }

                _estadoSalaAppService.Update(estadoSalaViewModel);
                return Response(estadoSalaViewModel);
            }

            [HttpDelete]
            [Authorize(Policy = "CanRemoveEstadoSalaData", Roles = "Admin")]
            [Route("delete/{id:int}")]
            public IActionResult DeleteEstadoSala(int id)
            {
                _estadoSalaAppService.Delete(id);
                return Response();
            }
        }
    }
