using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for managing animal access data.
/// </summary>
public interface IAnimalAccessRepository
{
    /// <summary>
    /// Creates a new AnimalAccess record in the database asynchronously.
    /// </summary>
    /// <param name="animalAccess">The AnimalAccess object to create.</param>
    /// <return>The created AnimalAccess object or null if the creation fails.</return>
    Task<AnimalAccess?> CreateAnimalAccessAsync(AnimalAccess animalAccess);

    /// <summary>
    /// Deletes an existing AnimalAccess record from the database asynchronously.
    /// </summary>
    /// <param name="animalAccess">The AnimalAccess object to delete.</param>
    /// <return>The number of state entries written to the database, or null if the deletion fails.</return>
    Task<int?> DeleteAnimalAccessAsync(AnimalAccess animalAccess);

    /// <summary>
    /// Retrieves an AnimalAccess record by its ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the AnimalAccess record.</param>
    /// <return>The AnimalAccess object if found, or null if no record is found.</return>
    Task<AnimalAccess?> GetAnimalAccessByIDAsync(Guid id);

    /// <summary>
    /// Retrieves a list of AnimalAccess objects asynchronously from the database.
    /// </summary>
    /// <return>
    /// A Task representing the asynchronous operation. The task result contains an IEnumerable of AnimalAccess objects if the database is available,
    /// otherwise, it returns null.
    /// </return>
    Task<IEnumerable<AnimalAccess>?> GetAnimalAccessesAsync();

    /// <summary>
    /// Updates an existing AnimalAccess record in the database asynchronously.
    /// </summary>
    /// <param name="animalAccess">The AnimalAccess object containing updated data.</param>
    /// <return>The updated AnimalAccess object or null if the update fails.</return>
    Task<AnimalAccess?> UpdateAnimalAccessAsync(AnimalAccess animalAccess);

    /// <summary>
    /// Retrieves a list of AnimalID objects associated with a given UserID asynchronously from the database.
    /// </summary>
    /// <param name="userID">The unique identifier of the user for whom the animal IDs are being retrieved.</param>
    /// <returns>
    /// A Task representing the asynchronous operation. The task result contains an IEnumerable of Guid objects
    /// representing the Animal IDs if the database is available,
    /// otherwise, it returns null.
    /// </returns>
    Task<IEnumerable<Guid>?> GetAnimalIDsByUserIDAsync(Guid userID);

    /// <summary>
    /// Checks asynchronously if a user has access to a specified animal in the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <return>A Task representing the asynchronous operation. The task result contains a boolean value indicating whether the user has access to the specified animal.</return>
    Task<bool> UserHasAccessAsync(Guid userId, Guid animalId);
}