using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository class for managing OrgRequests entities.
/// </summary>
public class OrgRequestsRepository(SchemaContext ctx) : IOrgRequestsRepository
{
    /// <summary>
    ///     Retrieves a list of organization requests asynchronously.
    /// </summary>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains an enumerable of OrgRequests if
    ///     successful; otherwise, null.
    /// </returns>
    public async Task<IEnumerable<OrgRequests>?> GetOrgRequestsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var orgRequests = await ctx.OrgRequests.ToListAsync();
        orgRequests.ForEach(a => ctx.OrgRequests.Attach(a));
        return orgRequests;
    }

    /// <summary>
    ///     Retrieves an organization request based on its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the organization request.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the organization request if found;
    ///     otherwise, null.
    /// </returns>
    public async Task<OrgRequests?> GetOrgRequestsByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var orgRequest = await ctx.OrgRequests.FindAsync(id);
        return orgRequest;
    }

    /// <summary>
    ///     Asynchronously creates a new organization request.
    /// </summary>
    /// <param name="orgRequests">The organization request to create.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the created OrgRequests object if
    ///     successful; otherwise, null.
    /// </returns>
    public async Task<OrgRequests?> CreateOrgRequestsAsync(OrgRequests orgRequests)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.OrgRequests.Attach(orgRequests);
        ctx.OrgRequests.Add(orgRequests);
        await ctx.SaveChangesAsync();
        return orgRequests;
    }

    /// <summary>
    ///     Updates an existing organization request asynchronously.
    /// </summary>
    /// <param name="orgRequests">
    ///     The organization request object to be updated.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the updated OrgRequests if successful;
    ///     otherwise, null.
    /// </returns>
    public async Task<OrgRequests?> UpdateOrgRequestsAsync(OrgRequests orgRequests)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.OrgRequests.Update(orgRequests);
        await ctx.SaveChangesAsync();
        return orgRequests;
    }

    /// <summary>
    ///     Deletes the specified organization requests asynchronously.
    /// </summary>
    /// <param name="orgRequests">The organization requests to delete.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the number of state entries written to the
    ///     database if successful; otherwise, null.
    /// </returns>
    public async Task<int?> DeleteOrgRequestsAsync(OrgRequests orgRequests)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.OrgRequests.Remove(orgRequests);
        return await ctx.SaveChangesAsync();
    }
}