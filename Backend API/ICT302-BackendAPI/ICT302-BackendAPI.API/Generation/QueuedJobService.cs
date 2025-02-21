using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ICT302_BackendAPI.API.Generation;

/// <summary>
///     The application job service
/// </summary>
/// <param name="taskQueue">The background job queue</param>
/// <param name="logger">the application logger</param>
public sealed class QueuedJobService(
    IBackgroundJobQueue taskQueue,
    ILogger<QueuedJobService> logger) : BackgroundService
{
    /// <summary>
    ///     Executes the background task queue thread
    /// </summary>
    /// <param name="stoppingToken">cancellation token</param>
    /// <returns>task</returns>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting Background job queue thread...");

        return ProcessTaskQueueAsync(stoppingToken);
    }

    /// <summary>
    ///     Processes a task queue item
    /// </summary>
    /// <param name="stoppingToken">cancellation token</param>
    private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var workItem =
                    await taskQueue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing task work item.");
            }
    }

    /// <summary>
    ///     Stops the task queue thread
    /// </summary>
    /// <param name="stoppingToken"></param>
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Background job queue is stopping.");

        await base.StopAsync(stoppingToken);
    }
}