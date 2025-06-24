using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TestController : BaseController
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

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
    public IActionResult Free()
    {
        _logger.LogInformation("starting free method");
        _logger.LogInformation("finishing free method");
        return Ok();
    }

    [HttpGet("free/error")]
    [AllowAnonymous]
    public IActionResult FreeError()
    {
        _logger.LogInformation("starting free method");
        _logger.LogError("error while trying to resolve free method");
        throw new Exception("error while trying to resolve free method");
    }
}