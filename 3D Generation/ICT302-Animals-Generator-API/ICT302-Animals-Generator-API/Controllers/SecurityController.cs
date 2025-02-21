using ICT302_Animals_Generator_API.Util;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_Animals_Generator_API.Controllers;

/**
 * <summary>
 *     API Controller for the default route and to return if the application is alive and running.
 *     Also manages auth requests to the <see cref="SecurityMaster" />
 * </summary>
 * <param name="logger">Application logger</param>
 * <param name="configuration">Application configuration</param>
 * <param name="securityMaster">Application security master</param>
 * <remarks>This controller is the default / route for the application</remarks>
 */
[Route("/")]
public class SecurityController(
    ILogger<SecurityController> logger,
    IConfiguration configuration,
    SecurityMaster securityMaster)
    : ControllerBase
{
    /// <summary>Application configuration</summary>
    private readonly IConfiguration _configuration = configuration;

    /// <summary>Logger to log to the console and log file</summary>
    private readonly ILogger<SecurityController> _logger = logger;

    /// <summary>Application SecurityMaster</summary>
    private readonly SecurityMaster _securityMaster = securityMaster;

    /**
     * <summary>Default endpoint function for route / and /alive</summary>
     * <remarks>This is a catch-all for any requests that don't have an auth token</remarks>
     * <returns>Http status 418 I'm a TeaPot</returns>
     */
    [HttpGet("/")]
    [HttpGet("alive")]
    public ActionResult Index()
    {
        return StatusCode(StatusCodes.Status418ImATeapot);
    }

    /**
     * <summary>Catch-all or alive check if this application is responding</summary>
     * <param name="model">Object information from request. Only auth token expected</param>
     * <returns>Http Action Result 200 Ok if token is valid, 418 I'm a TeaPot if not.</returns>
     */
    [HttpPost("*/*")]
    [HttpPost("alive")]
    public ActionResult Alive([FromForm] StartGenerationModel? model)
    {
        if (model == null)
            return StatusCode(StatusCodes.Status418ImATeapot); //Connection isn't authorized, I'm a Teapot

        model.StartGenerationJson = StartGenerationJsonConverter.GetFromJson(HttpContext.Request.Form);

        if (model.StartGenerationJson == null || string.IsNullOrEmpty(model.StartGenerationJson.Token))
            return StatusCode(StatusCodes.Status418ImATeapot); //Connection isn't authorized, I'm a Teapot

        return StatusCode(
            _securityMaster.IsRequestAuthorized(model.StartGenerationJson.Token)); //Only return the status code
    }
}