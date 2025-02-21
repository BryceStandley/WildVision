using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository class for managing organisation-related operations.
/// </summary>
public class OrganisationRepository(SchemaContext ctx) : IOrganisationRepository
{
    /// <summary>
    ///     Asynchronously retrieves a list of organisations from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains an enumerable collection of Organisation objects,
    ///     or null if the database is not available.
    /// </returns>
    public async Task<IEnumerable<Organisation>?> GetOrganisationsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var organisations = await ctx.Organisations.ToListAsync();
        organisations.ForEach(a => ctx.Organisations.Attach(a));
        return organisations;
    }

    /// <summary>
    ///     Asynchronously retrieves an organisation by its unique identifier from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the organisation to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the Organisation object if found, or null if the database is not available or the
    ///     organisation is not found.
    /// </returns>
    public async Task<Organisation?> GetOrganisationByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var organisation = await ctx.Organisations.FindAsync(id);
        return organisation;
    }

    /// <summary>
    ///     Asynchronously creates a new organisation in the database.
    /// </summary>
    /// <param name="organisation">The Organisation object to be created.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the created Organisation object,
    ///     or null if the database is not available.
    /// </returns>
    public async Task<Organisation?> CreateOrganisationAsync(Organisation organisation)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Organisations.Attach(organisation);
        ctx.Organisations.Add(organisation);
        await ctx.SaveChangesAsync();
        return organisation;
    }

    /// <summary>
    ///     Asynchronously updates the details of an existing organisation in the database.
    /// </summary>
    /// <param name="organisation">The organisation entity with updated details.</param>
    /// <returns>
    ///     A task that represents the asynchronous update operation.
    ///     The task result contains the updated organisation, or null if the database is not available.
    /// </returns>
    public async Task<Organisation?> UpdateOrganisationAsync(Organisation organisation)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Organisations.Update(organisation);
        await ctx.SaveChangesAsync();
        return organisation;
    }

    /// <summary>
    ///     Asynchronously deletes an organisation from the database.
    /// </summary>
    /// <param name="organisation">The organisation object to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains the number of state entries written to the database,
    ///     or null if the database is not available.
    /// </returns>
    public async Task<int?> DeleteOrganisationAsync(Organisation organisation)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Organisations.Remove(organisation);
        return await ctx.SaveChangesAsync();
    }
}