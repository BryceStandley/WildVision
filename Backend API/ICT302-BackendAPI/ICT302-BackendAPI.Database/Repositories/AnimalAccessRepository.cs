using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;
// Added for LINQ methods

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository class that provides access to animal-related data.
/// </summary>
public class AnimalAccessRepository(SchemaContext ctx) : IAnimalAccessRepository
{
    /// <summary>
    ///     Retrieves a list of AnimalAccess objects asynchronously from the database.
    /// </summary>
    /// <returns>
    ///     A Task representing the asynchronous operation. The task result contains an IEnumerable of AnimalAccess objects if
    ///     the database is available,
    ///     otherwise, it returns null.
    /// </returns>
    public async Task<IEnumerable<AnimalAccess>?> GetAnimalAccessesAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var animalAccesses = await ctx.AnimalAccesses.ToListAsync();
        animalAccesses.ForEach(a => ctx.AnimalAccesses.Attach(a));
        return animalAccesses;
    }

    /// <summary>
    ///     Retrieves an AnimalAccess object by its ID asynchronously from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the AnimalAccess object to retrieve.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation. The task result contains an AnimalAccess object if the ID is found
    ///     in the database,
    ///     otherwise, it returns null.
    /// </returns>
    public async Task<AnimalAccess?> GetAnimalAccessByIDAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var animalAccess = await ctx.AnimalAccesses.FindAsync(id);
        return animalAccess;
    }

    /// <summary>
    ///     Creates a new AnimalAccess object asynchronously and saves it to the database.
    /// </summary>
    /// <param name="animalAccess">The AnimalAccess object to be created and saved.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation. The task result contains the created AnimalAccess object if the
    ///     operation is successful,
    ///     otherwise, it returns null.
    /// </returns>
    public async Task<AnimalAccess?> CreateAnimalAccessAsync(AnimalAccess animalAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.AnimalAccesses.Attach(animalAccess);
        ctx.AnimalAccesses.Add(animalAccess);
        await ctx.SaveChangesAsync();
        return animalAccess;
    }

    /// <summary>
    ///     Updates the specified AnimalAccess object asynchronously in the database.
    /// </summary>
    /// <param name="animalAccess">The AnimalAccess object to be updated.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation. The task result contains the updated AnimalAccess object if the
    ///     database is available,
    ///     otherwise, it returns null.
    /// </returns>
    public async Task<AnimalAccess?> UpdateAnimalAccessAsync(AnimalAccess animalAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.AnimalAccesses.Attach(animalAccess);
        ctx.AnimalAccesses.Update(animalAccess);
        await ctx.SaveChangesAsync();
        return animalAccess;
    }

    /// <summary>
    ///     Deletes an AnimalAccess object asynchronously from the database.
    /// </summary>
    /// <param name="animalAccess">The AnimalAccess object to be deleted.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation. The task result contains the number of state entries written to the
    ///     database,
    ///     or null if the database is unavailable.
    /// </returns>
    public async Task<int?> DeleteAnimalAccessAsync(AnimalAccess animalAccess)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.AnimalAccesses.Remove(animalAccess);
        return await ctx.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves a list of AnimalID objects associated with a given UserID asynchronously from the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the animal IDs are being retrieved.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation. The task result contains an IEnumerable of Guid objects
    ///     representing the Animal IDs if the database is available,
    ///     otherwise, it returns null.
    /// </returns>
    public async Task<IEnumerable<Guid>?> GetAnimalIDsByUserIDAsync(Guid userId)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        return await ctx.AnimalAccesses
            .Where(a => a.UserID == userId)
            .Select(a => a.AnimalID)
            .ToListAsync();
    }


    /// <summary>
    ///     Checks asynchronously if a user has access to a specified animal in the database.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="animalId">The unique identifier of the animal.</param>
    /// <returns>
    ///     A Task representing the asynchronous operation. The task result contains a boolean value indicating whether the
    ///     user has access
    ///     to the specified animal. It returns false if the database is not available.
    /// </returns>
    public async Task<bool> UserHasAccessAsync(Guid userId, Guid animalId)
    {
        if (!await ctx.CheckDbIsAvailable())
            return false; // Or consider throwing an exception

        return await ctx.AnimalAccesses
            .AnyAsync(a => a.UserID == userId && a.AnimalID == animalId);
    }
}