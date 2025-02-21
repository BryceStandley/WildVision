using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository interface for managing completed job records in the database.
/// </summary>
public interface IJobsCompletedRepository
{
    /// <summary>
    /// Creates a new record for a completed job in the database asynchronously.
    /// </summary>
    /// <param name="job">The job completion details to be added to the database.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created job if successful;
    /// otherwise, null.
    /// </returns>
    Task<JobsCompleted?> CreateJobsCompletedAsync(JobsCompleted job);

    /// <summary>
    /// Creates a new record for a completed job in the database asynchronously.
    /// </summary>
    /// <param name="job">The job completion details to be added to the database.</param>
    /// <param name="attach">Specifies whether the job entity should be attached to the context.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the created job if successful;
    /// otherwise, null.
    /// </returns>
    Task<JobsCompleted?> CreateJobsCompletedAsync(JobsCompleted job, bool attach);

    /// <summary>
    /// Deletes a completed job record from the database asynchronously.
    /// </summary>
    /// <param name="job">The completed job record to be deleted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of state entries written to the
    /// database if successful; otherwise, null.
    /// </returns>
    Task<int?> DeleteJobsCompletedAsync(JobsCompleted job);

    /// <summary>
    /// Retrieves a completed job record by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the completed job to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the completed job if found;
    /// otherwise, null.
    /// </returns>
    Task<JobsCompleted?> GetCompletedJobsByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a completed job record based on the provided Job Details ID asynchronously.
    /// </summary>
    /// <param name="jobDetailsId">The unique identifier of the job details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the completed job if found;
    /// otherwise, null.
    /// </returns>
    Task<JobsCompleted?> GetCompletedJobsFromJobDetailsIdAsync(Guid? jobDetailsId);

    /// <summary>
    /// Retrieves a list of all completed job records from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the list of completed jobs if found; otherwise,
    /// null.
    /// </returns>
    Task<IEnumerable<JobsCompleted>?> GetJobsCompletedAsync();

    /// <summary>
    /// Updates an existing record for a completed job in the database asynchronously.
    /// </summary>
    /// <param name="job">The job completion details to be updated in the database.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated job if successful;
    /// otherwise, null.
    /// </returns>
    Task<JobsCompleted?> UpdateJobsCompletedAsync(JobsCompleted job);
}