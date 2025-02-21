using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     UserRepository provides methods to perform CRUD operations on User entities and manage user-related data in the
///     database.
/// </summary>
public class UserRepository(SchemaContext ctx, ILogger<UserRepository> logger) : IUserRepository
{
    /// <summary>
    ///     Asynchronously retrieves a list of users from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an enumerable collection of users,
    ///     or null if the database is not available.
    /// </returns>
    public async Task<IEnumerable<User>?> GetUsersAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var users = await ctx.Users.ToListAsync();
        users.ForEach(a => ctx.Users.Attach(a));
        return users;
    }

    /// <summary>
    ///     Asynchronously retrieves a user by their unique identifier from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the user if found, or null if the
    ///     user is not available or the database is not available.
    /// </returns>
    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var user = await ctx.Users.FindAsync(id);
        return user;
    }

    /// <summary>
    ///     Asynchronously creates a new user in the database.
    /// </summary>
    /// <param name="user">The user entity to be created.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created user entity, or null
    ///     if the database is not available.
    /// </returns>
    public async Task<User?> CreateUserAsync(User user)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Users.Attach(user);
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    /// <summary>
    ///     Asynchronously updates a user record in the database.
    /// </summary>
    /// <param name="user">The user entity with updated information to save in the database.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the updated user entity, or null
    ///     if the database is not available.
    /// </returns>
    public async Task<User?> UpdateUserAsync(User user)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    /// <summary>
    ///     Asynchronously deletes a user from the database.
    /// </summary>
    /// <param name="user">The user entity to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the number of state entries
    ///     written to the database, or null if the database is not available.
    /// </returns>
    public async Task<int?> DeleteUserAsync(User user)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Users.Remove(user);
        return await ctx.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves a subscription by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the subscription if found, or null
    ///     if the subscription is not available or the database is not accessible.
    /// </returns>
    public async Task<Subscription?> GetSubscriptionByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var subscription = await ctx.Subscriptions.FindAsync(id);

        if (subscription == null)
            logger.LogWarning("No subscription found with ID {id}", id);
        else
            logger.LogInformation("Found subscription: {title}", subscription.SubscriptionTitle);

        return subscription;
    }

    /// <summary>
    ///     Asynchronously retrieves the default subscription from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the default subscription, or null
    ///     if the database is not available or no subscriptions exist.
    /// </returns>
    public async Task<Subscription?> GetDefaultSubscriptionAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var subs = await ctx.Subscriptions.ToListAsync();
        if (subs.Count == 0)
            return null;

        return subs.FirstOrDefault();
    }

    /// <summary>
    ///     Asynchronously retrieves the email address of a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose email address is to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the email address of the user, or
    ///     null if the user is not found or the database is unavailable.
    /// </returns>
    public async Task<string?> GetEmailByIdAsync(Guid userId)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var user = await ctx.Users.FindAsync(userId);
        return user?.UserEmail; // Return null if the user is not found
    }
}