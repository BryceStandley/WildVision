using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ICT302_BackendAPI.API.APIs;

/// <summary>
/// Controller for handling file-related endpoints
/// </summary>
[Route("api/files")]
[ApiController]
public class FilesController(
    IConfiguration configuration,
    ILogger<FilesController> logger,
    IAnimalRepository animalRepository,
    IWebHostEnvironment webHostEnvironment,
    IAnimalAccessRepository animalAccessRepository,
    IGraphicRepository graphicRepository,
    IModel3DRepository model3DRepository)
    : ControllerBase
{
    private readonly IGraphicRepository _graphicRepository = graphicRepository;


    /// <summary>
    ///     Endpoint to get details of an animal from its ID
    /// </summary>
    /// <param name="id">The animals ID</param>
    /// <returns>Http status</returns>
    [HttpGet("animals/details/{id}")]
    public async Task<IActionResult> GetAnimalDetails(Guid id)
    {
        try
        {
            var animal = await animalRepository.GetAnimalByIdAsync(id);
            if (animal == null) return NotFound(new { message = "Animal not found" });

            // Construct video URLs dynamically based on the filename
            foreach (var graphic in animal.Graphics)
                if (webHostEnvironment.EnvironmentName == "Development")
                    graphic.FilePath = $"http://{Request.Host}/api/files/animals/videos/{graphic.FilePath}";
                else
                    graphic.FilePath = $"https://{Request.Host}/api/files/animals/videos/{graphic.FilePath}";

            return Ok(animal);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving animal details.");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }


    /// <summary>
    ///     Endpoint to get a file from the host server from a provided filename
    /// </summary>
    /// <param name="fileName">The file name to serve to the requested client</param>
    /// <returns>Http result</returns>
    [HttpGet("animals/videos/{fileName}")]
    public IActionResult GetAnimalVideo(string fileName)
    {
        try
        {
            var storedFilesPath = webHostEnvironment.IsDevelopment()
                ? configuration.GetValue<string>("dev_StoredFilesPath")
                : configuration.GetValue<string>("StoredFilesPath");

            var filePath = Path.Combine(storedFilesPath!, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                logger.LogWarning("Requested video file not found: {FilePath}", filePath);
                return NotFound(new { message = "Video file not found." });
            }

            var mimeType = GetMimeType(filePath);
            var videoStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(videoStream, mimeType, true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while serving video file: {FileName}", fileName);
            return StatusCode(500, new { message = "Internal server error while fetching video." });
        }
    }

    /// <summary>
    ///     Endpoint to get the full list of animals within the db
    /// </summary>
    /// <returns>Http status</returns>
    [HttpGet("animals/list")]
    public async Task<IActionResult> GetAnimalsListAsync()
    {
        try
        {
            var animals = await animalRepository.GetAnimalsAsync();

            return !animals!.Any() ? Ok(new { message = "No animals found" }) : Ok(animals);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching the list of animals");
            return StatusCode(500, new { message = "Internal server error while fetching animals list." });
        }
    }

    /// <summary>
    ///     Gets the mime type from a file name
    /// </summary>
    /// <param name="filePath">The filepath to check</param>
    /// <returns>string of the mime type</returns>
    private string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".mp4" => "video/mp4",
            ".mov" => "video/quicktime",
            ".avi" => "video/x-msvideo",
            ".mkv" => "video/x-matroska",
            _ => "application/octet-stream"
        };
    }

    /// <summary>
    ///     Endpoint to get the animal ID's for a given user
    /// </summary>
    /// <param name="userID">The userID to check</param>
    /// <returns>Http status</returns>
    [HttpGet("user/{userID}/animalIDs")]
    public async Task<IActionResult> GetAnimalIDsByUserID(Guid userID)
    {
        try
        {
            var animalIDs = await animalAccessRepository.GetAnimalIDsByUserIDAsync(userID);
            if (!animalIDs!.Any()) return NotFound(new { message = "No animals found for this user." });
            return Ok(animalIDs);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching animal IDs for the user.");
            return StatusCode(500, new { message = "Internal server error while fetching animal IDs." });
        }
    }

    /// <summary>
    ///     Endpoint to get a animals 3D generated model from the graphic it was generated from
    /// </summary>
    /// <param name="graphicsId">The graphic ID the model was generated from</param>
    /// <returns>Http status</returns>
    [HttpGet("animals/models/graphics/{graphicsId}")]
    public async Task<ActionResult> GetAnimalModel(Guid graphicsId)
    {
        try
        {
            var model = await model3DRepository.GetModel3DFromGraphicsIdAsync(graphicsId);
            if (model == null)
                return NotFound(new { message = "Animal model not found" });

            model.FilePath = webHostEnvironment.EnvironmentName == "Development"
                ? $"http://{Request.Host}/api/files/animals/models/file/{model.FilePath}"
                : $"https://{Request.Host}/api/files/animals/models/file/{model.FilePath}";


            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while fetching the model from the provided graphic");
            return StatusCode(500,
                new { message = "Internal server error while fetching the model from the provided graphic" });
        }
    }

    /// <summary>
    ///     Endpoint to get all models for a given animal
    /// </summary>
    /// <param name="animalId">The animal id to search</param>
    /// <returns>Http status</returns>
    [HttpGet("animals/models/{animalId}")]
    public async Task<ActionResult> GetAllModelsForAnimal(Guid animalId)
    {
        try
        {
            var models = await model3DRepository.GetModel3DListFromAnimalIdAsync(animalId);
            if (models!.Count == 0)
                return NotFound(new { message = "Animal model not found" });

            foreach (var model in models)
                if (webHostEnvironment.EnvironmentName == "Development")
                    model.FilePath = $"http://{Request.Host}/api/files/animals/models/file/{model.FilePath}";
                else
                    model.FilePath = $"https://{Request.Host}/api/files/animals/models/file/{model.FilePath}";

            return Ok(models);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred while fetching the model from the provided graphic");
            return StatusCode(500,
                new { message = "Internal server error while fetching the model from the provided graphic" });
        }
    }

    /// <summary>
    ///     Endpoint to serve a requested 3D model file to the client
    /// </summary>
    /// <param name="fileName">The file name of the 3D model</param>
    /// <returns>Http status</returns>
    [HttpGet("animals/models/file/{fileName}")]
    public ActionResult GetModelFile(string fileName)
    {
        try
        {
            var storedFilesPath = webHostEnvironment.IsDevelopment()
                ? configuration.GetValue<string>("dev_StoredFilesPath")
                : configuration.GetValue<string>("StoredFilesPath");

            var filePath = Path.Combine(storedFilesPath!, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                logger.LogWarning("Requested model file not found: {FilePath}", filePath);
                return NotFound(new { message = "Model file not found." });
            }

            var mimeType = GetMimeType(filePath);
            var modelStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(modelStream, mimeType, true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while serving model file: {FileName}", fileName);
            return StatusCode(500, new { message = "Internal server error while fetching model." });
        }
    }
}