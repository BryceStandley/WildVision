namespace ICT302_BackendAPI.Database.Models;

/// <summary>
/// Indicates that the job is undergoing initial data processing and preparation before main execution steps.
/// </summary>
public enum JobStatus
{
    /// <summary>
    /// Indicates that the job has been submitted
    /// </summary>
    Submitted,

    /// <summary>
    /// Indicates that the job is currently being validated for correctness and completeness.
    /// </summary>
    Validating,

    /// <summary>
    /// Indicates that the job is undergoing initial data processing and preparation before main execution steps.
    /// </summary>
    PreProcessing,
    /// <summary>
    /// Indicates that the job is currently being evaluated for correctness and completeness.
    /// </summary>
    Evaluating,
    /// <summary>
    ///     Indicates that the job is currently being masked to protect sensitive data.
    /// </summary>
    Masking,
    /// <summary>
    ///    Indicates that the job is currently generating data.
    /// </summary>
    Generating,
    /// <summary>
    ///    Indicates that the job is currently converting data to a different format.
    /// </summary>
    Converting,
    /// <summary>
    ///   Indicates that the job is currently cleaning up after processing.
    /// </summary>
    CleaningUp,
    /// <summary>
    ///     Indicates that the job has finished processing.
    /// </summary>
    Finished,
    /// <summary>
    ///    Indicates that the job is currently fetching files.
    /// </summary>
    Fetching,
    /// <summary>
    ///    Indicates that the job is currently closing.
    /// </summary>
    Closing,
    /// <summary>
    ///   Indicates that the job has been closed.
    /// </summary>
    Closed,
    /// <summary>
    ///    Indicates that the job has encountered an error.
    /// </summary>
    Error
}

/// <summary>
///     A helper class for managing job statuses.
/// </summary>
public static class JobStatusHelper
{
    /**
     * <summary>Gets the job status from a string</summary>
     * <param name="status">the job status string</param>
     * <returns>The JobStatus</returns>
     */
    public static JobStatus GetJobStatus(string status)
    {
        switch (status)
        {
            case "Submitted":
                return JobStatus.Submitted;
            case "Validating":
                return JobStatus.Validating;
            case "PreProcessing":
                return JobStatus.PreProcessing;
            case "Evaluating":
                return JobStatus.Evaluating;
            case "Masking":
                return JobStatus.Masking;
            case "Generating":
                return JobStatus.Generating;
            case "Converting":
                return JobStatus.Converting;
            case "CleaningUp":
                return JobStatus.CleaningUp;
            case "Finished":
                return JobStatus.Finished;
            case "Fetching":
                return JobStatus.Fetching;
            case "Closing":
                return JobStatus.Closing;
            case "Error":
                return JobStatus.Error;
            case "Closed":
                return JobStatus.Closed;
            default:
                return JobStatus.Error;
        }
    }

    /**
     * <summary>Gets the job status as a string</summary>
     * <param name="status">the job status value</param>
     * <returns>The status string</returns>
     */
    public static string GetJobStatusString<TJobStatus>(TJobStatus status) where TJobStatus : Enum
    {
        return status.ToString();
    }

    /**
     * <summary>Gets the api endpoint address for a job status</summary>
     * <param name="status">the job status value</param>
     * <returns>The endpoint address string</returns>
     */
    public static string GetJobStatusEndpoint<TJobStatus>(TJobStatus status) where TJobStatus : Enum
    {
        switch (status)
        {
            case JobStatus.Submitted:
                return "";
            case JobStatus.Validating:
                return "/api/gen/preprocess/validate";
            case JobStatus.PreProcessing:
                return "/api/gen/preprocess/split";
            case JobStatus.Evaluating:
                return "/api/gen/preprocess/evaluate";
            case JobStatus.Masking:
                return "/api/gen/preprocess/mask";
            case JobStatus.Generating:
                return "/api/gen/generate/bite"; //TODO: Possibly add check for different generator options here? 
            case JobStatus.Converting:
                return "/api/gen/postprocess/convert";
            case JobStatus.CleaningUp:
                return "/api/gen/postprocess/cleanup";
            case JobStatus.Finished:
                return "";
            case JobStatus.Fetching:
                return "/api/gen/files";
            case JobStatus.Closing:
                return "";
            case JobStatus.Closed:
                return "";
            case JobStatus.Error:
                return "";
            default:
                return "";
        }
    }

    /**
     * <summary>Gets the next status from the current job status</summary>
     * <param name="status">the job status value</param>
     * <returns>The next job status</returns>
     */
    public static JobStatus GetNextJobStatusFromPrevious<TJobStatus>(TJobStatus status) where TJobStatus : Enum
    {
        switch (status)
        {
            case JobStatus.Submitted:
                return JobStatus.Validating;
            case JobStatus.Validating:
                return JobStatus.PreProcessing;
            case JobStatus.PreProcessing:
                return JobStatus.Generating;
            case JobStatus.Evaluating: // TODO: Add functionality to GenAPI for Eval. Currently Stubbed
                return JobStatus.Masking;
            case JobStatus.Masking
                : // TODO: Add more functionality to GenAPI for Masking. Currently functional but not needed for BITE
                return JobStatus.Generating;
            case JobStatus.Generating:
                return JobStatus.Converting;
            case JobStatus.Converting:
                return JobStatus.CleaningUp;
            case JobStatus.CleaningUp:
                return JobStatus.Finished;
            case JobStatus.Finished:
                return JobStatus.Fetching;
            case JobStatus.Fetching:
                return JobStatus.Closing;
            case JobStatus.Closing:
                return JobStatus.Closed;
            case JobStatus.Error:
                return JobStatus.Closed;
            default:
                return JobStatus.Error;
        }
    }

    /**
     * <summary>Checks if the job status is in processing status's</summary>
     * <param name="status">the job status value</param>
     * <returns>
     *     True if in Submitted, Validating, PreProcessing, Evaluating, Masking, Generating, Converting or CleaningUp
     *     Status, else False
     * </returns>
     */
    public static bool IsJobInProcessingStaus(JobStatus status)
    {
        switch (status)
        {
            case JobStatus.Submitted:
            case JobStatus.Validating:
            case JobStatus.PreProcessing:
            case JobStatus.Evaluating:
            case JobStatus.Masking:
            case JobStatus.Generating:
            case JobStatus.Converting:
            case JobStatus.CleaningUp:
                return true;
            case JobStatus.Finished:
            case JobStatus.Closing:
            case JobStatus.Closed:
            case JobStatus.Error:
                return false;
            default:
                return false;
        }
    }
}