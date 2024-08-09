using Domain.Common;
using Domain.Entities.Dtos;
using Domain.Exceptions;
using Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    public class TestController() : BaseController
    {
        [HttpGet("405")]
        [SwaggerOperation(Summary = "Returns not allowed exception")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetNotAllowed()
        {
            throw new NotAllowedException();
        }

        [HttpGet("400")]
        [SwaggerOperation(Summary = "Returns not allowed exception")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetBadRequest()
        {
            NotificationsWrapper.AddNotification("Test error");
            throw new NotificationException();
        }

        [HttpPost("200")]
        [SwaggerOperation(Summary = "Returns not allowed exception")]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Post([FromBody] UserDto request)
        {
            return Ok(new GenericResponse<UserDto>(request));
        }
    }
}
