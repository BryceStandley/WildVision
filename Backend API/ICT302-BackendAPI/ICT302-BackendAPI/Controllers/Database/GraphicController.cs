// GraphicController.cs

using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     The <c>GraphicController</c> class provides API endpoints for managing graphic records
///     in the database. It allows clients to perform CRUD (Create, Read, Update, Delete) operations
///     and retrieve specific data related to graphics.
/// </summary>
/// <remarks>
///     This controller communicates with the <c>IGraphicRepository</c> and <c>IAnimalRepository</c>
///     to perform operations on the database.
/// </remarks>
[Route("api/db")]
[ApiController]
public class GraphicController(
    IGraphicRepository graphicRepo,
    ILogger<GraphicController> logger,
    IAnimalRepository animalRepo)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new graphic to the database asynchronously.
    /// </summary>
    /// <param name="graphic">The graphic object to be added.</param>
    /// <returns>Returns an ActionResult indicating the outcome of the operation.</returns>
    [HttpPost("graphic")]
    public async Task<ActionResult> AddGraphicAsync([FromBody] Graphic graphic)
    {
        try
        {
            graphic.GPCID = Guid.NewGuid();
            graphic.Animal = await animalRepo.GetAnimalByIdAsync(graphic.AnimalID) ?? new Animal();
            var g = await graphicRepo.CreateGraphicAsync(graphic);
            return Ok(g);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding graphic.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Retrieves a list of all graphics from the database asynchronously.
    /// </summary>
    /// <returns>Returns an ActionResult containing a list of all graphics.</returns>
    [HttpGet("graphics")]
    public async Task<ActionResult> GetGraphicsAsync()
    {
        try
        {
            var graphics = await graphicRepo.GetGraphicsAsync();
            return Ok(graphics);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving graphics.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Retrieves a graphic record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the graphic to be retrieved.</param>
    /// <returns>Returns an IActionResult containing the graphic data if found, or a suitable error message if not.</returns>
    [HttpGet("graphic/{id}")]
    public async Task<IActionResult> GetGraphicById(Guid id)
    {
        try
        {
            var graphic = await graphicRepo.GetGraphicByIDAsync(id);
            if (graphic == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(graphic);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving graphic.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Deletes an existing graphic from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the graphic to be deleted.</param>
    /// <returns>Returns an IActionResult indicating the outcome of the operation.</returns>
    [HttpDelete("graphic/{id}")]
    public async Task<IActionResult> DeleteGraphic(Guid id)
    {
        try
        {
            var existingGraphic = await graphicRepo.GetGraphicByIDAsync(id);
            if (existingGraphic == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await graphicRepo.DeleteGraphicAsync(existingGraphic);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting graphic.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Updates an existing graphic record in the database.
    /// </summary>
    /// <param name="graphicToUpdate">The graphic object containing updated information.</param>
    /// <returns>Returns an IActionResult indicating the outcome of the update operation.</returns>
    [HttpPut("graphic")]
    public async Task<IActionResult> UpdateGraphic([FromBody] Graphic graphicToUpdate)
    {
        try
        {
            var existingGraphic = await graphicRepo.GetGraphicByIDAsync(graphicToUpdate.GPCID);
            if (existingGraphic == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingGraphic.GPCName = graphicToUpdate.GPCName;
            existingGraphic.GPCDateUpload = graphicToUpdate.GPCDateUpload;
            existingGraphic.FilePath = graphicToUpdate.FilePath;
            existingGraphic.AnimalID = graphicToUpdate.AnimalID;
            existingGraphic.GPCSize = graphicToUpdate.GPCSize;

            await graphicRepo.UpdateGraphicAsync(existingGraphic);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating graphic.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Retrieves a video file associated with a given graphic by its GPCID.
    /// </summary>
    /// <param name="gpcID">The unique identifier of the graphic whose video is to be retrieved.</param>
    /// <returns>Returns an IActionResult containing the video file stream if found, otherwise an error message.</returns>
    [HttpGet("graphics/videos/{gpcID}")]
    public async Task<IActionResult> GetVideo(Guid gpcID)
    {
        try
        {
            var graphic = await graphicRepo.GetGraphicByIDAsync(gpcID);
            if (graphic == null) return NotFound(new { message = "Video not found" });

            var videoPath = graphic.FilePath;
            if (!System.IO.File.Exists(videoPath)) return NotFound(new { message = "Video file not found on server." });

            var videoStream = new FileStream(videoPath, FileMode.Open, FileAccess.Read);
            var mimeType = GetMimeType(videoPath);
            return File(videoStream, mimeType, true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving video.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Determines the MIME type based on the file extension.
    /// </summary>
    /// <param name="filePath">The full path of the file to determine the MIME type for.</param>
    /// <returns>Returns a string representing the MIME type associated with the file extension.</returns>
    private string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".mp4" => "video/mp4",
            ".mov" => "video/quicktime",
            ".webm" => "video/webm",
            ".mkv" => "video/x-matroska",
            _ => "application/octet-stream"
        };
    }
}