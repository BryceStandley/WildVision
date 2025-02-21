using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Defines interface methods for accessing and managing organisation access records in the database.
/// </summary>
public interface IOrganisationAccessRepository
{
    /// <summary>
    /// Asynchronously creates a new OrganisationAccess record in the database.
    /// </summary>
    /// <param name="organisationAccess">The OrganisationAccess entity to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created OrganisationAccess entity, or null if database is not available.</returns>
    Task<OrganisationAccess?> CreateOrganisationAccessAsync(OrganisationAccess organisationAccess);

    /// <summary>
    /// Asynchronously deletes an existing OrganisationAccess record from the database.
    /// </summary>
    /// <param name="organisationAccess">The OrganisationAccess entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database, or null if the database is not available.</returns>
    Task<int?> DeleteOrganisationAccessAsync(OrganisationAccess organisationAccess);

    /// <summary>
    /// Asynchronously retrieves an OrganisationAccess record from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the OrganisationAccess record to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved OrganisationAccess entity, or null if no record is found.</returns>
    Task<OrganisationAccess?> GetOrganisationAccessByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a list of organisation accesses asynchronously.
    /// </summary>
    /// <return>
    /// Returns an IEnumerable of OrganisationAccess objects if the database is available; otherwise, returns null.
    /// </return>
    Task<IEnumerable<OrganisationAccess>?> GetOrganisationAccessesAsync();

    /// <summary>
    /// Asynchronously updates an existing OrganisationAccess record in the database.
    /// </summary>
    /// <param name="organisationAccess">The OrganisationAccess entity to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated OrganisationAccess entity, or null if the update failed.</returns>
    Task<OrganisationAccess?> UpdateOrganisationAccessAsync(OrganisationAccess organisationAccess);
}