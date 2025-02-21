using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for Subscription Repository which manages subscription related operations.
/// </summary>
public interface ISubscriptionRepository
{
    /// <summary>
    /// Creates a new subscription asynchronously.
    /// </summary>
    /// <param name="subscription">The subscription object to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the created Subscription object, or null if the database is unavailable.
    /// </returns>
    Task<Subscription?> CreateSubscriptionAsync(Subscription subscription);

    /// <summary>
    /// Deletes a subscription asynchronously.
    /// </summary>
    /// <param name="subscription">
    /// The subscription object to be deleted.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an integer representing the number of state entries written to the database,
    /// or null if the database is unavailable.
    /// </returns>
    Task<int?> DeleteSubscriptionAsync(Subscription subscription);

    /// <summary>
    /// Retrieves a subscription by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription to be retrieved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the Subscription object if found, or null if no subscription with the specified ID exists.
    /// </returns>
    Task<Subscription?> GetSubscriptionByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a list of subscriptions asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an IEnumerable of Subscription objects, or null if the database is unavailable.
    /// </returns>
    Task<IEnumerable<Subscription>?> GetSubscriptionsAsync();

    /// <summary>
    /// Updates an existing subscription asynchronously.
    /// </summary>
    /// <param name="subscription">The subscription object containing updated details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the updated Subscription object, or null if the database is unavailable.
    /// </returns>
    Task<Subscription?> UpdateSubscriptionAsync(Subscription subscription);
}