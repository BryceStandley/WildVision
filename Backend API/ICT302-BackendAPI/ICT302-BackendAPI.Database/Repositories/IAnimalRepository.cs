using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     The IAnimalRepository interface provides methods for performing CRUD operations on Animal entities.
/// </summary>
public interface IAnimalRepository
{
    /// <summary>
    /// Asynchronously creates a new animal record in the database.
    /// </summary>
    /// <param name="animal">The animal entity to be created.</param>
    /// <returns>A task representing the asynchronous operation, which wraps the created animal entity, or null if creation failed.</returns>
    Task<Animal?> CreateAnimalAsync(Animal animal);

    /// <summary>
    /// Asynchronously deletes an animal record from the database.
    /// </summary>
    /// <param name="animal">The animal entity to be deleted.</param>
    /// <returns>A task representing the asynchronous operation, which wraps the number of state entries written to the database.
    /// If the state entries count is zero, deletion may have failed.</returns>
    Task<int?> DeleteAnimalAsync(Animal animal);

    /// <summary>
    /// Asynchronously retrieves an animal record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the animal to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation, which wraps the animal entity if found, or null if the animal does not exist.</returns>
    Task<Animal?> GetAnimalByIdAsync(Guid? id);

    /// <summary>
    /// Asynchronously retrieves a list of all animal records from the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, which wraps a collection of animal entities, or null if the database is not available.</returns>
    Task<IEnumerable<Animal>?> GetAnimalsAsync();

    /// <summary>
    /// Asynchronously updates an existing animal record in the database.
    /// </summary>
    /// <param name="animal">The animal entity to be updated.</param>
    /// <returns>A task representing the asynchronous operation, which wraps the updated animal entity, or null if the update failed.</returns>
    Task<Animal?> UpdateAnimalAsync(Animal animal);

    /// <summary>
    /// Updates the video data for a specific animal identified by the given animal ID asynchronously.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal to update.</param>
    /// <param name="videoFileName">The name of the video file associated with the animal.</param>
    /// <param name="uploadDate">The date when the video file was uploaded.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated Animal object,
    /// or null if the animal is not found or the database is not available.
    /// </returns>
    Task<Animal?> UpdateAnimalVideoDataAsync(Guid animalId, string videoFileName, DateTime uploadDate);
}