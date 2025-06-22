using Domain.Common;
using Domain.Common.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TestController : BaseController
{
    [HttpGet("token")]
    public IActionResult Token()
    {
        return Ok(new GenericResponse<BaseViewModel>());
    }
    
    [HttpGet("authorize")]
    [Authorize(Roles = "admin")]
    public IActionResult Authorize()
    {
        return Ok(new GenericResponse<BaseViewModel>());
    }

    [HttpGet("free")]
    [AllowAnonymous]
    public IActionResult Free()
    {
        return Ok(new GenericResponse<BaseViewModel>());
    }
}