using Microsoft.AspNetCore.Mvc;

namespace HashDiff.Controllers;

/// <summary>
/// Very simple controller. Its purpose is to provide simple endpoint that can be pinged. It is used in test client
/// to determine if the application is loaded.
///
/// The purpose of this controller can be extended in the future to provide full health check functionality that return
/// Ok/Degraded/Failed statuses for subcomponents of this system.
/// </summary>
[ApiController]
public class SystemContoller : ControllerBase
{
    [HttpGet, Route("system/check")]
    public IActionResult GetCheck()
    {
        return Ok();
    }
}