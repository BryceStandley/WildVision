using System.Net.Http.Json;
using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ICT302_BackendAPI.API.Generation;

/// <summary>
///     Job queue monitor
/// </summary>
/// <param name="taskQueue">the queue to monitor</param>
/// <param name="logger">application logger</param>
/// <param name="configuration">application configuration</param>
/// <param name="applicationLifetime">application lifetime</param>
/// <param name="jobsPendingRepository">repository for jobs pending</param>
/// <param name="jobsCompletedRepository">repository for jobs complete</param>
/// <param name="jobDetailsRepository">repository for job details</param>
/// <param name="webHostEnvironment">application web host env</param>
public sealed class MonitorJobLoop(
    IBackgroundJobQueue taskQueue,
    ILogger<MonitorJobLoop> logger,
    IConfiguration configuration,
    IHostApplicationLifetime applicationLifetime,
    IJobsPendingRepository jobsPendingRepository,
    IJobsCompletedRepository jobsCompletedRepository,
    IJobDetailsRepository jobDetailsRepository,
    IWebHostEnvironment webHostEnvironment)
{
    private const int ApiAliveDelayS = 10;

    private const int ApiAliveDelayMs = ApiAliveDelayS * 1000;

    private static int _totalInQueue;

    private readonly CancellationToken _cancellationToken = applicationLifetime.ApplicationStopping;
    private HttpClient? _sharedHttpClient;

    /// <summary>
    ///     Starts the loop to monitor jobs in the queue
    /// </summary>
    public async void StartMonitorLoop()
    {
        logger.LogInformation("Pending job queue monitor loop is starting.");
        logger.LogInformation("Checking if Database is available.");

        // Don't start the monitor unless the database is available 
        if (!await jobsPendingRepository.CheckDbAvailability())
        {
            logger.LogInformation("Database is not available. Monitoring pending jobs will be disabled.");
            return;
        }

        logger.LogInformation("Database is available.");
        // Run an init check if there are any pending jobs on start up
        _ = Task.Run(async () => await AssignJobWorkItem(), _cancellationToken);
    }

    /// <summary>
    ///     Gets the shared Http client
    /// </summary>
    /// <returns>The http client</returns>
    private HttpClient GetHttpClient()
    {
        if (_sharedHttpClient == null)
        {
            _sharedHttpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["GenAPIUrl"] ?? "http://localhost:5000")
            };
            return _sharedHttpClient;
        }

        return _sharedHttpClient;
    }

    /// <summary>
    ///     Creates a alive request to the generation API application
    /// </summary>
    /// <returns>HttpRequestMessage</returns>
    private HttpRequestMessage CreateAliveRequestAsync()
    {
        var genApiUrl = new Uri(configuration["GenAPIUrl"] + "/alive");

        var data = new
        {
            Token = configuration["GenAPIAuthToken"]
        };

        var request = new HttpRequestMessage(HttpMethod.Post, genApiUrl);
        var content = new MultipartFormDataContent();
        content.Add(JsonContent.Create(data), "StartGenerationJson");

        request.Content = content;
        return request;
    }

    /// <summary>
    ///     Assigns the work items and checks if the generation API is alive
    /// </summary>
    public async ValueTask AssignJobWorkItem()
    {
        //Check if the Generator API is alive and note how many items are waiting...
        //if alive continue with work items, if not delay for x seconds and check again until gen api is alive
        _totalInQueue++;

        HttpResponseMessage? alive = null;

        while (alive == null || alive.IsSuccessStatusCode)
            try
            {
                alive = await GetHttpClient().SendAsync(CreateAliveRequestAsync(), _cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogWarning("Generation API is down or not responding... retrying in {seconds} seconds...",
                    ApiAliveDelayS);
                logger.LogWarning("Error: {error}", e.Message);
                var delay = Task.Delay(ApiAliveDelayMs, _cancellationToken);
                await delay;
            }

        for (var i = 0; i < _totalInQueue; i++) await taskQueue.QueueBackgroundWorkItemAsync(BuildJobWorkItem);
    }

    /// <summary>
    ///     Builds the work item and processes the work step by step
    /// </summary>
    /// <param name="token">the cancellation token to cancel the work item</param>
    private async ValueTask BuildJobWorkItem(CancellationToken token)
    {
        var job = await jobsPendingRepository.GetPendingJobByQueuePosition(1);

        if (job is null)
            return;

        string storedFilesPath;
        if (webHostEnvironment.IsDevelopment())
        {
            // Dev environment
            var path = configuration["dev_StoredFilesPath"];
            storedFilesPath = path ?? "";
        }
        else
        {
            // Prod environment
            storedFilesPath = configuration["StoredFilesPath"] ?? "";
        }

        var startTime = DateTime.Now;
        var modelSize = 0;
        // Each step of processing. Job is finished when status is Closed
        while (!token.IsCancellationRequested && job.Status != JobStatus.Closed)
        {
            var genEndpoint = JobStatusHelper.GetJobStatusEndpoint(job.Status);
            try
            {
                var data = new
                {
                    Token = configuration["GenAPIAuthToken"], JobID = job.JobDetails.JDID,
                    FileName = job.JobDetails.Graphic!.FilePath,
                    SubjectHint = job.JobDetails.Graphic.Animal?.AnimalType,
                    ModelPath = job.JobDetails.JDID + ".glb", ModelVersion = 1
                };

                HttpResponseMessage? result = null;
                if (job.Status == JobStatus.Validating)
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, genEndpoint);
                    await using var stream =
                        File.OpenRead(Path.Join(storedFilesPath, job.JobDetails.Graphic.FilePath));
                    using var content = new MultipartFormDataContent();

                    content.Add(new StreamContent(stream), "InputFile", job.JobDetails.Graphic.FilePath); // Other data
                    content.Add(JsonContent.Create(data), "StartGenerationJson");

                    request.Content = content;

                    result = await GetHttpClient().SendAsync(request, token);
                    logger.LogInformation("Result: {0}", result.Content.ReadAsStringAsync().Result);
                }
                else if (JobStatusHelper.IsJobInProcessingStaus(job.Status) || job.Status == JobStatus.Fetching)
                {
                    if (job.Status != JobStatus.Submitted)
                    {
                        using var request = new HttpRequestMessage(HttpMethod.Post, genEndpoint);
                        using var content = new MultipartFormDataContent();
                        // Other data
                        content.Add(JsonContent.Create(data), "StartGenerationJson");

                        request.Content = content;

                        result = await GetHttpClient().SendAsync(request, token);
                        if (job.Status != JobStatus.Fetching)
                            logger.LogInformation("Result: {0}", result.Content.ReadAsStringAsync().Result);
                    }
                }

                if ((result != null && result.IsSuccessStatusCode &&
                     JobStatusHelper.IsJobInProcessingStaus(job.Status)) || job.Status == JobStatus.Submitted)
                {
                    job.Status = JobStatusHelper.GetNextJobStatusFromPrevious(job.Status);
                    await jobsPendingRepository.UpdateJobsPendingAsync(job);
                    continue;
                }

                if (job.Status == JobStatus.Finished)
                {
                    job.Status = JobStatusHelper.GetNextJobStatusFromPrevious(job.Status);
                    await jobsPendingRepository.UpdateJobsPendingAsync(job);
                    continue;
                }

                if (result != null && result.IsSuccessStatusCode && job.Status == JobStatus.Fetching)
                {
                    var modelFilePath = Path.Join(storedFilesPath, job.JobDetails.Model3D!.FilePath);
                    logger.LogInformation("Model file fetched successfully and saved to {modelFilePath}",
                        modelFilePath);

                    await using (var fs = new FileStream(modelFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await result.Content.CopyToAsync(fs, token);
                    }

                    ;


                    modelSize = (int)new FileInfo(modelFilePath).Length;
                    job.Status = JobStatusHelper.GetNextJobStatusFromPrevious(job.Status);
                    await jobsPendingRepository.UpdateJobsPendingAsync(job);
                }

                if (job.Status == JobStatus.Closing)
                {
                    var id = job.JobDetails.JDID;
                    var jobSize = job.JobDetails.Graphic.GPCSize;
                    var genType = job.JobDetails.ModelGenType;

                    var completeJob = new JobsCompleted
                    {
                        JobID = id,
                        JobsEnd = DateTime.Now,
                        JobType = genType,
                        JobSize = jobSize + modelSize,
                        JobsStart = startTime,
                        JDID = id
                    };

                    await jobsCompletedRepository.CreateJobsCompletedAsync(completeJob, false);
                    job.Status = JobStatusHelper.GetNextJobStatusFromPrevious(job.Status);
                    await jobsPendingRepository.UpdateJobsPendingAsync(job);
                    continue;
                }

                if (job.Status == JobStatus.Error)
                {
                    logger.LogInformation("Queued work item {jobId} occured an error and has been canceled",
                        job.JobDetailsId);
                    var jd = job.JobDetails;
                    await jobsPendingRepository.DeleteJobsPendingAsync(job);
                    await jobDetailsRepository.DeleteJobDetailsAsync(jd);
                    break;
                }

                if (job.Status != JobStatus.Submitted)
                {
                    // If we got to this point, there was an issue...
                    job.Status = JobStatus.Error;
                    logger.LogError("Error: job {jobId} failed.", job.JobDetailsId);
                    var j = job.JobDetails;
                    await jobsPendingRepository.DeleteJobsPendingAsync(job);
                    await jobDetailsRepository.DeleteJobDetailsAsync(j);
                }
            }
            catch (Exception e)
            {
                logger.LogInformation("Error processing job: {error}", e.Message);
            }
        }

        if (job.Status == JobStatus.Closed)
        {
            logger.LogInformation("Job {jobId} has finished", job.JobDetailsId);

            //Delete pending job from the queue and shuffle down the next pending job to position
            await jobsPendingRepository.DeleteJobsPendingAsync(job);
            await jobsPendingRepository.ShuffleJobQueue();
        }
    }
}