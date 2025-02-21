using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Controller responsible for handling HTTP requests related to job details.
/// </summary>
[Route("api/db")]
[ApiController]
public class JobDetailsController(IJobDetailsRepository jobDetailsRepo, ILogger<JobDetailsController> logger)
    : ControllerBase
{
    /// <summary>Asynchronously adds job details to the database.</summary>
    /// <param name="jobDetails">The job details object to be added.</param>
    /// <return>
    ///     A task that represents the asynchronous operation. The task result contains an ActionResult that represents
    ///     the HTTP response.
    /// </return>
    [HttpPost("jobdetails")]
    public async Task<ActionResult> AddJobDetailsAsync([FromBody] JobDetails jobDetails)
    {
        try
        {
            jobDetails.JDID = Guid.NewGuid();
            return Ok(await jobDetailsRepo.CreateJobDetailsAsync(jobDetails));
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

    /// <summary>Asynchronously retrieves all job details from the database.</summary>
    /// <return>
    ///     A task that represents the asynchronous operation. The task result contains an ActionResult which includes
    ///     a collection of JobDetails objects or an error message if the operation fails.
    /// </return>
    [HttpGet("jobdetails")]
    public async Task<ActionResult> GetJobDetailsAsync()
    {
        try
        {
            var jobDetails = await jobDetailsRepo.GetJobDetailsAsync();
            return Ok(jobDetails);
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

    /// <summary>Asynchronously retrieves job details by the specified unique identifier.</summary>
    /// <param name="id">The unique identifier of the job details to retrieve.</param>
    /// <return>
    ///     A task that represents the asynchronous operation. The task result contains an IActionResult that
    ///     represents the HTTP response, which includes the job details if found, or a corresponding error message.
    /// </return>
    [HttpGet("jobdetails/{id}")]
    public async Task<IActionResult> GetJobDetailsById(Guid id)
    {
        try
        {
            var jobDetails = await jobDetailsRepo.GetJobDetailsByIDAsync(id);
            if (jobDetails == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(jobDetails);
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

    /// <summary>Asynchronously deletes job details from the database.</summary>
    /// <param name="id">The unique identifier of the job details to be deleted.</param>
    /// <return>
    ///     A task that represents the asynchronous operation. The task result contains an ActionResult indicating the
    ///     outcome of the delete operation.
    /// </return>
    [HttpDelete("jobdetails/{id}")]
    public async Task<IActionResult> DeleteJobDetails(Guid id)
    {
        try
        {
            var existingJobDetails = await jobDetailsRepo.GetJobDetailsByIDAsync(id);
            if (existingJobDetails == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await jobDetailsRepo.DeleteJobDetailsAsync(existingJobDetails);
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

    /// <summary>Asynchronously updates job details in the database.</summary>
    /// <param name="jobDetailsToUpdate">The job details object to be updated.</param>
    /// <return>
    ///     A task that represents the asynchronous operation. The task result is an ActionResult indicating the outcome of
    ///     the operation.
    /// </return>
    [HttpPut("jobdetails")]
    public async Task<IActionResult> UpdateJobDetails([FromBody] JobDetails jobDetailsToUpdate)
    {
        try
        {
            var existingJobDetails = await jobDetailsRepo.GetJobDetailsByIDAsync(jobDetailsToUpdate.JDID);
            if (existingJobDetails == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingJobDetails.JDID = jobDetailsToUpdate.JDID;

            await jobDetailsRepo.UpdateJobDetailsAsync(existingJobDetails);
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