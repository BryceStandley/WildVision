using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository class responsible for handling operations related to JobsPending entities in the database.
/// </summary>
public class JobsPendingRepository(SchemaContext ctx) : IJobsPendingRepository
{
    /// <summary>
    ///     Asynchronously retrieves a list of all pending jobs from the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a list of pending jobs if available, else null.</returns>
    public async Task<List<JobsPending>?> GetJobsPendingAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var jobsPending = await ctx.JobsPending.ToListAsync();
        if (jobsPending.Count != 0) jobsPending.ForEach(j => ctx.JobsPending.Attach(j));
        return jobsPending;
    }

    /// <summary>
    ///     Retrieves a pending job by its queue position from the database.
    /// </summary>
    /// <param name="queuePosition">The position of the job in the queue.</param>
    /// <returns>A task representing the asynchronous operation, containing the job details if found, else null.</returns>
    public async Task<JobsPending?> GetPendingJobByQueuePosition(int queuePosition)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var jobs = await ctx.JobsPending.ToListAsync();
        var job = jobs.Find(j => j.QueueNumber == queuePosition);

        if (job == null) return null;

        await ctx.Entry(job).Reference(j => j.JobDetails).LoadAsync();
        await ctx.Entry(job.JobDetails).Reference(j => j.Graphic).LoadAsync();
        await ctx.Entry(job.JobDetails.Graphic!).Reference(j => j!.Animal).LoadAsync();
        await ctx.Entry(job.JobDetails).Reference(j => j.Model3D).LoadAsync();


        return job;
    }

    /// <summary>
    ///     Asynchronously retrieves a pending job by its unique identifier from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the pending job to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, containing the pending job if found, else null.</returns>
    public async Task<JobsPending?> GetJobsPendingByIdAsync(int id)
    {
        var job = await ctx.JobsPending.FindAsync(id);
        if (job != null)
            ctx.JobsPending.Attach(job);
        return job;
    }

    /// <summary>
    ///     Asynchronously checks the availability of the database.
    /// </summary>
    /// <returns>
    ///     A task representing the asynchronous operation, containing a boolean value indicating the database
    ///     availability status.
    /// </returns>
    public async Task<bool> CheckDbAvailability()
    {
        return await ctx.CheckDbIsAvailable();
    }

    /// <summary>
    ///     Asynchronously creates a new pending job in the database and assigns it a queue number.
    /// </summary>
    /// <param name="jobsPending">The job to be added to the pending jobs list.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the created JobsPending object if the operation is
    ///     successful, else null.
    /// </returns>
    public async Task<JobsPending?> CreateJobsPendingAsync(JobsPending jobsPending)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        // New pending jobs objects will default to -1 queue number until created
        var currentPendingJobs = await GetJobsPendingAsync();
        if (currentPendingJobs != null && currentPendingJobs.Any())
            jobsPending.QueueNumber = currentPendingJobs.Count + 1;
        else
            jobsPending.QueueNumber = 1;
        ctx.JobsPending.Attach(jobsPending);
        ctx.JobsPending.Add(jobsPending);
        await ctx.SaveChangesAsync();
        return jobsPending;
    }

    /// <summary>
    ///     Asynchronously updates the given pending job in the database.
    /// </summary>
    /// <param name="jobsPending">The pending job to be updated.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the updated pending job if the operation succeeds,
    ///     else null.
    /// </returns>
    public async Task<JobsPending?> UpdateJobsPendingAsync(JobsPending jobsPending)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.JobsPending.Update(jobsPending);
        await ctx.SaveChangesAsync();
        return jobsPending;
    }

    /// <summary>
    ///     Asynchronously deletes a specified pending job from the database.
    /// </summary>
    /// <param name="jobsPending">The pending job entity to be deleted.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the number of state entries written to the database
    ///     if successful, else null.
    /// </returns>
    public async Task<int?> DeleteJobsPendingAsync(JobsPending jobsPending)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.JobsPending.Remove(jobsPending);
        return await ctx.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously adjusts the queue number for each pending job in the database.
    /// </summary>
    /// <returns>
    ///     A task representing the asynchronous operation, containing a boolean indicating success, or null if the
    ///     database is unavailable.
    /// </returns>
    public async Task<bool?> ShuffleJobQueue()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var pendingJobs = await ctx.JobsPending.ToListAsync();
        foreach (var job in pendingJobs)
        {
            job.QueueNumber -= 1;
            ctx.JobsPending.Update(job);
            await ctx.SaveChangesAsync();
        }

        return true;
    }

    /// <summary>
    ///     Asynchronously retrieves a pending job by its details identifier.
    /// </summary>
    /// <param name="detailsId">The unique identifier of the job details.</param>
    /// <returns>A task representing the asynchronous operation, containing the pending job if found, else null.</returns>
    public async Task<JobsPending?> GetJobsPendingByDetailsId(Guid detailsId)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var jobs = await ctx.JobsPending.ToListAsync();
        if (jobs.Any())
        {
            var job = jobs.Find(j => j.JobDetailsId == detailsId);
            if (job == null)
                return null;

            ctx.JobsPending.Attach(job);
            return job;
        }

        return null;
    }
}