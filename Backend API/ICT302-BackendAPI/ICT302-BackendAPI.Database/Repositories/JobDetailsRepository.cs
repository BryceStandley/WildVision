using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Provides a repository for managing job details in the database.
/// </summary>
public class JobDetailsRepository(SchemaContext ctx) : IJobDetailsRepository
{
    /// <summary>
    ///     Asynchronously retrieves job details by a given graphic ID.
    /// </summary>
    /// <param name="graphicId">The unique identifier for the graphic.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task result contains the JobDetails
    ///     if found, or null if the graphic ID is null or the database is unavailable.
    /// </returns>
    public async Task<JobDetails?> GetJobDetailsByGraphicIdAsync(Guid? graphicId)
    {
        if (graphicId == null) return null;

        if (!await ctx.CheckDbIsAvailable())
            return null;

        var jobs = await ctx.JobDetails.ToListAsync();
        var job = jobs.Find(job => job.GPCID == graphicId);
        return job;
    }

    /// <summary>
    ///     Asynchronously retrieves all job details from the database.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task result contains an IEnumerable of JobDetails
    ///     if the database is available, or null if the database is unavailable.
    /// </returns>
    public async Task<IEnumerable<JobDetails>?> GetJobDetailsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var jobDetails = await ctx.JobDetails.ToListAsync();
        return jobDetails;
    }

    /// <summary>
    ///     Asynchronously retrieves job details by a given job ID.
    /// </summary>
    /// <param name="id">The unique identifier for the job.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task result contains the JobDetails if found, or null
    ///     if the database is unavailable or the job ID does not exist.
    /// </returns>
    public async Task<JobDetails?> GetJobDetailsByIDAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var jobDetails = await ctx.JobDetails.FindAsync(id);
        if (jobDetails != null) ctx.JobDetails.Attach(jobDetails);
        return jobDetails;
    }

    /// <summary>
    ///     Asynchronously creates and saves new job details.
    /// </summary>
    /// <param name="jobDetails">The JobDetails object containing the information to be saved.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task result contains the created JobDetails object if
    ///     successful, or null if the database is unavailable.
    /// </returns>
    public async Task<JobDetails?> CreateJobDetailsAsync(JobDetails jobDetails)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.JobDetails.Attach(jobDetails);
        ctx.JobDetails.Add(jobDetails);
        await ctx.SaveChangesAsync();
        return jobDetails;
    }

    /// <summary>
    ///     Asynchronously updates job details in the database.
    /// </summary>
    /// <param name="jobDetails">The job details to update in the database.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    ///     The Task result contains the updated JobDetails if the update was successful,
    ///     or null if the database is unavailable.
    /// </returns>
    public async Task<JobDetails?> UpdateJobDetailsAsync(JobDetails jobDetails)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.JobDetails.Update(jobDetails);
        await ctx.SaveChangesAsync();
        return jobDetails;
    }

    /// <summary>
    ///     Asynchronously deletes job details from the database.
    /// </summary>
    /// <param name="jobDetails">The job details entity to be deleted.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task result contains the number of state entities
    ///     written to the database, or null if the database is unavailable.
    /// </returns>
    public async Task<int?> DeleteJobDetailsAsync(JobDetails jobDetails)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.JobDetails.Remove(jobDetails);
        return await ctx.SaveChangesAsync();
    }
}