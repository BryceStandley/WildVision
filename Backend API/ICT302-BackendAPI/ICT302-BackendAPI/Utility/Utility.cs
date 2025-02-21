namespace ICT302_BackendAPI.Utility;

/**
 * <summary>Simple Utility class to print the starting application config to the console</summary>
 */
public class Utility(IConfiguration configuration, ILogger logger, IWebHostEnvironment environment)
{
    /**
     * <summary>Prints the starting application config to the console</summary>
     */
    public void PrintStartingConfig()
    {
        var config = "Starting with configuration:\n";
        config += "\tApp Environment: " + environment.EnvironmentName + "\n";

        if (environment.IsDevelopment())
            config += "\tUser Storage Path: " + configuration["dev_StoredFilesPath"] + "\n";
        else
            config += "\tUser Storage Path: " + configuration["StoredFilesPath"] + "\n";

        config += "\tLogging file: " + configuration.GetValue<string>("Logging:File:Path") + "\n";
        config += "\tSwagger Enabled: " + configuration.GetValue<bool>("EnableSwagger") + "\n";
        config += "\tGenAPI URL: " + configuration.GetValue<string>("GenAPIUrl") + "\n";

        logger.LogInformation(config);
    }
}