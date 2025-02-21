using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for performing CRUD operations on job details.
/// </summary>
public interface IJobDetailsRepository
{
    /// <summary>
    /// Asynchronously creates a new job detail record in the database.
    /// </summary>
    /// <param name="jobDetails">The job details to be created.</param>
    /// <returns>Returns the created job details if successful; otherwise, null.</returns>
    Task<JobDetails?> CreateJobDetailsAsync(JobDetails jobDetails);

    /// <summary>
    /// Asynchronously deletes a job detail record from the database.
    /// </summary>
    /// <param name="jobDetails">The job details to be deleted.</param>
    /// <returns>Returns the number of affected rows if successful; otherwise, null.</returns>
    Task<int?> DeleteJobDetailsAsync(JobDetails jobDetails);

    /// <summary>
    /// Asynchronously retrieves job details by their unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the job details to retrieve.</param>
    /// <returns>Returns the job details if found; otherwise, null.</returns>
    Task<JobDetails?> GetJobDetailsByIDAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves job details by the provided graphic ID.
    /// </summary>
    /// <param name="graphicId">The ID of the graphic associated with the job details.</param>
    /// <returns>Returns the job details if found; otherwise, null.</returns>
    Task<JobDetails?> GetJobDetailsByGraphicIdAsync(Guid? graphicId);

    /// <summary>
    /// Asynchronously retrieves all job details from the database.
    /// </summary>
    /// <returns>Returns an IEnumerable of JobDetails if the database is available; otherwise, null.</returns>
    Task<IEnumerable<JobDetails>?> GetJobDetailsAsync();

    /// <summary>
    /// Asynchronously updates job details in the database.
    /// </summary>
    /// <param name="jobDetails">The job details to update in the database.</param>
    /// <returns>
    /// A Task representing the asynchronous operation.
    /// The Task result contains the updated JobDetails if the update was successful; otherwise, null.
    /// </returns>
    Task<JobDetails?> UpdateJobDetailsAsync(JobDetails jobDetails);
}