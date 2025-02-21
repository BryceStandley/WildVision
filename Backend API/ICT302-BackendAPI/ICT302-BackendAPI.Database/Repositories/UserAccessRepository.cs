using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Provides access to user access data stored in the database.
/// </summary>
public class UserAccessRepository(SchemaContext ctx) : IUserAccessRepository
{
    /// <summary>
    ///     Retrieves all user accesses asynchronously.
    /// </summary>
    /// <returns>
    ///     An asynchronous task that returns an enumerable list of UserAccess objects or null if the database is not
    ///     available.
    /// </returns>
    public async Task<IEnumerable<UserAccess>?> GetUserAccessesAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var userAccesses = await ctx.UserAccess.ToListAsync();
        userAccesses.ForEach(a => ctx.UserAccess.Attach(a));
        return userAccesses;
    }

    /// <summary>
    ///     Retrieves a user access record by organization ID and user ID asynchronously.
    /// </summary>
    /// <param name="orgId">The ID of the organization.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>An asynchronous task that returns a UserAccess object or null if the database is not available.</returns>
    public async Task<UserAccess?> GetUserAccessByKeysAsync(Guid orgId, Guid userId)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var userAccess = await ctx.UserAccess.FindAsync(orgId, userId);
        return userAccess;
    }

    /// <summary>
    ///     Creates a new user access entry asynchronously.
    /// </summary>
    /// <param name="userAccess">The UserAccess object to be created.</param>
    /// <returns>An asynchronous task that returns the created UserAccess object or null if the database is not available.</returns>
    public async Task<UserAccess?> CreateUserAccessAsync(UserAccess userAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.UserAccess.Attach(userAccess);
        ctx.UserAccess.Add(userAccess);
        await ctx.SaveChangesAsync();
        return userAccess;
    }

    /// <summary>
    ///     Updates a user access record asynchronously in the database.
    /// </summary>
    /// <param name="userAccess">The UserAccess entity to be updated.</param>
    /// <returns>An asynchronous task that returns the updated UserAccess object, or null if the database is not available.</returns>
    public async Task<UserAccess?> UpdateUserAccessAsync(UserAccess userAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.UserAccess.Update(userAccess);
        await ctx.SaveChangesAsync();
        return userAccess;
    }

    /// <summary>
    ///     Deletes a user access entry asynchronously.
    /// </summary>
    /// <param name="userAccess">The user access object to be deleted.</param>
    /// <returns>
    ///     An asynchronous task that returns the number of state entries written to the database, or null if the database
    ///     is not available.
    /// </returns>
    public async Task<int?> DeleteUserAccessAsync(UserAccess userAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.UserAccess.Remove(userAccess);
        return await ctx.SaveChangesAsync();
    }
}