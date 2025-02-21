// IGraphicRepository.cs

using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for handling CRUD operations related to Graphic entities.
/// </summary>
public interface IGraphicRepository
{
    /// <summary>
    /// Asynchronously fetches a list of all Graphic entities from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a collection of Graphic entities if the database
    /// is available; otherwise, null.
    /// </returns>
    Task<IEnumerable<Graphic>?> GetGraphicsAsync();

    /// <summary>
    /// Asynchronously creates a new Graphic entity in the database.
    /// </summary>
    /// <param name="graphic">
    /// The Graphic entity to be created.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the created Graphic entity
    /// if the operation is successful; otherwise, null.
    /// </returns>
    Task<Graphic?> CreateGraphicAsync(Graphic graphic);

    /// <summary>
    /// Asynchronously deletes a specified Graphic entity from the database.
    /// </summary>
    /// <param name="graphic">
    /// The Graphic entity to be deleted.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the number of state entries written to the database if the operation is successful; otherwise, null.
    /// </returns>
    Task<int?> DeleteGraphicAsync(Graphic graphic);

    /// <summary>
    /// Asynchronously updates an existing Graphic entity in the database.
    /// </summary>
    /// <param name="graphic">The Graphic entity to be updated, containing the new values.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the updated Graphic entity if the update
    /// is successful; otherwise, null if the database is unavailable.
    /// </returns>
    Task<Graphic?> UpdateGraphicAsync(Graphic graphic);

    /// <summary>
    /// Asynchronously fetches a Graphic entity from the database using the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Graphic entity to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the Graphic entity if found; otherwise, null.
    /// </returns>
    Task<Graphic?> GetGraphicByIDAsync(Guid? id);

    /// <summary>
    /// Asynchronously retrieves a Graphic entity from the database based on the provided file name.
    /// </summary>
    /// <param name="fileName">
    /// The file name of the Graphic entity to retrieve.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the matching Graphic entity if found; otherwise, null.
    /// </returns>
    Task<Graphic?> GetGraphicByFileNameAsync(string fileName);
}