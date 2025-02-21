using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Controller responsible for handling operations related to the TransactionType entity.
/// </summary>
[Route("api/db")]
[ApiController]
public class TransactionTypeController(
    ITransactionTypeRepository transactionTypeRepo,
    ILogger<TransactionTypeController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Asynchronously adds a new TransactionType entity to the repository.
    /// </summary>
    /// <param name="transactionType">The TransactionType entity to be added. Must be provided in the request body.</param>
    /// <returns>
    ///     An ActionResult representing the outcome of the operation. On success, returns an OkResult containing the
    ///     created TransactionType. Otherwise, returns an appropriate error response.
    /// </returns>
    [HttpPost("transactiontype")]
    public async Task<ActionResult> AddTransactionTypeAsync([FromBody] TransactionType transactionType)
    {
        try
        {
            transactionType.TransTypeId = Guid.NewGuid();
            return Ok(await transactionTypeRepo.CreateTransactionTypeAsync(transactionType));
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
    ///     Asynchronously retrieves a list of all TransactionType entities from the repository.
    /// </summary>
    /// <returns>
    ///     An ActionResult representing the outcome of the operation. On success, returns an OkResult containing a list
    ///     of TransactionType entities. Otherwise, returns an appropriate error response.
    /// </returns>
    [HttpGet("transactiontypes")]
    public async Task<ActionResult> GetTransactionTypesAsync()
    {
        try
        {
            var transactionTypes = await transactionTypeRepo.GetTransactionTypesAsync();
            return Ok(transactionTypes);
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
    ///     Asynchronously retrieves a TransactionType entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the TransactionType entity.</param>
    /// <returns>
    ///     An ActionResult containing the TransactionType entity if found. If not found, returns a NotFoundResult. If an
    ///     error occurs, returns an appropriate error response.
    /// </returns>
    [HttpGet("transactiontype/{id}")]
    public async Task<IActionResult> GetTransactionTypeById(Guid id)
    {
        try
        {
            var transactionType = await transactionTypeRepo.GetTransactionTypeByIdAsync(id);
            if (transactionType == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(transactionType);
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
    ///     Asynchronously deletes an existing TransactionType entity from the repository using its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the TransactionType entity to be deleted.</param>
    /// <returns>
    ///     An IActionResult representing the outcome of the operation.
    ///     Returns NoContent on successful deletion. If the TransactionType is not found, returns a NotFound result.
    ///     In case of an error, returns a 500 Internal Server Error with an appropriate message.
    /// </returns>
    [HttpDelete("transactiontype/{id}")]
    public async Task<IActionResult> DeleteTransactionType(Guid id)
    {
        try
        {
            var existingTransactionType = await transactionTypeRepo.GetTransactionTypeByIdAsync(id);
            if (existingTransactionType == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await transactionTypeRepo.DeleteTransactionTypeAsync(existingTransactionType);
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
    ///     Asynchronously updates an existing TransactionType entity in the repository.
    /// </summary>
    /// <param name="transactionTypeToUpdate">The TransactionType entity to be updated. Must be provided in the request body.</param>
    /// <returns>
    ///     An IActionResult representing the outcome of the operation. On success, returns a NoContentResult. Otherwise,
    ///     returns an appropriate error response.
    /// </returns>
    [HttpPut("transactiontype")]
    public async Task<IActionResult> UpdateTransactionType([FromBody] TransactionType transactionTypeToUpdate)
    {
        try
        {
            var existingTransactionType =
                await transactionTypeRepo.GetTransactionTypeByIdAsync(transactionTypeToUpdate.TransTypeId);
            if (existingTransactionType == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingTransactionType.TransDetails = transactionTypeToUpdate.TransDetails;

            await transactionTypeRepo.UpdateTransactionTypeAsync(existingTransactionType);
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