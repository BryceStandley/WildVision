using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Controller for managing pending jobs in the database.
/// </summary>
[Route("api/db")]
[ApiController]
public class JobsPendingController(IJobsPendingRepository jobsPendingRepo, ILogger<JobsPendingController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new pending job to the database.
    /// </summary>
    /// <param name="jobsPending">The job to be added to the pending jobs queue.</param>
    /// <returns>An ActionResult indicating the result of the operation.</returns>
    [HttpPost("jobspending")]
    public async Task<ActionResult> AddJobsPendingAsync([FromBody] JobsPending jobsPending)
    {
        try
        {
            var currentPendingJobs = await jobsPendingRepo.GetJobsPendingAsync();
            if (currentPendingJobs!.Any())
                jobsPending.QueueNumber = currentPendingJobs!.Count + 1;
            else
                jobsPending.QueueNumber = 1;

            return Ok(await jobsPendingRepo.CreateJobsPendingAsync(jobsPending));
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
    ///     Retrieves the list of pending jobs from the database.
    /// </summary>
    /// <returns>An ActionResult containing the list of pending jobs.</returns>
    [HttpGet("jobspending")]
    public async Task<ActionResult> GetJobsPendingAsync()
    {
        try
        {
            var jobsPending = await jobsPendingRepo.GetJobsPendingAsync();
            return Ok(jobsPending);
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
    ///     Retrieves a pending job by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the pending job.</param>
    /// <returns>An IActionResult containing the details of the pending job if found, or an error message if not found.</returns>
    [HttpGet("jobspending/{id}")]
    public async Task<IActionResult> GetJobsPendingById(int id)
    {
        try
        {
            var job = await jobsPendingRepo.GetJobsPendingByIdAsync(id);
            if (job == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(job);
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
    ///     Deletes a pending job from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the pending job to be deleted.</param>
    /// <returns>An IActionResult indicating the result of the deletion operation.</returns>
    [HttpDelete("jobspending/{id}")]
    public async Task<IActionResult> DeleteJobsPending(int id)
    {
        try
        {
            var existingJob = await jobsPendingRepo.GetJobsPendingByIdAsync(id);
            if (existingJob == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await jobsPendingRepo.DeleteJobsPendingAsync(existingJob);
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
    ///     Updates an existing pending job in the database.
    /// </summary>
    /// <param name="jobsPendingToUpdate">The job with updated information.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPut("jobspending")]
    public async Task<IActionResult> UpdateJobsPending([FromBody] JobsPending jobsPendingToUpdate)
    {
        try
        {
            var existingJob = await jobsPendingRepo.GetJobsPendingByIdAsync(jobsPendingToUpdate.QueueNumber);
            if (existingJob == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingJob.JobAdded = jobsPendingToUpdate.JobAdded;
            existingJob.Status = jobsPendingToUpdate.Status;
            existingJob.JobDetails = jobsPendingToUpdate.JobDetails;

            await jobsPendingRepo.UpdateJobsPendingAsync(existingJob);
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