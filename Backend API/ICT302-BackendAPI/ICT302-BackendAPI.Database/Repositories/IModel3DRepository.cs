using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for managing 3D models in the database.
/// </summary>
public interface IModel3DRepository
{
    /// <summary>
    /// Asynchronously creates a new 3D model and saves it to the database.
    /// </summary>
    /// <param name="model3D">The 3D model to be created and saved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created 3D model, or null if the creation failed.</returns>
    Task<Model3D?> CreateModel3DAsync(Model3D model3D);

    /// <summary>
    /// Asynchronously deletes a 3D model from the database.
    /// </summary>
    /// <param name="model3D">The 3D model to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains the number of state entries written to the database, or null if the operation failed.</returns>
    Task<int?> DeleteModel3DAsync(Model3D model3D);

    /// <summary>
    /// Asynchronously gets a 3D model by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the 3D model.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the 3D model if found, or null if not found.</returns>
    Task<Model3D?> GetModel3DByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a 3D model associated with a specific graphics ID.
    /// </summary>
    /// <param name="graphicsId">The ID of the graphics to search for the associated 3D model.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the 3D model associated with the specified graphics ID, or null if not found.</returns>
    Task<Model3D?> GetModel3DFromGraphicsIdAsync(Guid graphicsId);

    /// <summary>
    /// Asynchronously retrieves a list of 3D models associated with a specific animal ID.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of 3D models associated with the specified animal, or null if the retrieval failed.</returns>
    Task<List<Model3D>?> GetModel3DListFromAnimalIdAsync(Guid animalId);

    /// <summary>
    /// Asynchronously retrieves a list of 3D models from the database.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an
    /// enumerable list of 3D models, or null if the database is unavailable.
    /// </returns>
    Task<IEnumerable<Model3D>?> GetModel3DsAsync();

    /// <summary>
    /// Asynchronously updates an existing 3D model in the database.
    /// </summary>
    /// <param name="model3D">The 3D model object containing updated information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated 3D model, or null if the update failed.</returns>
    Task<Model3D?> UpdateModel3DAsync(Model3D model3D);
}