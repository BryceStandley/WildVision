using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository for managing access types in the database.
/// </summary>
public class AccessTypeRepository(SchemaContext ctx) : IAccessTypeRepository
{
    /// <summary>
    ///     Retrieves a list of access types asynchronously from the database.
    /// </summary>
    /// <returns>An IEnumerable of AccessType representing the access types, or null if the database is not available.</returns>
    public async Task<IEnumerable<AccessType>?> GetAccessTypesAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var at = await ctx.AccessTypes.ToListAsync();
        at.ForEach(a => ctx.AccessTypes.Attach(a));
        return at;
    }

    /// <summary>
    ///     Retrieves an access type by its ID asynchronously from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the access type.</param>
    /// <returns>
    ///     An AccessType object representing the access type with the specified ID, or null if the database is not
    ///     available or the access type is not found.
    /// </returns>
    public async Task<AccessType?> GetAccessTypeByIDAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        return await ctx.AccessTypes.FindAsync(id);
    }

    /// <summary>
    ///     Creates a new access type record asynchronously in the database.
    /// </summary>
    /// <param name="accessType">The AccessType object to be created and saved.</param>
    /// <returns>The created AccessType object, or null if the database is not available.</returns>
    public async Task<AccessType?> CreateAccessTypeAsync(AccessType accessType)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.AccessTypes.Attach(accessType);
        ctx.AccessTypes.Add(accessType);
        await ctx.SaveChangesAsync();
        return accessType;
    }

    /// <summary>
    ///     Updates an existing AccessType record in the database asynchronously.
    /// </summary>
    /// <param name="accessType">The AccessType object to be updated.</param>
    /// <returns>The updated AccessType object if the operation is successful, or null if the database is not available.</returns>
    public async Task<AccessType?> UpdateAccessTypeAsync(AccessType accessType)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.AccessTypes.Attach(accessType);
        ctx.AccessTypes.Update(accessType);
        await ctx.SaveChangesAsync();
        return accessType;
    }

    /// <summary>
    ///     Asynchronously deletes the specified access type from the database.
    /// </summary>
    /// <param name="accessType">The access type to be deleted.</param>
    /// <returns>An integer representing the number of rows affected, or null if the database is not available.</returns>
    public async Task<int?> DeleteAccessTypeAsync(AccessType accessType)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.AccessTypes.Remove(accessType);
        return await ctx.SaveChangesAsync();
    }
}