using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Controller to manage access for organisations in the backend API.
///     Handles Create, Read, Update, and Delete (CRUD) operations for organisation access.
/// </summary>
[Route("api/db")]
[ApiController]
public class OrganisationAccessController(
    IOrganisationAccessRepository organisationAccessRepo,
    ILogger<OrganisationAccessController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new organisation access asynchronously and assigns a new unique identifier.
    /// </summary>
    /// <param name="organisationAccess">The OrganisationAccess object containing the details to be added.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an ActionResult.</returns>
    [HttpPost("organisationaccess")]
    public async Task<ActionResult> AddOrganisationAccessAsync([FromBody] OrganisationAccess organisationAccess)
    {
        try
        {
            organisationAccess.OrgAccessID = Guid.NewGuid();
            return Ok(await organisationAccessRepo.CreateOrganisationAccessAsync(organisationAccess));
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
    ///     Retrieves a list of all organisation accesses asynchronously.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an ActionResult with a list of
    ///     OrganisationAccess objects.
    /// </returns>
    [HttpGet("organisationaccesses")]
    public async Task<ActionResult> GetOrganisationAccessesAsync()
    {
        try
        {
            var organisationAccesses = await organisationAccessRepo.GetOrganisationAccessesAsync();
            return Ok(organisationAccesses);
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
    ///     Retrieves the organisation access details for the specified unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the organisation access to retrieve.</param>
    /// <returns>An IActionResult containing the organisation access details if found, otherwise a not found or error status.</returns>
    [HttpGet("organisationaccess/{id}")]
    public async Task<IActionResult> GetOrganisationAccessById(Guid id)
    {
        try
        {
            var organisationAccess = await organisationAccessRepo.GetOrganisationAccessByIdAsync(id);
            if (organisationAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(organisationAccess);
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
    ///     Deletes an existing organisation access by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the organisation access to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an IActionResult indicating the
    ///     outcome of the delete operation.
    /// </returns>
    [HttpDelete("organisationaccess/{id}")]
    public async Task<IActionResult> DeleteOrganisationAccess(Guid id)
    {
        try
        {
            var existingOrganisationAccess = await organisationAccessRepo.GetOrganisationAccessByIdAsync(id);
            if (existingOrganisationAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await organisationAccessRepo.DeleteOrganisationAccessAsync(existingOrganisationAccess);
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
    ///     Updates an existing organisation access with new details asynchronously.
    /// </summary>
    /// <param name="organisationAccessToUpdate">The OrganisationAccess object containing the details to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IActionResult.</returns>
    [HttpPut("organisationaccess")]
    public async Task<IActionResult> UpdateOrganisationAccess([FromBody] OrganisationAccess organisationAccessToUpdate)
    {
        try
        {
            var existingOrganisationAccess =
                await organisationAccessRepo.GetOrganisationAccessByIdAsync(organisationAccessToUpdate.OrgAccessID);
            if (existingOrganisationAccess == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingOrganisationAccess.OrgAccessID = organisationAccessToUpdate.OrgAccessID;
            existingOrganisationAccess.AccessType = organisationAccessToUpdate.AccessType;
            existingOrganisationAccess.AssignedDate = organisationAccessToUpdate.AssignedDate;
            existingOrganisationAccess.OrgID = organisationAccessToUpdate.OrgID;
            existingOrganisationAccess.AccessID = organisationAccessToUpdate.AccessID;

            await organisationAccessRepo.UpdateOrganisationAccessAsync(existingOrganisationAccess);
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