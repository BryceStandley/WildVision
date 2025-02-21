using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository class for handling organisation access-related operations in the database.
/// </summary>
public class OrganisationAccessRepository(SchemaContext ctx) : IOrganisationAccessRepository
{
    /// <summary>
    ///     Retrieves a list of organisation accesses asynchronously.
    /// </summary>
    /// <return>
    ///     Returns an IEnumerable of OrganisationAccess objects if the database is available; otherwise, returns null.
    /// </return>
    public async Task<IEnumerable<OrganisationAccess>?> GetOrganisationAccessesAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var organisationAccesses = await ctx.OrganisationAccesses.ToListAsync();
        organisationAccesses.ForEach(a => ctx.OrganisationAccesses.Attach(a));
        return organisationAccesses;
    }

    /// <summary>
    ///     Retrieves an organisation access by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the organisation access to retrieve.
    /// </param>
    /// <return>
    ///     Returns an OrganisationAccess object if found and the database is available; otherwise, returns null.
    /// </return>
    public async Task<OrganisationAccess?> GetOrganisationAccessByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var organisationAccess = await ctx.OrganisationAccesses.FindAsync(id);
        return organisationAccess;
    }

    /// <summary>
    ///     Creates a new organisation access record asynchronously.
    /// </summary>
    /// <param name="organisationAccess">
    ///     The organisation access entity to be added to the database.
    /// </param>
    /// <return>
    ///     Returns the created OrganisationAccess object if the operation is successful; otherwise, returns null.
    /// </return>
    public async Task<OrganisationAccess?> CreateOrganisationAccessAsync(OrganisationAccess organisationAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.OrganisationAccesses.Attach(organisationAccess);
        ctx.OrganisationAccesses.Add(organisationAccess);
        await ctx.SaveChangesAsync();
        return organisationAccess;
    }

    /// <summary>
    ///     Updates an existing organisation access asynchronously.
    /// </summary>
    /// <param name="organisationAccess">
    ///     The OrganisationAccess object containing updated details.
    /// </param>
    /// <return>
    ///     Returns the updated OrganisationAccess object if the update is successful;
    ///     otherwise, returns null if the database is unavailable.
    /// </return>
    public async Task<OrganisationAccess?> UpdateOrganisationAccessAsync(OrganisationAccess organisationAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.OrganisationAccesses.Update(organisationAccess);
        await ctx.SaveChangesAsync();
        return organisationAccess;
    }

    /// <summary>
    ///     Deletes a specified organisation access asynchronously.
    /// </summary>
    /// <param name="organisationAccess">
    ///     The OrganisationAccess object to be deleted.
    /// </param>
    /// <return>
    ///     Returns the number of state entries written to the database, or null if the database is not available.
    /// </return>
    public async Task<int?> DeleteOrganisationAccessAsync(OrganisationAccess organisationAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.OrganisationAccesses.Remove(organisationAccess);
        return await ctx.SaveChangesAsync();
    }
}