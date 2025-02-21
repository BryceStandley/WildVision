using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Manages the access and manipulation of subscription data within the database.
/// </summary>
public class SubscriptionRepository(SchemaContext ctx) : ISubscriptionRepository
{
    /// <summary>
    ///     Retrieves a list of subscriptions asynchronously.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an IEnumerable of Subscription objects, or null if the database is unavailable.
    /// </returns>
    public async Task<IEnumerable<Subscription>?> GetSubscriptionsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var subscriptions = await ctx.Subscriptions.ToListAsync();
        subscriptions.ForEach(a => ctx.Subscriptions.Attach(a));
        return subscriptions;
    }

    /// <summary>
    ///     Retrieves a subscription by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the Subscription object if found, or null if the database is unavailable or the
    ///     subscription does not exist.
    /// </returns>
    public async Task<Subscription?> GetSubscriptionByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var subscription = await ctx.Subscriptions.FindAsync(id);
        return subscription;
    }

    /// <summary>
    ///     Creates a new subscription asynchronously.
    /// </summary>
    /// <param name="subscription">The subscription object to be created.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the created Subscription object, or null if the database is unavailable.
    /// </returns>
    public async Task<Subscription?> CreateSubscriptionAsync(Subscription subscription)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Subscriptions.Attach(subscription);
        ctx.Subscriptions.Add(subscription);
        await ctx.SaveChangesAsync();
        return subscription;
    }

    /// <summary>
    ///     Updates an existing subscription asynchronously.
    /// </summary>
    /// <param name="subscription">
    ///     The subscription object containing the updated information.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the updated Subscription object, or null if the database is unavailable.
    /// </returns>
    public async Task<Subscription?> UpdateSubscriptionAsync(Subscription subscription)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Subscriptions.Update(subscription);
        await ctx.SaveChangesAsync();
        return subscription;
    }

    /// <summary>
    ///     Deletes a subscription asynchronously.
    /// </summary>
    /// <param name="subscription">
    ///     The subscription object to be deleted.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an integer representing the number of state entries written to the database,
    ///     or null if the database is unavailable.
    /// </returns>
    public async Task<int?> DeleteSubscriptionAsync(Subscription subscription)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Subscriptions.Remove(subscription);
        return await ctx.SaveChangesAsync();
    }
}