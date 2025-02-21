using System.Diagnostics;
using System.Text.Json.Nodes;
using Aspose.ThreeD;
using Aspose.ThreeD.Utilities;
using ICT302_Animals_Generator_API.Util;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_Animals_Generator_API.Controllers;

/**
 * <summary>The main API controller for API endpoints related to generation and the steps of generation</summary>
 */
[ApiController]
[Route("api/gen")]
public class GenerationController(
    ILogger<GenerationController> logger,
    IConfiguration configuration,
    SecurityMaster securityMaster,
    IWebHostEnvironment webHostEnvironment)
    : ControllerBase
{
    /// <summary>Application configuration</summary>
    private readonly IConfiguration _configuration = configuration;

    /// <summary>Logger to log to the console and log file</summary>
    private readonly ILogger<GenerationController> _logger = logger;

    /// <summary>Application security master</summary>
    private readonly SecurityMaster _securityMaster = securityMaster;

    /// <summary>Application web host environment</summary>
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    /**
     * <summary>Checks for a valid request and if the data in the request is valid</summary>
     * <param name="model"><see cref="StartGenerationModel" /> of the request</param>
     * <returns>HTTP status result</returns>
     */
    private ObjectResult CheckForValidRequestData(StartGenerationModel? model)
    {
        // Null Check
        if (model == null)
        {
            _logger.LogInformation("Error: Unauthorized token provided.");
            return StatusCode(StatusCodes.Status418ImATeapot, new
            {
                statusCode = 418,
                jobStatus = "Unauthorized",
                message = "Error: Unauthorized token provided for request"
            });
        }

        model.StartGenerationJson = GetFromJson();

        // Auth Check
        if (string.IsNullOrEmpty(model.StartGenerationJson.Token) ||
            _securityMaster.IsRequestAuthorized(model.StartGenerationJson.Token) == StatusCodes.Status418ImATeapot)
        {
            _logger.LogInformation("Error: Unauthorized token provided.");
            return StatusCode(StatusCodes.Status418ImATeapot, new
            {
                statusCode = 418,
                jobStatus = "Unauthorized",
                message = "Error: Unauthorized token provided for request"
            });
        }

        //FilePath check
        if (string.IsNullOrEmpty(model.StartGenerationJson.FileName))
        {
            _logger.LogInformation("Error: No details provided for generation.");
            return StatusCode(StatusCodes.Status400BadRequest, new
            {
                statusCode = 400,
                jobStatus = "Failed",
                message = "Error: No FilePath provided"
            });
        }

        // JobID Check
        if (model.StartGenerationJson.JobID == Guid.Empty)
        {
            _logger.LogInformation("Error: No JobID provided");
            return StatusCode(StatusCodes.Status400BadRequest, new
            {
                statusCode = 400,
                jobStatus = "Failed",
                message = "Error: No JobID provided"
            });
        }

        // Checks passed
        return StatusCode(StatusCodes.Status200OK, new
        {
            statusCode = 200,
            jobStatus = "Success",
            message = "Success: All required data is provided"
        });
    }

    /**
     * <summary>Main generation endpoint for the Bite generator. Non-blocking async</summary>
     * <param name="model"><see cref="StartGenerationModel" /> of the request</param>
     * <returns>HTTP status result</returns>
     */
    [HttpPost("generate/bite")]
    public async Task<ActionResult> StartGenerationAsync([FromForm] StartGenerationModel? model)
    {
        try
        {
            var checks = CheckForValidRequestData(model);
            if (checks.StatusCode == StatusCodes.Status418ImATeapot)
                return CheckForValidRequestData(model);


            if (Directory.Exists(Path.Join(GetOutputPath(model!.StartGenerationJson!.JobID!), model.GenOutputLoc)) &&
                Directory.GetFiles(Path.Join(GetOutputPath(model.StartGenerationJson.JobID!), model.GenOutputLoc))
                    .Length > 0)
            {
                // Output file full, masks already exist
                _logger.LogInformation(
                    $"Generation for job {model.StartGenerationJson.JobID} has completed successfully.");
                return StatusCode(StatusCodes.Status200OK, new
                {
                    statusCode = 200,
                    jobStatus = "Completed Generation",
                    message = "Success: 3D model generation has completed successfully."
                });
            }

            if (string.IsNullOrEmpty(model?.StartGenerationJson.OutputPath))
                model!.StartGenerationJson.OutputPath = GetOutputPath(model.StartGenerationJson.JobID!);

            _logger.LogInformation($"Starting 3D Generation for job: {model!.StartGenerationJson.JobID}");
            Directory.CreateDirectory(model.StartGenerationJson.OutputPath + model.GenOutputLoc);

            // Enable or disable in config for debug testing
            var genEnabled = _configuration.GetValue<bool>("3DGenEnabled");
            if (!genEnabled)
            {
                _logger.LogInformation($"3D generation for {model.StartGenerationJson.JobID} is disabled");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    statusCode = 500,
                    jobStatus = "Generation Disabled",
                    message = "Success: Model has been generated successfully."
                });
            }

            //move frame to image input
            var systemUser = _configuration.GetValue<string>("SystemUser") ?? null;
            var userHomePath = Path.Join("/home", systemUser);
            var genImageInputLocation = _configuration.GetValue<string>("Generator:ImageInputPath");
            var inputFramePath = Path.Join(model.StartGenerationJson.OutputPath + model.ImageOutputLoc, "0001.png");
            var outputFramePath = Path.Join(userHomePath, genImageInputLocation, "0001.png");
            _logger.LogInformation("Moving image frame from {0} to {1}", inputFramePath, outputFramePath);
            System.IO.File.Copy(inputFramePath, outputFramePath, true);

            _logger.LogInformation($"Starting 3D generation for job: {model.StartGenerationJson.JobID}");
            Directory.CreateDirectory(model.StartGenerationJson.OutputPath + model.GenOutputLoc);

            string? args = null;
            string? workingDir = null;

            // Linux Only due to python/cuda comapt issues
            var genRoot = _configuration.GetValue<string>("Generator:Root") ?? null;
            if (!string.IsNullOrEmpty(genRoot))
            {
                args = CreateGenerationStartCommand(model.StartGenerationJson.JobID);
                workingDir = genRoot;
            }
            

            if (string.IsNullOrEmpty(args))
                throw new Exception(
                    "Error in creating python arguments from config. Are the required arguments in the config file?");
            
            var startInfo = new ProcessStartInfo($"{userHomePath}anaconda3/bin/conda", args)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDir
            };

            var proc = Process.Start(startInfo);

            try
            {
                await proc?.WaitForExitAsync()!;

                if (proc.HasExited)
                {
                    _logger.LogInformation($"Process exited with code {proc.ExitCode}");

                    if (proc.ExitCode == 0)
                    {
                        var genOutputLocation = _configuration.GetValue<string>("Generator:ModelOutputPath") ?? null;
                        var fullOutputPath = Path.Join(genRoot, genOutputLocation);
                        var genFolderPrefix = _configuration.GetValue<string>("Generator:OutputFolderPrefix");
                        var genFolderName = genFolderPrefix + model.StartGenerationJson.JobID;
                        var genOutputFile = _configuration.GetValue<string>("Generator:OutputFileName");
                        var inputModelFile = Path.Join(fullOutputPath, genFolderName, genOutputFile);
                        var outputModelFile = Path.Join(model.StartGenerationJson.OutputPath + model.GenOutputLoc,
                            "bite_output.obj");
                        _logger.LogInformation("Moving generated model from {in} to {out}", inputModelFile,
                            outputModelFile);
                        System.IO.File.Copy(inputModelFile, outputModelFile, true);
                        
                        //remove frame from image input
                        _logger.LogInformation("Cleaning generator image inputs in path {path}", outputFramePath);
                        System.IO.File.Delete(outputFramePath);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("There was an error when starting generator with error: {err}", e.Message);
                throw;
            }

            _logger.LogInformation($"3D generation for {model.StartGenerationJson.JobID} has completed successfully.");
            return StatusCode(StatusCodes.Status200OK, new
            {
                statusCode = 200,
                jobStatus = "Completed Generation",
                message = "Success: Model has been generated successfully."
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                jobStatus = "Failed Generation",
                message = $"Error: {e.Message}"
            });
        }
    }


    /// <summary>Creates a generation start command with the necessary arguments from configuration settings</summary>
    /// <param name="jobId">The job ID to be included in the command</param>
    /// <returns>A string representing the full command to start the generation process</returns>
    /// <exception cref="Exception">Thrown when a required configuration setting is missing</exception>
    private string? CreateGenerationStartCommand(Guid? jobId)
    {
        if (jobId == null)
            throw new Exception("Error in creating python arguments from config. JobID not provided.");
        
        var terminal = _configuration.GetValue<string>("Generator:Terminal") ?? null;
        var systemUser = _configuration.GetValue<string>("SystemUser") ?? null;
        var condaEnv = _configuration.GetValue<string>("Generator:CondaEnv") ?? null;
        var scriptRoot = _configuration.GetValue<string>("Generator:ScriptRoot") ?? null;
        var script = _configuration.GetValue<string>("Generator:Script") ?? null;
        var scriptArgs = _configuration.GetValue<string>("Generator:ScriptArgs") ?? null;
        var genRoot = _configuration.GetValue<string>("Generator:Root") ?? null;

        if (condaEnv == null || genRoot == null || systemUser == null || terminal == null || scriptRoot == null || script == null || scriptArgs == null)
        {
            throw new Exception(
                "Error in creating python arguments from config. Are the required arguments in the config file?");
        }

        var userHomePath = Path.Join("/home", systemUser);
        var fullScriptPath = Path.Join(genRoot, scriptRoot, script);
        
        var command = $"{fullScriptPath} {scriptArgs}";
        var fullCmd = $" run -n {condaEnv} python {command} {jobId}";
        
        return fullCmd;
    }

    /**
     * <summary>Convert post process endpoint to convert from obj files from Bite into web friendly glb file format</summary>
     * <param name="model"><see cref="StartGenerationModel" /> of the request</param>
     * <returns>HTTP status result</returns>
     */
    [HttpPost("postprocess/convert")]
    public ActionResult StartGlbConversionAsync([FromForm] StartGenerationModel? model)
    {
        try
        {
            var checks = CheckForValidRequestData(model);
            if (checks.StatusCode == StatusCodes.Status418ImATeapot)
                return CheckForValidRequestData(model);

            if (string.IsNullOrEmpty(model?.StartGenerationJson!.OutputPath))
                if (model?.StartGenerationJson!.FileName != null)
                    model!.StartGenerationJson.OutputPath = GetOutputPath(model.StartGenerationJson.JobID);

            _logger.LogInformation($"Starting GLB conversion for job: {model!.StartGenerationJson!.JobID}");


            // Enable or disable in config for debug testing
            var convertEnabled = _configuration.GetValue<bool>("GLBConvertEnabled");
            if (!convertEnabled)
            {
                _logger.LogInformation($"GLB conversion for job: {model.StartGenerationJson.JobID} is disabled");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    statusCode = 500,
                    jobStatus = "Not Converted",
                    message = "Success: GLB conversion completed successfully"
                });
            }

            model.StartGenerationJson.OutputPath = GetOutputPath(model.StartGenerationJson.JobID);
            var scene = Scene.FromFile(Path.Join(model!.StartGenerationJson.OutputPath + model.GenOutputLoc,
                "bite_output.obj"));
            scene.RootNode.Transform.Scaling = new Vector3(10f, 10f, 10f);
            var root = model.StartGenerationJson.OutputPath;
            scene.Save(Path.Join(root, model.StartGenerationJson.JobID + ".glb"));

            _logger.LogInformation(
                $"GLB conversion for job: {model.StartGenerationJson.JobID} has completed successfully.");
            return StatusCode(StatusCodes.Status200OK, new
            {
                statusCode = 200,
                jobStatus = "Completed Conversion",
                message = "Success: GLB conversion completed successfully"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                jobStatus = "Failed Frame Splitting",
                message = "Error: There was an error when splitting the frames of the input file."
            });
        }
    }

    /**
     * <summary>Clean up post process endpoint to clean up any generated files that aren't required to store after generation</summary>
     * <param name="model"><see cref="StartGenerationModel" /> of the request</param>
     * <returns>HTTP status result</returns>
     */
    [HttpPost("postprocess/cleanup")]
    public ActionResult StartCleanUpAsync([FromForm] StartGenerationModel? model)
    {
        try
        {
            var checks = CheckForValidRequestData(model);
            if (checks.StatusCode == StatusCodes.Status418ImATeapot)
                return CheckForValidRequestData(model);

            if (string.IsNullOrEmpty(model?.StartGenerationJson!.OutputPath))
                if (model?.StartGenerationJson!.FileName != null)
                    model!.StartGenerationJson.OutputPath = GetOutputPath(model.StartGenerationJson.JobID);

            _logger.LogInformation($"Starting file clean up for job: {model?.StartGenerationJson!.JobID}");

            //Leave the output folder on the system with just the model file
            if (Directory.Exists(Path.Join(model!.StartGenerationJson!.OutputPath, model.ImageOutputLoc)))
                Directory.Delete(Path.Join(model!.StartGenerationJson.OutputPath, model.ImageOutputLoc), true);

            if (Directory.Exists(Path.Join(model!.StartGenerationJson.OutputPath, model.MaskOutputLoc)))
                Directory.Delete(Path.Join(model!.StartGenerationJson.OutputPath, model.MaskOutputLoc), true);

            if (Directory.Exists(Path.Join(model!.StartGenerationJson.OutputPath, model.GenOutputLoc)))
                Directory.Delete(Path.Join(model!.StartGenerationJson.OutputPath, model.GenOutputLoc), true);

            if (Path.Exists(Path.Join(model!.StartGenerationJson.OutputPath, model.StartGenerationJson.FileName)))
                System.IO.File.Delete(Path.Join(model!.StartGenerationJson.OutputPath,
                    model.StartGenerationJson.FileName));

            _logger.LogInformation(
                $"File clean up for job: {model.StartGenerationJson.JobID} has completed successfully.");
            return StatusCode(StatusCodes.Status200OK, new
            {
                statusCode = 200,
                jobStatus = "Completed CleanUp",
                message = "Success: File clean up completed successfully"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                jobStatus = "Failed Frame Splitting",
                message = "Error: There was an error when splitting the frames of the input file."
            });
        }
    }

    /**
     * <summary> Split pre-process endpoint for breaking up the input video into frame images</summary>
     * <param name="model"><see cref="StartGenerationModel" /> of the request</param>
     * <returns>HTTP status result</returns>
     */
    [HttpPost("preprocess/split")]
    public async Task<ActionResult> StartFrameSplittingAsync([FromForm] StartGenerationModel? model)
    {
        try
        {
            var checks = CheckForValidRequestData(model);
            if (checks.StatusCode == StatusCodes.Status418ImATeapot)
                return CheckForValidRequestData(model);

            if (model!.StartGenerationJson!.FileName != null &&
                Directory.Exists(Path.Join(GetOutputPath(model.StartGenerationJson.JobID), model.ImageOutputLoc)) &&
                Directory.GetFiles(Path.Join(GetOutputPath(model.StartGenerationJson.JobID), model.ImageOutputLoc))
                    .Length > 0)
            {
                // Output folder full, images already exist
                _logger.LogInformation(
                    $"Frame splitting for {model.StartGenerationJson.JobID} has completed successfully.");
                return StatusCode(StatusCodes.Status200OK, new
                {
                    statusCode = 200,
                    jobStatus = "Completed Splitting",
                    message = "Success: Frames have been split."
                });
            }

            if (string.IsNullOrEmpty(model?.StartGenerationJson.OutputPath))
                if (model?.StartGenerationJson.FileName != null)
                    model!.StartGenerationJson.OutputPath = GetOutputPath(model.StartGenerationJson.JobID);

            _logger.LogInformation($"Starting frame splitting for {model!.StartGenerationJson.JobID}");
            Directory.CreateDirectory(model!.StartGenerationJson.OutputPath + model.ImageOutputLoc);

            var ffmpeg = _configuration.GetValue<string>("Ffmpeg_bin_path");

            var outFormat = "/%0004d.png";
            var startInfo = new ProcessStartInfo(ffmpeg!,
                $"-i {model!.StartGenerationJson.OutputPath + "/" + model.StartGenerationJson.FileName} -filter:v fps=1 {model!.StartGenerationJson.OutputPath + model.ImageOutputLoc + outFormat}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var proc = Process.Start(startInfo);
            if (proc != null) await proc.WaitForExitAsync().ConfigureAwait(false);

            _logger.LogInformation(
                $"Frame splitting for {model.StartGenerationJson.JobID} has completed successfully.");
            return StatusCode(StatusCodes.Status200OK, new
            {
                statusCode = 200,
                jobStatus = "Completed Splitting",
                message = "Success: Frames have been split."
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                jobStatus = "Failed Frame Splitting",
                message = "Error: There was an error when splitting the frames of the input file."
            });
        }
    }

    /**
     * <summary>
     *     Evaluate pre-process endpoint to evaluate frames for issues like all black frames or frames not containing any
     *     animals or bad quality frames
     * </summary>
     * <param name="model"><see cref="StartGenerationModel" /> of the request</param>
     * <returns>HTTP status result</returns>
     * <remarks>This is unused and is left as a code stub</remarks>
     */
    [HttpPost("preprocess/evaluate")]
    private ActionResult StartFrameEvaluationAsync([FromForm] StartGenerationModel? model)
    {
        //TODO: Implement frame eval. This is a code stub and is ready for functionality to be added.

        var checks = CheckForValidRequestData(model);
        if (checks.StatusCode == StatusCodes.Status418ImATeapot)
            return checks;


        _logger.LogInformation($"Frame evaluation for {model!.StartGenerationJson!.JobID} has completed successfully.");
        return StatusCode(StatusCodes.Status200OK, new
        {
            statusCode = 200,
            jobStatus = "Completed Evaluation",
            selectedFrames = new[] { 1 },
            message = "Success: Frame evaluation completed successfully"
        });
    }

    /**
     * <summary>Masking pre-process endpoint to mask frames from the input video</summary>
     * <remarks>This is unused and is left here as a untested and un-debugged code stub</remarks>
     * <param name="model"><see cref="StartGenerationModel" /> of the request</param>
     * <returns>HTTP status result</returns>
     */
    [HttpPost("preprocess/mask")]
    public ActionResult StartFrameMasking([FromForm] StartGenerationModel? model)
    {
        try
        {
            var checks = CheckForValidRequestData(model);
            if (checks.StatusCode == StatusCodes.Status418ImATeapot)
                return checks;

            // This functionality is disabled as its unused. This is a code stub and is ready for functionality to be added. and tested
            // Note below code is outdated and wont work without modification for config values etc
            return new ObjectResult(StatusCodes.Status418ImATeapot);
            
            
            if (model!.StartGenerationJson!.FileName != null &&
                Directory.Exists(Path.Join(GetOutputPath(model.StartGenerationJson.JobID), model.MaskOutputLoc)) &&
                Directory.GetFiles(Path.Join(GetOutputPath(model.StartGenerationJson.JobID), model.MaskOutputLoc))
                    .Length > 0)
            {
                // Output file full, masks already exist
                _logger.LogInformation(
                    $"Frame masking for {model.StartGenerationJson.JobID} has completed successfully.");
                return StatusCode(StatusCodes.Status200OK, new
                {
                    statusCode = 200,
                    jobStatus = "Completed Masking",
                    message = "Success: Frames have been masked."
                });
            }

            if (string.IsNullOrEmpty(model?.StartGenerationJson.OutputPath))
                if (model?.StartGenerationJson.FileName != null)
                    model!.StartGenerationJson.OutputPath = GetOutputPath(model.StartGenerationJson.JobID);

            if (string.IsNullOrEmpty(model?.StartGenerationJson.SubjectHint))
                model!.StartGenerationJson.SubjectHint = "";

            _logger.LogInformation($"Starting frame masking for job: {model!.StartGenerationJson.JobID}");
            Directory.CreateDirectory(model.StartGenerationJson.OutputPath + model.MaskOutputLoc);

            var wslUser = _configuration.GetValue<string>("WslUser");
            var wslScriptPath = _configuration.GetValue<string>("WslScriptPath");
            var wslCondaEnv = _configuration.GetValue<string>("WslCondaEnv");
            var wslCmd = _configuration.GetValue<string>("WslStartCmd");

            var args =
                $"-d Ubuntu-20.04 -u {wslUser} sh -c \"cd \'{wslScriptPath!}\' && . ~/.bashrc && ~/anaconda3/bin/conda run -n {wslCondaEnv!} python ./gen_masks.py -p \'{GetWslPathFromWindowsPath(model.StartGenerationJson.OutputPath!)}\' -m {model.StartGenerationJson.SubjectHint}\"";
            _logger.LogInformation($"args: {args}");
            var startInfo = new ProcessStartInfo(wslCmd!, args);

            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;

            var proc = Process.Start(startInfo);
            proc?.WaitForExit();

            _logger.LogInformation(
                $"Frame masking for job: {model.StartGenerationJson.JobID} has completed successfully.");
            return StatusCode(StatusCodes.Status200OK, new
            {
                statusCode = 200,
                jobStatus = "Completed Masking",
                message = "Success: Frames have been masked."
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                jobStatus = "Failed Frame Masking",
                message = "Error: There was an error when masking the frames of the input file."
            });
        }
    }

    /**
     * <summary>
     *     Validation pre-process endpoint to validate the input media is found and to create the required output job
     *     directory
     * </summary>
     * <param name="model"><see cref="StartGenerationModel" /> of the request</param>
     * <returns>HTTP status result</returns>
     */
    [HttpPost("preprocess/validate")]
    public async Task<ActionResult> ValidFileCheck([FromForm] StartGenerationModel? model)
    {
        try
        {
            var checks = CheckForValidRequestData(model);
            if (checks.StatusCode == StatusCodes.Status418ImATeapot)
                return checks;

            model!.StartGenerationJson!.OutputPath = GetOutputPath(model.StartGenerationJson.JobID!);
            if (!string.IsNullOrEmpty(model.StartGenerationJson.OutputPath))
            {
                Directory.CreateDirectory(model.StartGenerationJson.OutputPath);
                _logger.LogInformation(
                    $"Success: Valid output directory created at path: {model.StartGenerationJson.OutputPath}");

                if (model.InputFile != null)
                {
                    using (var fileStream =
                           new FileStream(
                               model.StartGenerationJson.OutputPath + "/" + model.StartGenerationJson.FileName,
                               FileMode.Create))
                    {
                        await model.InputFile.CopyToAsync(fileStream);
                    }

                    _logger.LogInformation(
                        "Success: File was saved to output directory");
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        statusCode = 200,
                        jobStatus = "Completed Validation",
                        message =
                            $"Success: Valid output directory created at path: {model.StartGenerationJson.OutputPath} and input file saved"
                    });
                }

                return StatusCode(StatusCodes.Status200OK, new
                {
                    statusCode = 200,
                    jobStatus = "Completed Validation",
                    message = $"Success: Valid output directory created at path: {model.StartGenerationJson.OutputPath}"
                });
            }

            _logger.LogError($"Error: No valid file found at path: {model.StartGenerationJson.FileName}");
            return StatusCode(StatusCodes.Status404NotFound, new
            {
                statusCode = 404,
                jobStatus = "Failed Validation",
                message = $"Error: No valid file found at path: {model.StartGenerationJson.FileName}"
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                jobStatus = "Failed Validation",
                message = $"Error: {e.Message}"
            });
        }
    }

    /**
     * <summary> Gets the output path for generation. </summary>
     * <param name="jobId"> The jobID of the file being generated</param>
     * <returns>Output path as string or null if an error occurs.</returns>
     */
    private string? GetOutputPath(Guid? jobId)
    {
        var appOutputRoot = _configuration.GetValue<string>("AppOutputRoot");
        var user = _configuration.GetValue<string>("SystemUser");
        var userHome = Path.Join("/home", user);
        var outputRoot = Path.Join(userHome, appOutputRoot);
        var path = Path.Join(userHome, outputRoot, "job_" + jobId).Replace("\\", "/");
        _logger.LogInformation("Output path: {0}", path);
        return jobId == null ? null : path;
    }

    /**
     * <summary>Gets the request info from the HttpContext of the object</summary>
     * <returns><see cref="StartGenerationJson" /> of the request</returns>
     */
    private StartGenerationJson GetFromJson()
    {
        HttpContext.Request.Form.TryGetValue("StartGenerationJson", out var data);
        var j = JsonNode.Parse(data!);
        var jj = StartGenerationJsonConverter.FromJson(j);
        jj.OutputPath = GetOutputPath(jj.JobID!);
        return jj;
    }

    /**
     * <summary>Converts a Windows file path to a WSL linux file path</summary>
     * <param name="windowsPath">The Windows path string to convert</param>
     * <returns>String path in a linux format</returns>
     */
    private string GetWslPathFromWindowsPath(string windowsPath)
    {
        var fullWindowsPath = Path.GetFullPath(windowsPath);

        var driveLetter = char.ToLower(fullWindowsPath[0]).ToString();

        var remainingPath = fullWindowsPath.Substring(2).Replace(Path.DirectorySeparatorChar, '/');

        var wslPath = $"/mnt/it01{remainingPath}";

        return wslPath;
    }
}