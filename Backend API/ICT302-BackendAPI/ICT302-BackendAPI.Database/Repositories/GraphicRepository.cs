// GraphicRepository.cs

using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     The GraphicRepository class is responsible for handling CRUD operations
///     related to the Graphic entities in the database.
/// </summary>
public class GraphicRepository(SchemaContext ctx) : IGraphicRepository
{
    /// <summary>
    ///     Asynchronously fetches a list of all Graphic entities from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing a collection of Graphic entities if the database
    ///     is available; otherwise, null.
    /// </returns>
    public async Task<IEnumerable<Graphic>?> GetGraphicsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var graphics = await ctx.Graphics.ToListAsync();
        graphics.ForEach(a => ctx.Graphics.Attach(a));
        return graphics;
    }

    /// <summary>
    ///     Asynchronously creates a new Graphic entity in the database.
    /// </summary>
    /// <param name="graphic">The Graphic entity to be created.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the created Graphic entity if the database is
    ///     available; otherwise, null.
    /// </returns>
    public async Task<Graphic?> CreateGraphicAsync(Graphic graphic)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Graphics.Attach(graphic);
        ctx.Graphics.Add(graphic);
        await ctx.SaveChangesAsync();
        return graphic;
    }

    /// <summary>
    ///     Asynchronously updates an existing Graphic entity in the database.
    /// </summary>
    /// <param name="graphic">The Graphic entity to be updated.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing the updated Graphic entity if the operation is
    ///     successful; otherwise, null.
    /// </returns>
    public async Task<Graphic?> UpdateGraphicAsync(Graphic graphic)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Graphics.Update(graphic);
        await ctx.SaveChangesAsync();
        return graphic;
    }

    /// <summary>
    ///     Asynchronously retrieves a Graphic entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Graphic entity to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation, containing the Graphic entity if found and the database is
    ///     available; otherwise, null.
    /// </returns>
    public async Task<Graphic?> GetGraphicByIDAsync(Guid? id)
    {
        if (id == null) return null;

        if (!await ctx.CheckDbIsAvailable())
            return null;


        var graphic = await ctx.Graphics.FindAsync(id);
        if (graphic != null)
            ctx.Graphics.Attach(graphic);
        return graphic;
    }

    /// <summary>
    ///     Asynchronously fetches a Graphic entity from the database based on the provided file name.
    /// </summary>
    /// <param name="fileName">The file name of the graphic to be fetched.</param>
    /// <returns>A task that represents the asynchronous operation, containing the Graphic entity if found; otherwise, null.</returns>
    public async Task<Graphic?> GetGraphicByFileNameAsync(string fileName)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var graphics = await ctx.Graphics.ToListAsync();
        var graphic = graphics.Find(fn => fn.FilePath == fileName);

        if (graphic != null)
            ctx.Graphics.Attach(graphic);
        return graphic;
    }

    /// <summary>
    ///     Asynchronously deletes a specified Graphic entity from the database.
    /// </summary>
    /// <param name="graphic">The Graphic entity to be deleted.</param>
    /// <returns>
    ///     A task representing the asynchronous operation, containing the number of state entries written to the database
    ///     if the operation is successful; otherwise, null if the database is not available.
    /// </returns>
    public async Task<int?> DeleteGraphicAsync(Graphic graphic)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Graphics.Remove(graphic);
        return await ctx.SaveChangesAsync();
    }
}