using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository class for managing operations related to completed jobs in the database.
/// </summary>
public class JobsCompletedRepository(SchemaContext ctx) : IJobsCompletedRepository
{
    /// <summary>
    ///     Retrieves a completed job record from the database based on the
    ///     provided job details identifier.
    /// </summary>
    /// <param name="jobDetailsId">The identifier of the job details for which the completed job needs to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the completed job if found;
    ///     otherwise, null.
    /// </returns>
    public async Task<JobsCompleted?> GetCompletedJobsFromJobDetailsIdAsync(Guid? jobDetailsId)
    {
        if (jobDetailsId == null || jobDetailsId == Guid.Empty) return null;

        if (!await ctx.CheckDbIsAvailable())
            return null;


        var jobs = await ctx.JobsCompleted.ToListAsync();
        var job = jobs.Find(jd => jd.JDID == jobDetailsId);

        if (job != null) ctx.JobsCompleted.Attach(job);

        return job;
    }

    /// <summary>
    ///     Retrieves a list of all completed job records from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing the list of completed jobs if found; otherwise,
    ///     null.
    /// </returns>
    public async Task<IEnumerable<JobsCompleted>?> GetJobsCompletedAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var jobs = await ctx.JobsCompleted.ToListAsync();
        return jobs;
    }

    /// <summary>
    ///     Retrieves a completed job record from the database based on the provided job identifier.
    /// </summary>
    /// <param name="id">The identifier of the completed job that needs to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the completed job if found;
    ///     otherwise, null.
    /// </returns>
    public async Task<JobsCompleted?> GetCompletedJobsByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var job = await ctx.JobsCompleted.FindAsync(id);
        if (job != null) ctx.JobsCompleted.Attach(job);
        return job;
    }

    /// <summary>
    ///     Creates a new record for a completed job in the database.
    /// </summary>
    /// <param name="job">The job completion details to be added to the database.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created job if successful;
    ///     otherwise, null.
    /// </returns>
    public async Task<JobsCompleted?> CreateJobsCompletedAsync(JobsCompleted job)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Entry<JobsCompleted>(job).State = EntityState.Added;
        ctx.JobsCompleted.Attach(job);
        ctx.JobsCompleted.Add(job);
        await ctx.SaveChangesAsync();
        return job;
    }

    /// <summary>
    ///     Adds a new job completed record to the database.
    /// </summary>
    /// <param name="job">The job completed entity to be added.</param>
    /// <param name="attach">Determines whether the job entity should be attached to the context.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the added job completed entity if
    ///     the operation was successful; otherwise, null.
    /// </returns>
    public async Task<JobsCompleted?> CreateJobsCompletedAsync(JobsCompleted job, bool attach)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Entry<JobsCompleted>(job).State = EntityState.Added;
        if (attach)
            ctx.JobsCompleted.Attach(job);
        ctx.JobsCompleted.Add(job);
        await ctx.SaveChangesAsync();
        return null;
    }

    /// <summary>
    ///     Updates an existing completed job record in the database.
    /// </summary>
    /// <param name="job">The job record to update.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the updated job record if successful;
    ///     otherwise, null.
    /// </returns>
    public async Task<JobsCompleted?> UpdateJobsCompletedAsync(JobsCompleted job)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.JobsCompleted.Update(job);
        await ctx.SaveChangesAsync();
        return job;
    }

    /// <summary>
    ///     Deletes a completed job from the database.
    /// </summary>
    /// <param name="job">The completed job entity to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the number of state entries written to the database, or null if the database is not available.
    /// </returns>
    public async Task<int?> DeleteJobsCompletedAsync(JobsCompleted job)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.JobsCompleted.Remove(job);
        return await ctx.SaveChangesAsync();
    }
}