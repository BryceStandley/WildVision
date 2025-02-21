using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface definition for the JobsPending repository.
///     Provides methods to interact with jobs pending data store.
/// </summary>
public interface IJobsPendingRepository
{
    /// <summary>
    /// Asynchronously checks the availability of the database.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a boolean value indicating the database
    /// availability status.
    /// </returns>
    Task<bool> CheckDbAvailability();

    /// <summary>
    /// Asynchronously creates a new record in the JobsPending table.
    /// </summary>
    /// <param name="jobsPending">The JobsPending object to be added to the database.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the created JobsPending object
    /// if the operation is successful, or null if it fails.
    /// </returns>
    Task<JobsPending?> CreateJobsPendingAsync(JobsPending jobsPending);

    /// <summary>
    /// Asynchronously deletes a specific pending job from the database.
    /// </summary>
    /// <param name="jobsPending">
    /// The pending job to be deleted.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing an integer value indicating the number of records affected, or null if an error occurred.
    /// </returns>
    Task<int?> DeleteJobsPendingAsync(JobsPending jobsPending);

    /// <summary>
    /// Asynchronously retrieves a pending job by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the pending job.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a JobsPending object if found,
    /// otherwise null.
    /// </returns>
    Task<JobsPending?> GetJobsPendingByIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves the list of pending jobs.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a list of pending jobs, or null if no jobs are found.
    /// </returns>
    Task<List<JobsPending>?> GetJobsPendingAsync();

    /// <summary>
    /// Asynchronously retrieves the pending job by its queue position.
    /// </summary>
    /// <param name="queuePosition">The position of the job in the queue.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the pending job
    /// at the specified queue position, or null if no job is found.
    /// </returns>
    Task<JobsPending?> GetPendingJobByQueuePosition(int queuePosition);

    /// <summary>
    /// Asynchronously updates a pending job in the database.
    /// </summary>
    /// <param name="jobsPending">
    /// The updated details of the pending job to be persisted.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the updated JobsPending object or null if the update fails.
    /// </returns>
    Task<JobsPending?> UpdateJobsPendingAsync(JobsPending jobsPending);

    /// <summary>
    /// Asynchronously adjusts the queue number for each pending job in the database.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a boolean indicating success, or null if the
    /// database is unavailable.
    /// </returns>
    Task<bool?> ShuffleJobQueue();

    /// <summary>
    /// Asynchronously retrieves a pending job by its details ID.
    /// </summary>
    /// <param name="detailsId">The details ID of the job.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the pending job
    /// associated with the specified details ID, if found; otherwise, null.
    /// </returns>
    Task<JobsPending?> GetJobsPendingByDetailsId(Guid detailsId);
}