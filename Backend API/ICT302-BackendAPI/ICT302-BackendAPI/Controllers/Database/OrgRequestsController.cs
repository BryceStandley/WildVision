using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     The OrgRequestsController class handles HTTP requests related to organization requests.
/// </summary>
[Route("api/db")]
[ApiController]
public class OrgRequestsController(IOrgRequestsRepository orgRequestsRepo, ILogger<OrgRequestsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new organization request asynchronously.
    /// </summary>
    /// <param name="orgRequests">The organization request object to be added.</param>
    /// <returns>An ActionResult representing the result of the asynchronous operation.</returns>
    [HttpPost("orgrequests")]
    public async Task<ActionResult> AddOrgRequestAsync([FromBody] OrgRequests orgRequests)
    {
        try
        {
            orgRequests.RequestID = Guid.NewGuid();
            return Ok(await orgRequestsRepo.CreateOrgRequestsAsync(orgRequests));
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
    ///     Retrieves all organization requests asynchronously.
    /// </summary>
    /// <returns>An ActionResult containing the list of organization requests.</returns>
    [HttpGet("orgrequests")]
    public async Task<ActionResult> GetOrgRequestsAsync()
    {
        try
        {
            var orgRequests = await orgRequestsRepo.GetOrgRequestsAsync();
            return Ok(orgRequests);
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
    ///     Retrieves an organization request by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the organization request.</param>
    /// <returns>
    ///     An IActionResult containing the organization request if found, otherwise a 404 Not Found or 500 Internal
    ///     Server Error status code.
    /// </returns>
    [HttpGet("orgrequests/{id}")]
    public async Task<IActionResult> GetOrgRequestById(Guid id)
    {
        try
        {
            var orgRequest = await orgRequestsRepo.GetOrgRequestsByIdAsync(id);
            if (orgRequest == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(orgRequest);
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
    ///     Deletes an organization request by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the organization request to be deleted.</param>
    /// <returns>An IActionResult representing the result of the delete operation.</returns>
    [HttpDelete("orgrequests/{id}")]
    public async Task<IActionResult> DeleteOrgRequest(Guid id)
    {
        try
        {
            var existingOrgRequest = await orgRequestsRepo.GetOrgRequestsByIdAsync(id);
            if (existingOrgRequest == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await orgRequestsRepo.DeleteOrgRequestsAsync(existingOrgRequest);
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
    ///     Updates an existing organization request.
    /// </summary>
    /// <param name="orgRequestToUpdate">The organization request object to be updated.</param>
    /// <returns>An IActionResult representing the result of the update operation.</returns>
    [HttpPut("orgrequests")]
    public async Task<IActionResult> UpdateOrgRequest([FromBody] OrgRequests orgRequestToUpdate)
    {
        try
        {
            var existingOrgRequest = await orgRequestsRepo.GetOrgRequestsByIdAsync(orgRequestToUpdate.RequestID);
            if (existingOrgRequest == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingOrgRequest.OrgID = orgRequestToUpdate.OrgID;
            existingOrgRequest.UserID = orgRequestToUpdate.UserID;
            existingOrgRequest.DateRequested = orgRequestToUpdate.DateRequested;
            existingOrgRequest.DateProcessed = orgRequestToUpdate.DateProcessed;
            existingOrgRequest.Status = orgRequestToUpdate.Status;

            await orgRequestsRepo.UpdateOrgRequestsAsync(existingOrgRequest);
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