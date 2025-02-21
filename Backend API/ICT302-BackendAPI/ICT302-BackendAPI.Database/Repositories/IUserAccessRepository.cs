using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for managing user access in the database.
/// </summary>
public interface IUserAccessRepository
{
    /// <summary>
    /// Creates a new user access entry asynchronously.
    /// </summary>
    /// <param name="userAccess">The UserAccess object to be created.</param>
    /// <returns>An asynchronous task that returns the created UserAccess object or null if the database is not available.</returns>
    Task<UserAccess?> CreateUserAccessAsync(UserAccess userAccess);

    /// <summary>
    /// Deletes a user access entry asynchronously.
    /// </summary>
    /// <param name="userAccess">The user access object to be deleted.</param>
    /// <returns>An asynchronous task that returns the number of state entries written to the database, or null if the database is not available.</returns>
    Task<int?> DeleteUserAccessAsync(UserAccess userAccess);

    /// <summary>
    /// Retrieves a user access entry by the provided organization ID and user ID asynchronously.
    /// </summary>
    /// <param name="orgId">The unique identifier for the organization.</param>
    /// <param name="userId">The unique identifier for the user.</param>
    /// <returns>An asynchronous task that returns the UserAccess object if found, or null if not found or the database is not available.</returns>
    Task<UserAccess?> GetUserAccessByKeysAsync(Guid orgId, Guid userId);

    /// <summary>
    /// Retrieves all user accesses asynchronously.
    /// </summary>
    /// <returns>An asynchronous task that returns an enumerable list of UserAccess objects or null if the database is not available.</returns>
    Task<IEnumerable<UserAccess>?> GetUserAccessesAsync();

    /// <summary>
    /// Updates a user access record asynchronously in the database.
    /// </summary>
    /// <param name="userAccess">The UserAccess entity to be updated.</param>
    /// <returns>An asynchronous task that returns the updated UserAccess object or null if the database is not available.</returns>
    Task<UserAccess?> UpdateUserAccessAsync(UserAccess userAccess);
}