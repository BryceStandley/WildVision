using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     The AnimalAccessController class manages the CRUD operations for the AnimalAccess entity via HTTP requests.
/// </summary>
[Route("api/db")]
[ApiController]
public class AnimalAccessController(
    IAnimalAccessRepository animalAccessRepo,
    ILogger<AnimalAccessController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new AnimalAccess entity to the database.
    /// </summary>
    /// <param name="animalAccess">The AnimalAccess entity to be added.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an ActionResult which will contain the created AnimalAccess entity if successful.
    /// </returns>
    [HttpPost("animalaccess")]
    public async Task<ActionResult> AddAnimalAccessAsync([FromBody] AnimalAccess animalAccess)
    {
        try
        {
            animalAccess.AccessID = Guid.NewGuid();
            return Ok(await animalAccessRepo.CreateAnimalAccessAsync(animalAccess));
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
    ///     Retrieves all AnimalAccess entities from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an ActionResult which will contain a list of AnimalAccess entities if successful.
    /// </returns>
    [HttpGet("animalaccesses")]
    public async Task<ActionResult> GetAnimalAccessesAsync()
    {
        try
        {
            var animalAccesses = await animalAccessRepo.GetAnimalAccessesAsync();
            return Ok(animalAccesses);
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
    ///     Retrieves an AnimalAccess entity from the database by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the AnimalAccess entity to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an IActionResult which will contain the AnimalAccess entity if found, or a NotFound result
    ///     if the entity does not exist.
    /// </returns>
    [HttpGet("animalaccess/{id}")]
    public async Task<IActionResult> GetAnimalAccessById(Guid id)
    {
        try
        {
            var animalAccess = await animalAccessRepo.GetAnimalAccessByIDAsync(id);
            if (animalAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(animalAccess);
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
    ///     Deletes an existing AnimalAccess entity from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the AnimalAccess entity to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an IActionResult which will be a NoContent response if successful,
    ///     or a NotFound response if the entity is not found, or an error response in case of an exception.
    /// </returns>
    [HttpDelete("animalaccess/{id}")]
    public async Task<IActionResult> DeleteAnimalAccess(Guid id)
    {
        try
        {
            var existingAnimalAccess = await animalAccessRepo.GetAnimalAccessByIDAsync(id);
            if (existingAnimalAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await animalAccessRepo.DeleteAnimalAccessAsync(existingAnimalAccess);
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
    ///     Updates an existing AnimalAccess entity in the database.
    /// </summary>
    /// <param name="animalAccessToUpdate">The AnimalAccess entity to be updated.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an IActionResult indicating the
    ///     outcome of the operation.
    /// </returns>
    [HttpPut("animalaccess")]
    public async Task<IActionResult> UpdateAnimalAccess([FromBody] AnimalAccess animalAccessToUpdate)
    {
        try
        {
            var existingAnimalAccess = await animalAccessRepo.GetAnimalAccessByIDAsync(animalAccessToUpdate.AccessID);
            if (existingAnimalAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingAnimalAccess.AccessType = animalAccessToUpdate.AccessType;
            existingAnimalAccess.AssignedDate = animalAccessToUpdate.AssignedDate;
            existingAnimalAccess.AnimalID = animalAccessToUpdate.AnimalID;
            existingAnimalAccess.UserID = animalAccessToUpdate.UserID;

            await animalAccessRepo.UpdateAnimalAccessAsync(existingAnimalAccess);
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