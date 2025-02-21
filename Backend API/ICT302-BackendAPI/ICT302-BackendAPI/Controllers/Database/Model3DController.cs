using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     The Model3DController class provides API endpoints to manage 3D models in the database.
/// </summary>
/// <remarks>
///     This class includes operations for adding, retrieving, updating, and deleting 3D models using the
///     IModel3DRepository interface for data access.
/// </remarks>
[Route("api/db")]
[ApiController]
public class Model3DController(IModel3DRepository model3DRepo, ILogger<Model3DController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new 3D model to the database asynchronously.
    /// </summary>
    /// <param name="model">The 3D model to be added, provided in the request body.</param>
    /// <returns>An ActionResult containing the created 3D model or an error message.</returns>
    [HttpPost("model3d")]
    public async Task<ActionResult> AddModel3DAsync([FromBody] Model3D model)
    {
        try
        {
            model.ModelID = Guid.NewGuid(); // Ensure a new GUID is generated for each model
            return Ok(await model3DRepo.CreateModel3DAsync(model));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Retrieves a list of all 3D models from the database asynchronously.
    /// </summary>
    /// <returns>An ActionResult containing a list of 3D models or an error message.</returns>
    [HttpGet("model3ds")]
    public async Task<ActionResult> GetModel3DsAsync()
    {
        try
        {
            var models = await model3DRepo.GetModel3DsAsync();
            return Ok(models);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Retrieves a 3D model from the database by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the 3D model.</param>
    /// <returns>An ActionResult containing the requested 3D model or an error message.</returns>
    [HttpGet("model3d/{id}")]
    public async Task<IActionResult> GetModel3DById(Guid id)
    {
        try
        {
            var model = await model3DRepo.GetModel3DByIdAsync(id);
            if (model == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Model not found"
                });
            return Ok(model);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Deletes an existing 3D model from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the 3D model to be deleted.</param>
    /// <returns>
    ///     A response indicating the result of the deletion operation, including appropriate status codes for not found
    ///     and internal server errors.
    /// </returns>
    [HttpDelete("model3d/{id}")]
    public async Task<IActionResult> DeleteModel3D(Guid id)
    {
        try
        {
            var existingModel = await model3DRepo.GetModel3DByIdAsync(id);
            if (existingModel == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Model not found"
                });

            await model3DRepo.DeleteModel3DAsync(existingModel);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Updates an existing 3D model in the database.
    /// </summary>
    /// <param name="modelToUpdate">The 3D model containing updated fields.</param>
    /// <returns>An IActionResult indicating the outcome of the update operation.</returns>
    [HttpPut("model3d")]
    public async Task<IActionResult> UpdateModel3D(Model3D modelToUpdate)
    {
        try
        {
            var existingModel = await model3DRepo.GetModel3DByIdAsync(modelToUpdate.ModelID);
            if (existingModel == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Model not found"
                });

            // Update fields
            existingModel.ModelTitle = modelToUpdate.ModelTitle;
            existingModel.ModelDateGen = modelToUpdate.ModelDateGen;
            existingModel.FilePath = modelToUpdate.FilePath;
            existingModel.GPCID = modelToUpdate.GPCID;

            await model3DRepo.UpdateModel3DAsync(existingModel);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }
}