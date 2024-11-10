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
        public class RolController 
        {
          
        }
    }
