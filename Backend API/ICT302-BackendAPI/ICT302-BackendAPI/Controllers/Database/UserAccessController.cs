using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     The UserAccessController class provides API endpoints for managing user access permissions within an organization.
/// </summary>
[Route("api/db")]
[ApiController]
public class UserAccessController(IUserAccessRepository userAccessRepo, ILogger<UserAccessController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new user access entry asynchronously.
    /// </summary>
    /// <param name="userAccess">The UserAccess object containing details of the user access to be added.</param>
    /// <returns>An ActionResult representing the outcome of the add operation.</returns>
    [HttpPost("useraccess")]
    public async Task<ActionResult> AddUserAccessAsync([FromBody] UserAccess userAccess)
    {
        try
        {
            return Ok(await userAccessRepo.CreateUserAccessAsync(userAccess));
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
    ///     Retrieves a list of all user access entries asynchronously.
    /// </summary>
    /// <returns>A list of UserAccess objects representing all user access entries.</returns>
    [HttpGet("useraccesses")]
    public async Task<ActionResult> GetUserAccessesAsync()
    {
        try
        {
            var userAccesses = await userAccessRepo.GetUserAccessesAsync();
            return Ok(userAccesses);
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
    ///     Retrieves a user access entry based on organization ID and user ID asynchronously.
    /// </summary>
    /// <param name="orgId">The unique identifier of the organization.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>An IActionResult containing the user access entry if found, or a status indicating the result.</returns>
    [HttpGet("useraccess/{orgId}/{userId}")]
    public async Task<IActionResult> GetUserAccessByKeys(Guid orgId, Guid userId)
    {
        try
        {
            var userAccess = await userAccessRepo.GetUserAccessByKeysAsync(orgId, userId);
            if (userAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(userAccess);
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
    ///     Deletes a user access entry identified by the provided organization ID and user ID.
    /// </summary>
    /// <param name="orgId">The unique identifier of the organization.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>An IActionResult representing the outcome of the delete operation.</returns>
    [HttpDelete("useraccess/{orgId}/{userId}")]
    public async Task<IActionResult> DeleteUserAccess(Guid orgId, Guid userId)
    {
        try
        {
            var existingUserAccess = await userAccessRepo.GetUserAccessByKeysAsync(orgId, userId);
            if (existingUserAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await userAccessRepo.DeleteUserAccessAsync(existingUserAccess);
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
    ///     Updates an existing user access entry.
    /// </summary>
    /// <param name="userAccessToUpdate">The UserAccess object containing updated details of the user access.</param>
    /// <returns>An ActionResult representing the outcome of the update operation.</returns>
    [HttpPut("useraccess")]
    public async Task<IActionResult> UpdateUserAccess([FromBody] UserAccess userAccessToUpdate)
    {
        try
        {
            var existingUserAccess =
                await userAccessRepo.GetUserAccessByKeysAsync(userAccessToUpdate.OrgId, userAccessToUpdate.UserId);
            if (existingUserAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingUserAccess.AccessTypeId = userAccessToUpdate.AccessTypeId;
            await userAccessRepo.UpdateUserAccessAsync(existingUserAccess);
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