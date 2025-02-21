using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for accessing and managing access types in the database.
/// </summary>
public interface IAccessTypeRepository
{
    /// <summary>
    /// Creates a new access type record asynchronously in the database.
    /// </summary>
    /// <param name="accessType">The AccessType object to be created and saved.</param>
    /// <returns>The created AccessType object, or null if the database is not available.</returns>
    Task<AccessType?> CreateAccessTypeAsync(AccessType accessType);

    /// <summary>
    /// Asynchronously deletes the specified access type from the database.
    /// </summary>
    /// <param name="accessType">The access type to be deleted.</param>
    /// <returns>An integer representing the number of rows affected, or null if the database is not available.</returns>
    Task<int?> DeleteAccessTypeAsync(AccessType accessType);

    /// <summary>
    /// Retrieves an access type record by its unique identifier asynchronously from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the access type to retrieve.</param>
    /// <returns>The AccessType object if found, or null if the access type does not exist or the database is not available.</returns>
    Task<AccessType?> GetAccessTypeByIDAsync(Guid id);

    /// <summary>
    /// Retrieves a list of access types asynchronously from the database.
    /// </summary>
    /// <returns>An IEnumerable of AccessType representing the access types, or null if the database is not available.</returns>
    Task<IEnumerable<AccessType>?> GetAccessTypesAsync();

    /// <summary>
    /// Updates an existing AccessType record in the database asynchronously.
    /// </summary>
    /// <param name="accessType">The AccessType object to be updated.</param>
    /// <returns>The updated AccessType object if the operation is successful, or null if the database is not available.</returns>
    Task<AccessType?> UpdateAccessTypeAsync(AccessType accessType);
}