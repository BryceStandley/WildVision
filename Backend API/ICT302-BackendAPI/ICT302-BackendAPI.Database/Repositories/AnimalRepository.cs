using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     The AnimalRepository class is responsible for handling CRUD operations for the Animal entity.
/// </summary>
public class AnimalRepository(SchemaContext ctx) : IAnimalRepository
{
    /// <summary>
    ///     Retrieves the list of all animals from the repository asynchronously.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an enumerable of Animal objects,
    ///     or null if the database is not available.
    /// </returns>
    public async Task<IEnumerable<Animal>?> GetAnimalsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var animals = await ctx.Animals.ToListAsync();
        animals.ForEach(a => ctx.Animals.Attach(a));
        return animals;
    }


    /// <summary>
    ///     Retrieves a specific animal by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the animal to be retrieved. Can be null.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the Animal object
    ///     associated with the specified identifier, or null if no such animal is found or the database is not available.
    /// </returns>
    public async Task<Animal?> GetAnimalByIdAsync(Guid? id)
    {
        if (id == null) return null;

        if (!await ctx.CheckDbIsAvailable())
            return null;

        // Use Include to eagerly load the related Graphics data
        var animal = await ctx.Animals
            .Include(a => a.Graphics)
            .FirstOrDefaultAsync(a =>
                a.AnimalID == id); // Use FirstOrDefaultAsync instead of FindAsync to support Include

        if (animal != null) ctx.Animals.Attach(animal); // Attach the entity to the context

        return animal;
    }


    /// <summary>
    ///     Creates a new animal entry in the repository asynchronously.
    /// </summary>
    /// <param name="animal">The animal object to be created and added to the repository.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the created Animal object,
    ///     or null if the database is not available.
    /// </returns>
    public async Task<Animal?> CreateAnimalAsync(Animal animal)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Animals.Attach(animal);
        ctx.Animals.Add(animal);
        await ctx.SaveChangesAsync();
        return animal;
    }


    /// <summary>
    ///     Updates the details of an existing animal in the repository asynchronously.
    /// </summary>
    /// <param name="animal">The Animal object containing updated information.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the updated Animal object,
    ///     or null if the database is not available.
    /// </returns>
    public async Task<Animal?> UpdateAnimalAsync(Animal animal)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Animals.Attach(animal);
        ctx.Animals.Update(animal);
        await ctx.SaveChangesAsync();
        return animal;
    }


    /// <summary>
    ///     Deletes the specified animal from the repository asynchronously.
    /// </summary>
    /// <param name="animal">The animal to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the number of state entries written to
    ///     the database,
    ///     or null if the database is not available.
    /// </returns>
    public async Task<int?> DeleteAnimalAsync(Animal animal)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Animals.Remove(animal);
        return await ctx.SaveChangesAsync();
    }


    /// <summary>
    ///     Updates the video data for a specific animal identified by the given animal ID asynchronously.
    /// </summary>
    /// <param name="animalId">The unique identifier of the animal to update.</param>
    /// <param name="videoFileName">The name of the video file associated with the animal.</param>
    /// <param name="uploadDate">The date when the video file was uploaded.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the updated Animal object,
    ///     or null if the animal is not found or the database is not available.
    /// </returns>
    public async Task<Animal?> UpdateAnimalVideoDataAsync(Guid animalId, string videoFileName, DateTime uploadDate)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var animal = await ctx.Animals.FindAsync(animalId);
        if (animal == null) return null; // Animal not found

        ctx.Animals.Update(animal);
        await ctx.SaveChangesAsync();

        return animal;
    }
}