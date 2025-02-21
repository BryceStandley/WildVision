namespace ICT302_BackendAPI.API.Generation;

/// <summary>
///     Background job queue interface
/// </summary>
public interface IBackgroundJobQueue
{
    /// <summary>
    /// Adds a work item to the queue.
    /// </summary>
    /// <param name="workItem">The work item to queue.</param>
    /// <returns>A ValueTask representing the asynchronous operation.</returns>
    ValueTask QueueBackgroundWorkItemAsync(
        Func<CancellationToken, ValueTask> workItem);

    /// <summary>
    /// Dequeues a work item from the queue.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the dequeue operation.</param>
    /// <returns>Function that processes the work item.</returns>
    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken);
}