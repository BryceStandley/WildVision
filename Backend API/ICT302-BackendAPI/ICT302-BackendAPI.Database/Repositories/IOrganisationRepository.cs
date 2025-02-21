using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for repository operations related to Organisation entities.
/// </summary>
public interface IOrganisationRepository
{
    /// <summary>
    /// Asynchronously creates a new organisation in the database.
    /// </summary>
    /// <param name="organisation">The Organisation object to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the created Organisation object,
    /// or null if the database is not available.
    /// </returns>
    Task<Organisation?> CreateOrganisationAsync(Organisation organisation);

    /// <summary>
    /// Asynchronously deletes an organisation from the database.
    /// </summary>
    /// <param name="organisation">The organisation object to be deleted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of state entries written to the database,
    /// or null if the database is not available.
    /// </returns>
    Task<int?> DeleteOrganisationAsync(Organisation organisation);

    /// <summary>
    /// Asynchronously retrieves an organisation from the database by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the organisation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the Organisation object, or null if not found or the database is unavailable.
    /// </returns>
    Task<Organisation?> GetOrganisationByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a list of organisations from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an enumerable collection of Organisation objects,
    /// or null if the database is not available.
    /// </returns>
    Task<IEnumerable<Organisation>?> GetOrganisationsAsync();

    /// <summary>
    /// Asynchronously updates the details of an existing organisation in the database.
    /// </summary>
    /// <param name="organisation">The organisation entity with updated details.</param>
    /// <returns>
    /// A task that represents the asynchronous update operation.
    /// The task result contains the updated organisation, or null if the database is not available.
    /// </returns>
    Task<Organisation?> UpdateOrganisationAsync(Organisation organisation);
}