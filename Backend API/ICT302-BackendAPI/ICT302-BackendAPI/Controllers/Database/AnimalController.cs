using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     AnimalController is a REST API controller for managing animal records.
///     It provides endpoints to create, retrieve, update, and delete animal data in the database.
/// </summary>
[Route("api/db")]
[ApiController]
public class AnimalController(IAnimalRepository animalRepo, ILogger<AnimalController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new animal to the database asynchronously.
    /// </summary>
    /// <param name="animalName">The name of the animal to add.</param>
    /// <param name="animalType">The type of the animal to add.</param>
    /// <param name="animalDOB">The date of birth of the animal to add.</param>
    /// <returns>An ActionResult indicating the outcome of the add operation.</returns>
    [HttpPost("animals/{animalName}&{animalType}&{animalDOB}")]
    public async Task<ActionResult> AddAnimalAsync(string animalName, string animalType, DateTime animalDOB)
    {
        try
        {
            var animal = new Animal();
            animal.AnimalID = Guid.NewGuid();
            animal.AnimalName = animalName;
            animal.AnimalType = animalType;
            animal.AnimalDOB = animalDOB;
            return Ok(await animalRepo.CreateAnimalAsync(animal));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Retrieves a list of all animals asynchronously from the database.
    /// </summary>
    /// <returns>An ActionResult containing an IEnumerable of Animal objects.</returns>
    [HttpGet("animals")]
    public async Task<ActionResult> GetAnimalsAsync()
    {
        try
        {
            var animals = await animalRepo.GetAnimalsAsync();
            return Ok(animals);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Retrieves an animal by its ID asynchronously from the database.
    /// </summary>
    /// <param name="id">The ID of the animal to retrieve.</param>
    /// <returns>An ActionResult containing the Animal object.</returns>
    [HttpGet("animal/{id}")]
    public async Task<IActionResult> GetAnimalById(Guid id)
    {
        try
        {
            var animal = await animalRepo.GetAnimalByIdAsync(id);
            if (animal == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "record not found"
                });
            return Ok(animal);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Deletes an animal by its ID asynchronously from the database.
    /// </summary>
    /// <param name="id">The ID of the animal to delete.</param>
    /// <returns>An IActionResult indicating the outcome of the delete operation.</returns>
    [HttpDelete("animal/{id}")]
    public async Task<IActionResult> DeleteAnimal(Guid id)
    {
        try
        {
            var existingAnimal = await animalRepo.GetAnimalByIdAsync(id);
            if (existingAnimal == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "record not found"
                });

            await animalRepo.DeleteAnimalAsync(existingAnimal);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Updates an existing animal asynchronously in the database.
    /// </summary>
    /// <param name="animalToUpdate">The animal object with updated data.</param>
    /// <returns>An IActionResult indicating the outcome of the update operation.</returns>
    [HttpPut("animal")]
    public async Task<IActionResult> UpdateAnimal(Animal animalToUpdate)
    {
        try
        {
            var existingAnimal = await animalRepo.GetAnimalByIdAsync(animalToUpdate.AnimalID);
            if (existingAnimal == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "record not found"
                });
            existingAnimal.AnimalID = animalToUpdate.AnimalID;
            existingAnimal.AnimalName = animalToUpdate.AnimalName;
            existingAnimal.AnimalDOB = animalToUpdate.AnimalDOB;
            existingAnimal.AnimalType = animalToUpdate.AnimalType;
            await animalRepo.UpdateAnimalAsync(existingAnimal);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }
}