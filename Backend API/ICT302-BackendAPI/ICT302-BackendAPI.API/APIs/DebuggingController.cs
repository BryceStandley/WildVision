using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ICT302_BackendAPI.API.APIs;

/// <summary>
/// Simple endpoint controller for debugging
/// </summary>
/// <param name="logger">Instance of logger to log debug information</param>
/// <param name="environment">Instance of environment to check current hosting environment</param>
[Route("api/debug")]
[ApiController]
public class DebuggingController(ILogger<DebuggingController> logger, IWebHostEnvironment environment)
    : ControllerBase
{
    private readonly ILogger<DebuggingController> _logger = logger;

    /// <summary>
    ///     Endpoint to check if the app environment is in dev mode
    /// </summary>
    /// <returns>Http OK if in dev else Http Not Found</returns>
    [HttpGet]
    public ActionResult GetDebugOk()
    {
        // Only return something if this route is called in development mode
        if (environment.IsDevelopment())
            return Ok(new { message = "Api application is responding..." });

        return NotFound();
    }
}