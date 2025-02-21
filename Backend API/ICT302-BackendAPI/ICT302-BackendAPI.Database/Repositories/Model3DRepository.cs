using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Class responsible for handling CRUD operations for 3D models in the database.
/// </summary>
public class Model3DRepository(SchemaContext ctx) : IModel3DRepository
{
    /// <summary>
    ///     Asynchronously retrieves a list of 3D models from the database.
    /// </summary>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains an
    ///     enumerable list of 3D models, or null if the database is unavailable.
    /// </returns>
    public async Task<IEnumerable<Model3D>?> GetModel3DsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var models = await ctx.Model3D.ToListAsync();
        models.ForEach(a => ctx.Model3D.Attach(a));
        return models;
    }

    /// <summary>
    ///     Asynchronously retrieves a 3D model from the database using the provided graphics ID.
    /// </summary>
    /// <param name="graphicsId">The unique identifier for the graphics model.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the 3D model
    ///     if found, or null if the model does not exist or the database is unavailable.
    /// </returns>
    public async Task<Model3D?> GetModel3DFromGraphicsIdAsync(Guid graphicsId)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var models = await ctx.Model3D.ToListAsync();
        var model = models.Find(m => m.GPCID == graphicsId) ?? null;
        return model;
    }

    /// <summary>
    ///     Asynchronously retrieves a list of 3D models associated with a specific animal ID from the database.
    /// </summary>
    /// <param name="animalId">
    ///     The identifier for the animal whose associated 3D models are to be retrieved.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains a list of 3D models
    ///     associated with the specified animal, or null if the database is unavailable.
    /// </returns>
    public async Task<List<Model3D>?> GetModel3DListFromAnimalIdAsync(Guid animalId)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var models = await ctx.Model3D.ToListAsync();
        var graphics = await ctx.Graphics.ToListAsync();
        var animalsGraphics = graphics.Where(g => g.AnimalID == animalId);

        var animalsModels = new List<Model3D>();
        foreach (var g in animalsGraphics)
        {
            var model = models.Find(m => m.GPCID == g.GPCID);
            if (model != null)
                animalsModels.Add(model);
        }

        return animalsModels;
    }

    /// <summary>
    ///     Asynchronously retrieves a 3D model from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the 3D model to retrieve.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the 3D model if found,
    ///     or null if the model does not exist or the database is unavailable.
    /// </returns>
    public async Task<Model3D?> GetModel3DByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var model = await ctx.Model3D.FindAsync(id);
        if (model != null)
            ctx.Model3D.Attach(model);
        return model;
    }

    /// <summary>
    ///     Asynchronously creates a new 3D model in the database.
    /// </summary>
    /// <param name="model3D">The 3D model to be created.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the created 3D model,
    ///     or null if the database is unavailable.
    /// </returns>
    public async Task<Model3D?> CreateModel3DAsync(Model3D model3D)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Model3D.Attach(model3D);
        ctx.Model3D.Add(model3D);
        await ctx.SaveChangesAsync();
        return model3D;
    }

    /// <summary>
    ///     Asynchronously updates a 3D model in the database.
    /// </summary>
    /// <param name="model3D">The 3D model to be updated.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the updated 3D model, or null if the
    ///     database is unavailable.
    /// </returns>
    public async Task<Model3D?> UpdateModel3DAsync(Model3D model3D)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Model3D.Update(model3D);
        await ctx.SaveChangesAsync();
        return model3D;
    }

    /// <summary>
    ///     Asynchronously deletes a specified 3D model from the database.
    /// </summary>
    /// <param name="model3D">
    ///     The 3D model to be deleted.
    /// </param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the number of state entries written to the
    ///     database, or null if the database is unavailable.
    /// </returns>
    public async Task<int?> DeleteModel3DAsync(Model3D model3D)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Model3D.Remove(model3D);
        return await ctx.SaveChangesAsync();
    }
}