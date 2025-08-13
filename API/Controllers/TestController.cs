using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TestController(
    ILogger<TestController> logger,
    ITestService testService
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

    [HttpGet("free/error")]
    [AllowAnonymous]
    public IActionResult FreeError()
    {
        logger.LogInformation("starting free method");
        logger.LogError("error while trying to resolve free method");
        throw new Exception("error while trying to resolve free method");
    }
}