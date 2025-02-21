using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// The JobsCompletedController class provides endpoints for managing completed jobs in the system.
/// This class includes CRUD operations to add, retrieve, update, and delete completed job records.
[Route("api/db")]
[ApiController]
public class JobsCompletedController(
    IJobsCompletedRepository jobsCompletedRepo,
    ILogger<JobsCompletedController> logger)
    : ControllerBase
{
    /// Adds a new job completion record to the system asynchronously.
    /// <param name="job">The job completion record to be added. It should be provided in the request body.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an ActionResult containing the operation
    ///     result.
    /// </returns>
    [HttpPost("jobscompleted")]
    public async Task<ActionResult> AddJobsCompletedAsync([FromBody] JobsCompleted job)
    {
        try
        {
            job.JobID = Guid.NewGuid();
            return Ok(await jobsCompletedRepo.CreateJobsCompletedAsync(job));
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

    /// Retrieves all job completion records from the system asynchronously.
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an ActionResult containing a list of job
    ///     completion records.
    /// </returns>
    [HttpGet("jobscompleted")]
    public async Task<ActionResult> GetJobsCompletedAsync()
    {
        try
        {
            var jobs = await jobsCompletedRepo.GetJobsCompletedAsync();
            return Ok(jobs);
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

    /// Retrieves a job completion record by its ID asynchronously.
    /// <param name="id">The unique identifier of the job completion record to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an ActionResult containing the job
    ///     completion record if found, or a NotFound result if the record does not exist.
    /// </returns>
    [HttpGet("jobscompleted/{id}")]
    public async Task<IActionResult> GetJobsCompletedById(Guid id)
    {
        try
        {
            var job = await jobsCompletedRepo.GetCompletedJobsByIdAsync(id);
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

    /// Deletes an existing job completion record from the system asynchronously.
    /// <param name="id">The unique identifier of the job completion record to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an ActionResult indicating success or
    ///     failure of the operation.
    /// </returns>
    [HttpDelete("jobscompleted/{id}")]
    public async Task<IActionResult> DeleteJobsCompleted(Guid id)
    {
        try
        {
            var existingJob = await jobsCompletedRepo.GetCompletedJobsByIdAsync(id);
            if (existingJob == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await jobsCompletedRepo.DeleteJobsCompletedAsync(existingJob);
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

    /// Updates an existing job completion record asynchronously.
    /// <param name="jobToUpdate">The job completion record to be updated. It should be provided in the request body.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an IActionResult containing the
    ///     operation outcome, such as NoContent for successful updates, or appropriate error statuses.
    /// </returns>
    [HttpPut("jobscompleted")]
    public async Task<IActionResult> UpdateJobsCompleted([FromBody] JobsCompleted jobToUpdate)
    {
        try
        {
            var existingJob = await jobsCompletedRepo.GetCompletedJobsByIdAsync(jobToUpdate.JobID);
            if (existingJob == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingJob.JobType = jobToUpdate.JobType;
            existingJob.JobsStart = jobToUpdate.JobsStart;
            existingJob.JobsEnd = jobToUpdate.JobsEnd;
            existingJob.JobSize = jobToUpdate.JobSize;

            await jobsCompletedRepo.UpdateJobsCompletedAsync(existingJob);
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