using Domain.Exceptions;
using Domain.Interfaces.Services;
using Domain.SeedWork.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TestController(
    ILogger<TestController> logger,
    ITestService testService,
    INotification notification
) : BaseController
{
    [HttpGet("token")]
    public IActionResult Token()
    {
        return Ok();
    }
    
    [HttpGet("authorize")]
    [Authorize(Roles = "admin")]
    public IActionResult Authorize()
    {
        return Ok();
    }

    [HttpGet("free")]
    [AllowAnonymous]
    public async Task<IActionResult> Free()
    {
        logger.LogInformation("starting free method");
        var result = await testService.TestExchange();
        logger.LogInformation("finishing free method");
        return Ok(result);
    }

    [HttpGet("free/random-error")]
    [AllowAnonymous]
    public IActionResult RandomError()
    {
        logger.LogInformation("starting random error method");
        throw new ArgumentException("error while trying to resolve random error method");
    }

    [HttpGet("free/notification-error")]
    [AllowAnonymous]
    public IActionResult NotificationError()
    {
        logger.LogInformation("starting notification error method");
        notification.AddNotification("error while trying to resolve notification error method");
        throw new NotificationException();
    }
}