using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Controller for managing organisations in the system.
/// </summary>
[Route("api/db")]
[ApiController]
public class OrganisationController(
    IOrganisationRepository organisationRepo,
    ILogger<OrganisationController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new organisation to the system asynchronously.
    /// </summary>
    /// <param name="organisation">The organisation entity to be added.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is a status of the operation, encapsulated
    ///     within an ActionResult.
    /// </returns>
    [HttpPost("organisations")]
    public async Task<ActionResult> AddOrganisationAsync([FromBody] Organisation organisation)
    {
        try
        {
            organisation.OrgID = Guid.NewGuid();
            return Ok(await organisationRepo.CreateOrganisationAsync(organisation));
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
    ///     Retrieves a list of organisations from the system asynchronously.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an ActionResult which encapsulates
    ///     the list of organisations.
    /// </returns>
    [HttpGet("organisations")]
    public async Task<ActionResult> GetOrganisationsAsync()
    {
        try
        {
            var organisations = await organisationRepo.GetOrganisationsAsync();
            return Ok(organisations);
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
    ///     Retrieves an organisation from the system by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the organisation to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an ActionResult which encapsulates
    ///     the organisation entity.
    /// </returns>
    [HttpGet("organisation/{id}")]
    public async Task<IActionResult> GetOrganisationById(Guid id)
    {
        try
        {
            var organisation = await organisationRepo.GetOrganisationByIdAsync(id);
            if (organisation == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(organisation);
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
    ///     Deletes an existing organisation from the system asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the organisation to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an IActionResult indicating the outcome
    ///     of the operation.
    /// </returns>
    [HttpDelete("organisation/{id}")]
    public async Task<IActionResult> DeleteOrganisation(Guid id)
    {
        try
        {
            var existingOrganisation = await organisationRepo.GetOrganisationByIdAsync(id);
            if (existingOrganisation == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await organisationRepo.DeleteOrganisationAsync(existingOrganisation);
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
    ///     Updates an existing organisation in the system.
    /// </summary>
    /// <param name="organisationToUpdate">The organisation entity with updated details.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result indicates the success of the update and
    ///     contains an ActionResult.
    /// </returns>
    [HttpPut("organisation")]
    public async Task<IActionResult> UpdateOrganisation([FromBody] Organisation organisationToUpdate)
    {
        try
        {
            var existingOrganisation = await organisationRepo.GetOrganisationByIdAsync(organisationToUpdate.OrgID);
            if (existingOrganisation == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingOrganisation.OrgID = organisationToUpdate.OrgID;
            existingOrganisation.OrgName = organisationToUpdate.OrgName;
            existingOrganisation.OrgEmail = organisationToUpdate.OrgEmail;

            await organisationRepo.UpdateOrganisationAsync(existingOrganisation);
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