using Application;
using Domain.Common;
using Domain.Exceptions;
using Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    public class TesteController() : BaseController
    {
        [HttpPost]
        [SwaggerOperation(Summary = "Test of a post endpoint")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public IActionResult Get([FromBody] NotificationModel request)
        {
            NotificationsWrapper.AddNotification("O endpoint deve falhar");
            if(NotificationsWrapper.HasNotification()) throw new NotificationException();
            return Ok(new GenericResponse<object>());
        }
    }
}
