using System.Threading.Channels;

namespace ICT302_BackendAPI.API.Generation;

/// <summary>
///     Class to manage a separate background thread for generation jobs
/// </summary>
public class BackgroundJobQueue : IBackgroundJobQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    /// <summary>
    ///     Creates a background job queue
    /// </summary>
    /// <param name="capacity">the capacity of the queue</param>
    public BackgroundJobQueue(int capacity)
    {
        BoundedChannelOptions options = new(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }

    /// <summary>
    ///     Adds a work item to the queue
    /// </summary>
    /// <param name="workItem">The work item to queue</param>
    public async ValueTask QueueBackgroundWorkItemAsync(
        Func<CancellationToken, ValueTask> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        await _queue.Writer.WriteAsync(workItem);
    }

    /// <summary>
    ///     Dequeues a work item from the queue
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the work item</param>
    /// <returns>Function that processes the work item</returns>
    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        var workItem =
            await _queue.Reader.ReadAsync(cancellationToken);

        return workItem;
    }
}