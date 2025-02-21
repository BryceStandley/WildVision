using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Provides an interface for managing user-related data operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Asynchronously creates a new user in the repository.
    /// </summary>
    /// <param name="user">The user object to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created user object or null if creation was unsuccessful.</returns>
    Task<User?> CreateUserAsync(User user);

    /// <summary>
    /// Asynchronously deletes a user from the repository.
    /// </summary>
    /// <param name="user">The user object to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows or null if deletion was unsuccessful.</returns>
    Task<int?> DeleteUserAsync(User user);

    /// <summary>
    /// Asynchronously retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user object if found, or null if the user does not exist.</returns>
    Task<User?> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a list of users from the repository.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of users,
    /// or null if retrieval was unsuccessful.
    /// </returns>
    Task<IEnumerable<User>?> GetUsersAsync();

    /// <summary>
    /// Asynchronously updates an existing user in the repository.
    /// </summary>
    /// <param name="user">The user object containing updated information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user object or null if the update was unsuccessful.</returns>
    Task<User?> UpdateUserAsync(User user);

    /// <summary>
    /// Asynchronously retrieves a subscription by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the subscription if found, or null
    /// if the subscription is not available or the database is not accessible.
    /// </returns>
    Task<Subscription?> GetSubscriptionByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves the default subscription from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the default subscription object or null if no subscription is found.</returns>
    Task<Subscription?> GetDefaultSubscriptionAsync();

    /// <summary>
    /// Asynchronously retrieves the email address of a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose email address is to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the email address of the user, or null if the user is not found.</returns>
    Task<string?> GetEmailByIdAsync(Guid userId);
}