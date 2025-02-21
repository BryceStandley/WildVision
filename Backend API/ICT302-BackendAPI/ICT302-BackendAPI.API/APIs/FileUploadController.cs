// FileUploadController.cs

using System.Globalization;
using System.Text.RegularExpressions;
using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ICT302_BackendAPI.API.Controllers;

/// <summary>
///     File upload endpoint controller to manage all files being uploaded by users
/// </summary>
[Route("api/upload")]
[ApiController]
public class FileUploadController(
    IConfiguration configuration,
    ILogger<FileUploadController> logger,
    IAnimalRepository animalRepository,
    IWebHostEnvironment webHostEnvironment,
    IGraphicRepository graphicRepository,
    IAnimalAccessRepository animalAccessRepository)
    : ControllerBase
{
    /// <summary>
    ///     Endpoint to upload a file to an animal that already exists in the db
    /// </summary>
    /// <param name="files">Files from the http post</param>
    /// <param name="animalID">Animal id from the http post</param>
    /// <param name="userId">User ID from the http post</param>
    /// <returns>Http status</returns>
    [HttpPost("existing")]
    public async Task<IActionResult> UploadFileForExistingAnimalByIdAsync(
        [FromForm] List<IFormFile> files,
        [FromForm] Guid animalID, // Fetch existing animal
        [FromForm] Guid userId)
    {
        try
        {
            var animal = await animalRepository.GetAnimalByIdAsync(animalID);
            if (animal == null) return NotFound(new { message = "Animal not found." });

            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                if (!IsFileTypeAllowed(fileExtension))
                    return BadRequest(new { message = $"File type {fileExtension} not supported." });

                // Save the file and create a Graphic entry for the existing animal
                var gpcid = Guid.NewGuid();
                var uniqueFileName = $"{gpcid}{fileExtension}";
                var filePath = Path.Combine(GetStoredFilesPath(), uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var graphic = new Graphic
                {
                    GPCID = gpcid,
                    AnimalID = animal.AnimalID, // Link graphic to existing animal
                    GPCName = $"{animal.AnimalName}_{animal.AnimalType}",
                    GPCDateUpload = DateTime.Now,
                    FilePath = uniqueFileName,
                    GPCSize = (int)file.Length,
                    Animal = animal
                };

                await graphicRepository.CreateGraphicAsync(graphic);
            }

            return Ok(new { message = "Files uploaded to existing animal successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading files for existing animal.");
            return StatusCode(500, new { message = "Internal server error." });
        }
    }

    /// <summary>
    ///     Endpoint to manage uploading media and creating a new animal
    /// </summary>
    /// <param name="files">Files from the http post</param>
    /// <param name="animalName">New animal name from the http post</param>
    /// <param name="animalType">New animal type from http post</param>
    /// <param name="dateOfBirth">New animal DOB from http post</param>
    /// <param name="userId">User ID from http post</param>
    /// <returns>Http status</returns>
    [HttpPost]
    public async Task<IActionResult> UploadFileAsync([FromForm] List<IFormFile> files, [FromForm] string animalName,
        [FromForm] string animalType, [FromForm] string dateOfBirth, [FromForm] Guid userId)
    {
        try
        {
            logger.LogInformation(
                $"Received file upload request: AnimalName = {animalName}, AnimalType = {animalType}, DateOfBirth = {dateOfBirth}");

            // Validate input fields
            if (files == null || files.Count == 0)
            {
                var msg = "Invalid request: No files provided.";
                logger.LogWarning(msg);
                return BadRequest(new { message = msg });
            }

            if (string.IsNullOrEmpty(animalName) || string.IsNullOrEmpty(animalType) ||
                string.IsNullOrEmpty(dateOfBirth))
            {
                var msg =
                    $"Invalid request: Missing animal details. AnimalName: {animalName}, AnimalType: {animalType}, DateOfBirth: {dateOfBirth}";
                logger.LogWarning(msg);
                return BadRequest(new { message = msg });
            }

            if (!DateTime.TryParseExact(dateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var parsedAnimalDOB))
            {
                var msg = $"Invalid date of birth format: {dateOfBirth}, Required format: yyyy-MM-dd";
                logger.LogWarning(msg);
                return BadRequest(new { message = msg });
            }

            if (userId == Guid.Empty)
            {
                var msg = $"Invalid ID , id : {userId}";
                logger.LogWarning(msg);
                return BadRequest(new { message = msg });
            }

            // Determine stored file path
            var storedFilesPath = GetStoredFilesPath();

            // Ensure directory exists
            if (!Directory.Exists(storedFilesPath)) Directory.CreateDirectory(storedFilesPath);

            // Create the animal entry
            var animal = new Animal
            {
                AnimalID = Guid.NewGuid(),
                AnimalName = animalName,
                AnimalType = animalType,
                AnimalDOB = parsedAnimalDOB
            };

            // Add the animal to the database before processing upload
            var a = await animalRepository.CreateAnimalAsync(animal);

            if (a != null && a.AnimalID == Guid.Empty)
            {
                var msg = "Failed to create animal in the database.";
                logger.LogError(msg);
                return StatusCode(500, new { message = msg });
            }

            // Process each file
            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                if (!IsFileTypeAllowed(fileExtension))
                {
                    var msg = $"Uploaded file: {file.FileName} is of a file type that is not supported";
                    logger.LogWarning(msg);
                    return BadRequest(new { message = msg });
                }

                // Generate a unique file name
                var gpcid = Guid.NewGuid();
                var uniqueFileName = $"{gpcid}{fileExtension}";
                var filePath = Path.Combine(storedFilesPath, uniqueFileName);

                // Save the file to disk
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Create a Graphic entry
                var graphic = new Graphic
                {
                    GPCID = gpcid,
                    AnimalID = animal.AnimalID,
                    GPCName = $"{animal.AnimalName}_{animal.AnimalType}",
                    GPCDateUpload = DateTime.Now,
                    FilePath = uniqueFileName,
                    GPCSize = (int)file.Length,
                    Animal = animal
                };

                await graphicRepository.CreateGraphicAsync(graphic);

                // Create AnimalAccess entry
                var access = new AnimalAccess
                {
                    AccessID = Guid.NewGuid(),
                    AnimalID = animal.AnimalID,
                    UserID = userId,
                    AccessType = "Default",
                    AssignedDate = DateTime.Now
                };

                await animalAccessRepository.CreateAnimalAccessAsync(access);
            }

            logger.LogInformation(
                $"AnimalAccess entry created successfully for AnimalID: {animal.AnimalID}, UserID: {userId}");
            return Ok(new { message = "Files uploaded and animal data saved successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred during file upload.");
            return StatusCode(500, new { message = "Internal server error during file upload." });
        }
    }

    /// <summary>
    ///     Endpoint to delete an animal and all associated (graphics, videos)
    /// </summary>
    /// <param name="animalId">Animal to delete</param>
    /// <returns>Http status</returns>
    [HttpDelete("animal/{animalId}")]
    public async Task<IActionResult> DeleteAnimalAsync(Guid animalId)
    {
        try
        {
            var success = await DeleteAnimal(animalId);
            if (!success) return NotFound(new { message = "Animal not found." });

            return Ok(new { message = "Animal and all associated data deleted successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting the animal.");
            return StatusCode(500, new { message = "Internal server error during animal deletion." });
        }
    }

    /// <summary>
    ///     Deletes a media file
    /// </summary>
    /// <param name="storedFilesPath">path the file is stored in on the host</param>
    /// <param name="videoFileName">file to delete on the host</param>
    /// <returns>true if successful or false if not</returns>
    private bool DeleteFile(string storedFilesPath, string videoFileName)
    {
        if (!string.IsNullOrEmpty(videoFileName))
        {
            var filePath = Path.Combine(storedFilesPath, videoFileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                logger.LogInformation("File successfully deleted: {FilePath}", filePath);
                return true;
            }

            logger.LogWarning("File not found: {FilePath}", filePath);
        }

        return false;
    }

    /// <summary>
    ///     Deletes an animal from the DB
    /// </summary>
    /// <param name="animalId">animal id to delete</param>
    /// <returns>true if successful or false if not</returns>
    private async Task<bool> DeleteAnimal(Guid animalId)
    {
        var animal = await animalRepository.GetAnimalByIdAsync(animalId);

        if (animal == null) return false; // Animal not found

        // Delete all associated video files
        var storedFilesPath = GetStoredFilesPath();

        //DeleteFile(storedFilesPath, animal.VideoFileName); This needs to be looked at


        // Delete the animal record from the database
        await animalRepository.DeleteAnimalAsync(animal);
        logger.LogInformation("Animal and associated videos deleted successfully: {AnimalID}", animalId);

        return true;
    }

    /// <summary>
    ///     Gets the path where files are stored within the appsettings file
    /// </summary>
    /// <returns>string of the stored files path</returns>
    /// <exception cref="InvalidOperationException">If stored files path is not found within the app settings</exception>
    private string GetStoredFilesPath()
    {
        var storedFilesPath = webHostEnvironment.IsDevelopment()
            ? configuration.GetValue<string>("dev_StoredFilesPath")
            : configuration.GetValue<string>("StoredFilesPath");

        if (string.IsNullOrEmpty(storedFilesPath))
        {
            logger.LogError("StoredFilesPath is not configured.");
            throw new InvalidOperationException("StoredFilesPath is not defined.");
        }

        return storedFilesPath;
    }

    /// <summary>
    ///     Checks if a file uploaded is allowed
    /// </summary>
    /// <param name="fileExtension">the files extension</param>
    /// <returns>true if allowed or false if not</returns>
    private bool IsFileTypeAllowed(string fileExtension)
    {
        var allowedExtensions = configuration.GetSection("AllowedFileUploadTypes").Get<string[]>();
        return allowedExtensions!.Contains(fileExtension.ToLowerInvariant());
    }

    /// <summary>
    ///     Sanitizes a files name
    /// </summary>
    /// <param name="fileName">File name to sanitize</param>
    /// <returns>Cleaned file name</returns>
    private string SanitizeFileName(string fileName)
    {
        // Remove any character that is not a letter, digit, underscore, or dash
        return Regex.Replace(fileName, "[^a-zA-Z0-9_-]", "");
    }

    /// <summary>
    ///     Endpoint to delete an animals graphic
    /// </summary>
    /// <param name="animalId">Animal's id</param>
    /// <param name="graphicId">graphic to delete</param>
    /// <returns>Http status</returns>
    [HttpDelete("animal/{animalId}/graphic/{graphicId}")]
    public async Task<IActionResult> DeleteGraphicAsync(Guid animalId, string graphicId)

    {
        try
        {
            // Fetch the animal from the database using the animalId
            var animal = await animalRepository.GetAnimalByIdAsync(animalId);
            if (animal == null) return NotFound(new { message = "Animal not found." });

            // Determine the stored file path
            var storedFilesPath = GetStoredFilesPath();

            // Delete the graphic 
            var fileDeleted = DeleteFile(storedFilesPath, graphicId);
            if (!fileDeleted) return NotFound(new { message = "Graphic file not found." });

            // Will need to change when the structure of animal changes

            return Ok(new { message = "Graphic deleted successfully." });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting the graphic.");
            return StatusCode(500, new { message = "Internal server error during graphic deletion." });
        }
    }
}