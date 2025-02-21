using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Controller to manage CRUD operations for AccessType entities.
/// </summary>
[Route("api/db")]
[ApiController]
public class AccessTypeController(IAccessTypeRepository accessTypeRepo, ILogger<AccessTypeController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new AccessType to the database asynchronously.
    /// </summary>
    /// <param name="accessType">The AccessType object to be added.</param>
    /// <returns>A task representing the asynchronous operation, with an ActionResult as the result.</returns>
    [HttpPost("accesstypes")]
    public async Task<ActionResult> AddAccessTypeAsync([FromBody] AccessType accessType)
    {
        try
        {
            accessType.AccessTypeID = Guid.NewGuid(); // Ensure a new GUID is generated for each access type
            return Ok(await accessTypeRepo.CreateAccessTypeAsync(accessType));
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
    ///     Retrieves all AccessType entities from the database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with an ActionResult containing a list of AccessType entities.</returns>
    [HttpGet("accesstypes")]
    public async Task<ActionResult> GetAccessTypesAsync()
    {
        try
        {
            var accessTypes = await accessTypeRepo.GetAccessTypesAsync();
            return Ok(accessTypes);
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
    ///     Retrieves a specific AccessType entity from the database asynchronously by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the AccessType to be retrieved.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, with an IActionResult containing the AccessType entity if
    ///     found, otherwise a NotFound or error result.
    /// </returns>
    [HttpGet("accesstype/{id}")]
    public async Task<IActionResult> GetAccessTypeByID(Guid id)
    {
        try
        {
            var accessType = await accessTypeRepo.GetAccessTypeByIDAsync(id);
            if (accessType == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Access type not found"
                });
            return Ok(accessType);
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
    ///     Deletes an AccessType from the database asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the AccessType to be deleted.</param>
    /// <returns>A task representing the asynchronous operation, with an IActionResult as the result.</returns>
    [HttpDelete("accesstype/{id}")]
    public async Task<IActionResult> DeleteAccessType(Guid id)
    {
        try
        {
            var existingAccessType = await accessTypeRepo.GetAccessTypeByIDAsync(id);
            if (existingAccessType == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Access type not found"
                });

            await accessTypeRepo.DeleteAccessTypeAsync(existingAccessType);
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
    ///     Updates an existing AccessType in the database.
    /// </summary>
    /// <param name="accessTypeToUpdate">The AccessType object with updated details.</param>
    /// <returns>A task representing the asynchronous operation, with an IActionResult as the result.</returns>
    [HttpPut("accesstype")]
    public async Task<IActionResult> UpdateAccessType(AccessType accessTypeToUpdate)
    {
        try
        {
            var existingAccessType = await accessTypeRepo.GetAccessTypeByIDAsync(accessTypeToUpdate.AccessTypeID);
            if (existingAccessType == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Access type not found"
                });

            existingAccessType.AccessTypeDetails = accessTypeToUpdate.AccessTypeDetails;
            await accessTypeRepo.UpdateAccessTypeAsync(existingAccessType);
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