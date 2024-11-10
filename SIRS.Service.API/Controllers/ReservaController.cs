using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SIRS.Service.API.Controllers
{
    public class ReservaController : Controller
    {
        [Authorize]
        [ApiVersion("1.0")]
        [Route("api/v1/[controller]")]
        public class ReservaController : ApiController
        {
            private readonly IReservaAppService _reservaAppService;

            public ReservaController(IReservaAppService reservaAppService, INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
                : base(notifications, mediator)
            {
                _reservaAppService = reservaAppService;
            }

            [HttpGet]
            [AllowAnonymous]
            [Route("all")]
            public IActionResult GetAllReservations()
            {
                var reservas = _reservaAppService.GetAll();
                return Response(reservas);
            }

            [HttpGet]
            [AllowAnonymous]
            [Route("{id:int}")]
            public IActionResult GetReservationById(int id)
            {
                var reserva = _reservaAppService.GetById(id);
                return Response(reserva);
            }

            [HttpPost]
            [Authorize(Policy = "CanWriteReservationData", Roles = "Admin,User")]
            [Route("create")]
            public IActionResult CreateReservation([FromBody] ReservaViewModel reservaViewModel)
            {
                if (!ModelState.IsValid)
                {
                    NotifyModelStateErrors();
                    return Response(reservaViewModel);
                }

                _reservaAppService.Create(reservaViewModel);
                return Response(reservaViewModel);
            }

            [HttpPut]
            [Authorize(Policy = "CanWriteReservationData", Roles = "Admin")]
            [Route("update")]
            public IActionResult UpdateReservation([FromBody] ReservaViewModel reservaViewModel)
            {
                if (!ModelState.IsValid)
                {
                    NotifyModelStateErrors();
                    return Response(reservaViewModel);
                }

                _reservaAppService.Update(reservaViewModel);
                return Response(reservaViewModel);
            }

            [HttpDelete]
            [Authorize(Policy = "CanRemoveReservationData", Roles = "Admin")]
            [Route("delete/{id:int}")]
            public IActionResult DeleteReservation(int id)
            {
                _reservaAppService.Delete(id);
                return Response();
            }
        }
    }
