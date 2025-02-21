using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Provides methods for creating, deleting, updating, and retrieving organization requests.
/// </summary>
public interface IOrgRequestsRepository
{
    /// <summary>
    /// Asynchronously creates a new organization request.
    /// </summary>
    /// <param name="orgRequests">The organization request to create.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the created OrgRequests object if
    /// successful; otherwise, null.
    /// </returns>
    Task<OrgRequests?> CreateOrgRequestsAsync(OrgRequests orgRequests);

    /// <summary>
    /// Asynchronously deletes the specified organization request.
    /// </summary>
    /// <param name="orgRequests">The organization request to delete.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of state entries written to the database.
    /// </returns>
    Task<int?> DeleteOrgRequestsAsync(OrgRequests orgRequests);

    /// <summary>
    /// Asynchronously retrieves an organization request by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the organization request.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the OrgRequests object if found; otherwise, null.
    /// </returns>
    Task<OrgRequests?> GetOrgRequestsByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a list of organization requests.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an enumerable of OrgRequests if
    /// successful; otherwise, null.
    /// </returns>
    Task<IEnumerable<OrgRequests>?> GetOrgRequestsAsync();

    /// <summary>
    /// Asynchronously updates an existing organization request.
    /// </summary>
    /// <param name="orgRequests">The organization request to update.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the updated OrgRequests object if successful; otherwise, null.
    /// </returns>
    Task<OrgRequests?> UpdateOrgRequestsAsync(OrgRequests orgRequests);
}