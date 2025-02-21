using System.Text.Json;
using System.Text.Json.Nodes;

namespace ICT302_Animals_Generator_API.Controllers;

/**
 * <summary>
 *     Defines the structure of data for input into the generator API.
 *     This class is used to store the info about a generation request from the backend
 * </summary>
 */
public class StartGenerationModel
{
    /// <summary>Default file path for generation that's inside the created job work folder</summary>
    public readonly string GenOutputLoc = "/gen";

    /// <summary>Default file path for images that's inside the created job work folder</summary>
    public readonly string ImageOutputLoc = "/images";

    /**
     * <summary>Default file path for masks that's inside the created job work folder</summary>
     * <remarks>This is unused and is left here as a stub</remarks>
     */
    public readonly string MaskOutputLoc = "/masks";

    /// <summary>Json holding vital information about the request</summary>
    public StartGenerationJson? StartGenerationJson { get; set; }

    /// <summary>Input file to the generator </summary>
    public IFormFile? InputFile { get; set; }
}

/**
 * <summary>This class defines the structure of data that is expected within an HTTP post request to the generator</summary>
 */
public class StartGenerationJson
{
    /// <summary>Default constructor to nulls</summary>
    public StartGenerationJson()
    {
        Token = null;
        JobID = null;
        FileName = null;
        OutputPath = null;
        SubjectHint = null;
        ModelPath = null;
        ModelVersion = null;
    }

    /**
     * <summary>Default constructor for the class</summary>
     * <param name="token">String auth token from the HTTP post request</param>
     */
    public StartGenerationJson(string token)
    {
        Token = token;
        JobID = null;
        FileName = null;
        OutputPath = null;
        SubjectHint = null;
        ModelPath = null;
        ModelVersion = null;
    }

    /**
     * <summary>Default constructor for the class</summary>
     * <param name="token">String auth token from the HTTP post request</param>
     * <param name="jobId">Guid of the job</param>
     * <param name="fileName">String file name of the file being uploaded to the generator</param>
     */
    public StartGenerationJson(string token, Guid jobId, string fileName)
    {
        Token = token;
        JobID = jobId;
        FileName = fileName;
    }


    /// <summary>Auth token of the request</summary>
    public string? Token { get; set; }

    /// <summary>Job ID of the request</summary>
    public Guid? JobID { get; set; }

    /// <summary>File name of the media file in the request</summary>
    public string? FileName { get; set; }

    /// <summary>Output path used for generation</summary>
    public string? OutputPath { get; set; }

    /**
     * <summary>Subject hint used for masking tasks</summary>
     * <remarks>This is un-used and is left here as a stub. Intended to be the animal type of the animal being generated</remarks>
     */
    public string? SubjectHint { get; set; }

    /// <summary>Model path used for generation output</summary>
    public string? ModelPath { get; set; }

    /**
     * <summary>Model version used for generation</summary>
     * <remarks>This is un-used and left here as a stub. Intended for generating new models of the same animal with new media</remarks>
     */
    public int? ModelVersion { get; set; }
}

/**
 * <summary>Json converter class to help convert HTTP post request json into the correct object values</summary>
 */
public class StartGenerationJsonConverter
{
    /**
     * <summary>Converts Json string into <see cref="StartGenerationJson" /></summary>
     * <param name="json">Json node of the HTTP post request containing Json strings</param>
     * <returns>
     *     <see cref="StartGenerationJson" />
     * </returns>
     */
    public static StartGenerationJson FromJson(JsonNode? json)
    {
        if (json == null)
            return new StartGenerationJson();

        var token = json["token"]?.ToString();
        var sguid = json["jobID"]?.ToString();
        var file = json["fileName"]?.ToString();

        if (string.IsNullOrEmpty(token))
            return new StartGenerationJson();

        if (string.IsNullOrEmpty(sguid))
            return new StartGenerationJson();

        if (string.IsNullOrEmpty(file))
            return new StartGenerationJson();

        return new StartGenerationJson(token, Guid.Parse(sguid), file);
    }

    /**
     * <summary>Gets <see cref="StartGenerationJson" /> from a HTTP post form Json collection</summary>
     * <param name="form">HTTP form containing Json strings</param>
     * <returns><see cref="StartGenerationJson" /> or null</returns>
     */
    public static StartGenerationJson? GetFromJson(IFormCollection form)
    {
        try
        {
            if (form.ContainsKey("StartGenerationJson"))
            {
                var values = new StartGenerationJson();
                var jsonData = form["StartGenerationJson"].ToString();

                var jsonDoc = JsonDocument.Parse(jsonData);

                if (jsonDoc.RootElement.TryGetProperty("token", out var tokenVal))
                {
                    Console.WriteLine("Token provided for the request");
                    values.Token = tokenVal.GetString();
                }

                if (jsonDoc.RootElement.TryGetProperty("jobId", out var jobIdVal))
                    values.JobID = Guid.Parse(jobIdVal.GetString()!);
                if (jsonDoc.RootElement.TryGetProperty("fileName", out var fileNameVal))
                    values.FileName = fileNameVal.GetString();
                if (jsonDoc.RootElement.TryGetProperty("subjectHint", out var subjectHintVal))
                    values.SubjectHint = subjectHintVal.GetString();
                if (jsonDoc.RootElement.TryGetProperty("modelPath", out var modelPathVal))
                    values.ModelPath = modelPathVal.GetString();
                if (jsonDoc.RootElement.TryGetProperty("modelVersion", out var modelVerVal))
                    values.ModelVersion = modelVerVal.GetInt32();
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e.Message);
            return null;
        }
    }
}