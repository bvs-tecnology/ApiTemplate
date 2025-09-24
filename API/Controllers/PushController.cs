using Domain.Common;
using Domain.Common.InputModel;
using Domain.Entities.Dtos;
using Domain.Entities.Dtos.PushNotification;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PushController(IPushService pushService) : BaseController
{
    [HttpPost("subscription")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Subscribe([FromBody] PushSubscriptionInputModel inputModel)
    {
        await pushService.Subscribe(inputModel, UserId);
        return Created();
    }

    [HttpDelete("subscription")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Unsubscribe()
    {
        await pushService.Unsubscribe(UserId);
        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendPushNotification([FromBody] SendPushInputModel inputModel)
    {
        await pushService.SendPush(inputModel);
        return NoContent();
    }
    
}