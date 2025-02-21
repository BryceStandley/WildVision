using System.Net.Http.Json;
using ICT302_BackendAPI.API.Generation;
using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ICT302_BackendAPI.API.APIs;

/// <summary>
///     Generation endpoint controller to control user generation requests from the frontend
/// </summary>
[Route("api/generate")]
[ApiController]
public class GenerationController(
    IConfiguration configuration,
    ILogger<GenerationController> logger,
    IGraphicRepository graphicRepository,
    IJobDetailsRepository jobDetailsRepository,
    IJobsPendingRepository jobsPendingRepository,
    IJobsCompletedRepository jobsCompletedRepository,
    IModel3DRepository model3DRepository,
    MonitorJobLoop jobLoop,
    IAnimalRepository animalRepository)
    : ControllerBase
{
    /// <summary>
    ///     Endpoint to check if the Generation API application is alive and responding
    /// </summary>
    /// <returns>Http status</returns>
    [HttpGet]
    public ActionResult IsAlive()
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri(configuration.GetValue<string>("GenAPIUrl")!)
        };
        var data = new
        {
            Token = configuration["GenAPIAuthToken"]
        };

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(configuration.GetValue<string>("GenAPIUrl")!));
        var content = new MultipartFormDataContent();
        content.Add(JsonContent.Create(data), "StartGenerationJson");

        request.Content = content;
        var res = client.Send(request);
        return Ok(res.IsSuccessStatusCode);
    }

    /// <summary>
    ///     Endpoint to create a Generation request from a graphic
    /// </summary>
    /// <param name="request">The request data from the http post</param>
    /// <returns>Http status</returns>
    [HttpPost]
    public async Task<ActionResult> GenerateFromGraphicAsync(GenerationRequest? request)
    {
        try
        {
            //Safety checks on request data
            if (request == null)
                return BadRequest(new { message = "Error: No request information provided" });

            if (request.AnimalId == null)
                return BadRequest(new { message = "Error: No AnimalID provided within request" });

            if (string.IsNullOrEmpty(request.AnimalGraphicFileName))
                return BadRequest(new { message = "Error: No Graphic File Name provided within request" });

            if (string.IsNullOrEmpty(request.GenType))
                return BadRequest(new { message = "Error: No Generation Type provided within request" });

            // TODO: Check file name, might need to do some directory/path changes for valid file name
            var graphic = request.GraphicID != null
                ? await graphicRepository.GetGraphicByIDAsync(request.GraphicID)
                : await graphicRepository.GetGraphicByFileNameAsync(request.AnimalGraphicFileName);

            if (graphic == null)
                return BadRequest(new { message = "Error: No Graphic could be found with provided ID or file name" });

            graphic.Animal = await animalRepository.GetAnimalByIdAsync(graphic.AnimalID);

            logger.LogInformation(
                $"3D model for animal: {graphic.AnimalID} with graphic: {graphic.GPCName} has been requested. Checking job status...");

            graphic.FilePath = Path.GetFileName(graphic.FilePath);

            // Checking if the graphic has already processed a job or pending job making it a duplicate request
            var jobDetails = await jobDetailsRepository.GetJobDetailsByGraphicIdAsync(graphic.GPCID);
            var jobsPending = await jobsPendingRepository.GetJobsPendingAsync();
            if (jobDetails != null)
            {
                // Check if Job has already been completed
                var completedJob =
                    await jobsCompletedRepository.GetCompletedJobsFromJobDetailsIdAsync(jobDetails.JDID);
                if (completedJob != null)
                    return BadRequest(new
                    {
                        message =
                            $"Error: Requested generation job was already completed with jobID: {completedJob.JobID}"
                    });

                // Check if job is the queue
                if (jobsPending!.Any())
                {
                    var pendingJob = jobsPending!.Find(job => job.JobDetailsId == jobDetails.JDID);
                    if (pendingJob != null)
                        return BadRequest(new
                        {
                            message =
                                $"Error: Requested generation job is already queued with jobID: {pendingJob.JobDetailsId} in queue position: {pendingJob.QueueNumber}"
                        });
                }

                return BadRequest(new
                {
                    message =
                        $"Error: Requested generation job with jobID: {jobDetails.JDID} has details but isnt in the queue or complete... Internal Job error?"
                });
            }


            // If all checks passed... The job can now be created and submitted to the generation API


            // Create the model record for the job
            var model = new Model3D
            {
                GPCID = graphic.GPCID,
                ModelID = Guid.NewGuid(),
                ModelTitle = graphic.GPCName,
                FilePath = graphic.GPCID + ".glb",
                ModelDateGen = DateTime.Now
            };
            await model3DRepository.CreateModel3DAsync(model);

            // Create the details for the job
            var newJob = new JobDetails
            {
                JDID = Guid.NewGuid(),
                GPCID = graphic.GPCID,
                ModelGenType = request.GenType,
                ModelID = model.ModelID
            };
            await jobDetailsRepository.CreateJobDetailsAsync(newJob);

            logger.LogInformation("Required model and job details generated... adding to job queue...");

            //Create the Pending job and let the repo set the queue position
            var newPendingJob = new JobsPending
            {
                JobDetails = newJob,
                JobDetailsId = newJob.JDID,
                QueueNumber = -1,
                Status = JobStatus.Submitted,
                JobAdded = DateTime.Now
            };
            await jobsPendingRepository.CreateJobsPendingAsync(newPendingJob);

            // Telling the job monitor there's a job pending and needs to be added to the job queue
            // DO NOT await. We don't want the api to wait for gen to complete before returning to the frontend
            // This is a fire and forget return. A background thread will do the work from here
            _ = jobLoop.AssignJobWorkItem();

            return Ok(new
            {
                message = "Success: Job was successfully lodged and will begin generation shortly!",
                jobId = newJob.JDID
            });
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest();
        }
    }

    /// <summary>
    ///     Gets the status of a generation job
    /// </summary>
    /// <param name="jobId">The job id to check</param>
    /// <returns>Http status</returns>
    [HttpGet("status/job/{jobId}")]
    public async Task<ActionResult> GetJobStatusFromJobId(Guid jobId)
    {
        if (jobId == Guid.Empty)
        {
            logger.LogInformation("Error: No graphic ID provided with request");
            return BadRequest(new { message = "Error: No graphic ID provided within request" });
        }

        var job = await jobDetailsRepository.GetJobDetailsByIDAsync(jobId);

        if (job == null)
        {
            logger.LogInformation("Error: Jobs with ID: {jobId} was not found", jobId);
            return BadRequest(new { message = "Error: Job ID: " + jobId + " was not found" });
        }

        var pendingJob = await jobsPendingRepository.GetJobsPendingByDetailsId(jobId);

        if (pendingJob == null)
        {
            // Job has been found but is complete
            logger.LogInformation("Error: No pending in job queue with id {jobId} was found", job.JDID);
            return Ok(new
            {
                message = "Job found",
                jobId = job.JDID,
                status = "Complete"
            });
        }

        logger.LogInformation("Success: Job: {jobId} was found with status: {status} in position: {pos}", job.JDID,
            pendingJob.Status.ToString(), pendingJob.QueueNumber);
        return Ok(new
        {
            message = "Job found",
            jobId = job.JDID,
            status = pendingJob.Status.ToString(),
            queuePos = pendingJob.QueueNumber
        });
    }

    /// <summary>
    ///     Gets a generation job status from a graphic
    /// </summary>
    /// <param name="gpcid">The graphic id to check</param>
    /// <returns>http status</returns>
    [HttpGet("status/graphic/{gpcid}")]
    public async Task<ActionResult> GetJobStatusFromGraphicsId(Guid gpcid)
    {
        if (gpcid == Guid.Empty)
        {
            logger.LogInformation("Error: No graphic ID provided with request");
            return BadRequest(new { message = "Error: No graphic ID provided within request" });
        }

        var job = await jobDetailsRepository.GetJobDetailsByGraphicIdAsync(gpcid);

        if (job == null)
        {
            logger.LogInformation("Error: Jobs for graphic ID: {gpcid} was not found", gpcid);
            return BadRequest(new { message = "Error: Jobs for graphic ID: " + gpcid + " was not found" });
        }

        var pendingJob = await jobsPendingRepository.GetJobsPendingByDetailsId(job.JDID);

        if (pendingJob == null)
        {
            logger.LogInformation("Error: No pending in job queue with id {jobId} was found", job.JDID);
            return Ok(new
            {
                message = "Job found",
                jobId = job.JDID,
                status = "Complete"
            });
        }

        logger.LogInformation("Success: Job: {jobId} was found with status: {status} in position: {pos}", job.JDID,
            pendingJob.Status.ToString(), pendingJob.QueueNumber);
        return Ok(new
        {
            message = "Job found",
            jobId = job.JDID,
            status = pendingJob.Status.ToString(),
            queuePos = pendingJob.QueueNumber
        });
    }

    /// <summary>
    ///     Internal sub-class for request objects
    /// </summary>
    public class GenerationRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for an animal.
        /// </summary>
        public Guid? AnimalId { get; set; }

        /// <summary>
        /// The file name of the graphical representation of the animal.
        /// </summary>
        public string? AnimalGraphicFileName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for a graphic.
        /// </summary>
        public Guid? GraphicID { get; set; }

        /// <summary>
        /// Specifies the type of generation requested by the user
        /// </summary>
        public string? GenType { get; set; }
    }
}