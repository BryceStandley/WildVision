using System.Text.Json.Nodes;
using ICT302_Animals_Generator_API.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace ICT302_Animals_Generator_API.Controllers;

/**
 * <summary>Generation File API endpoint controller used to return a generated model</summary>
 */
public class GenFileController(
    ILogger<GenerationController> logger,
    IConfiguration configuration,
    SecurityMaster securityMaster)
    : ControllerBase
{
    /**
     * <summary>Gets the generated glb 3D model from the generation Async</summary>
     * <param name="model">The <see cref="StartGenerationModel" /> from the request information from the backend</param>
     * <returns>HTTP status and the 3D model file if found</returns>
     */
    [HttpPost("/api/gen/files")]
    public ActionResult GetGeneratedFileAsync([FromForm] StartGenerationModel? model)
    {
        try
        {
            if (model == null)
                return StatusCode(500, new { message = "Internal server error while fetching file." });

            model.StartGenerationJson = GetFromJson();

            if (string.IsNullOrEmpty(model.StartGenerationJson.Token) ||
                securityMaster.IsRequestAuthorized(model.StartGenerationJson.Token) == StatusCodes.Status418ImATeapot)
                return StatusCode(401, new { message = "Unauthorized" });

            var jobFolder = "job_" + model.StartGenerationJson.JobID;

            var jobFile = model.StartGenerationJson.JobID + ".glb";

            var output = configuration.GetValue<string>("AppOutputRoot");
            var user = configuration.GetValue<string>("SystemUser");
            var userHome = Path.Join("/home", user);
            var outPath = Path.Join(userHome, output, jobFolder, jobFile);


            if (System.IO.File.Exists(outPath))
            {
                logger.LogInformation("Requested file found: {FilePath}", outPath);
                var mimeType = GetMimeType(outPath);
                var fileStream = new FileStream(outPath, FileMode.Open, FileAccess.Read);
                return File(fileStream, mimeType, true);
            }

            logger.LogWarning("Requested file not found: {FilePath}", outPath);
            return NotFound(new { message = "File not found." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while serving file: {FileName}", model!.StartGenerationJson!.FileName);
            return StatusCode(500, new { message = "Internal server error while fetching file." });
        }
    }

    /**
     * <summary>Gets the request info from the HttpContext of the object</summary>
     * <returns><see cref="StartGenerationJson" /> of the request</returns>
     */
    private StartGenerationJson GetFromJson()
    {
        StringValues data;
        HttpContext.Request.Form.TryGetValue("StartGenerationJson", out data);
        var j = JsonNode.Parse(data!);
        var jj = StartGenerationJsonConverter.FromJson(j);
        return jj;
    }

    /**
     * <summary>Gets the Mime type of file based on its file extension</summary>
     * <param name="filePath">String of the file with its extension</param>
     * <returns>String in the Mime format based on the file</returns>
     * <example>Video file with .mp4 would return video/mp4 as the Mime type</example>
     */
    private string GetMimeType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".mp4" => "video/mp4",
            ".mov" => "video/quicktime",
            ".avi" => "video/x-msvideo",
            ".mkv" => "video/x-matroska",
            ".glb" => "model/gltf-binary",
            ".jpg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            _ => "application/octet-stream"
        };
    }
}