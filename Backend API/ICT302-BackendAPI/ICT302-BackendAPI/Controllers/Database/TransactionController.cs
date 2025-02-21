using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Controller for handling transactions in the database.
/// </summary>
[Route("api/db")]
[ApiController]
public class TransactionController(ITransactionRepository transactionRepo, ILogger<TransactionController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Adds a new transaction asynchronously to the database.
    /// </summary>
    /// <param name="transaction">The transaction object to be added.</param>
    /// <returns>An ActionResult object representing the result of the operation.</returns>
    [HttpPost("transaction")]
    public async Task<ActionResult> AddTransactionAsync([FromBody] Transaction transaction)
    {
        try
        {
            transaction.TransactionId = Guid.NewGuid();
            return Ok(await transactionRepo.CreateTransactionAsync(transaction));
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
    ///     Retrieves a list of all transactions asynchronously from the database.
    /// </summary>
    /// <returns>An ActionResult object containing an enumerable list of Transaction objects.</returns>
    [HttpGet("transactions")]
    public async Task<ActionResult> GetTransactionsAsync()
    {
        try
        {
            var transactions = await transactionRepo.GetTransactionsAsync();
            return Ok(transactions);
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
    ///     Retrieves a transaction by its unique identifier asynchronously from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to be retrieved.</param>
    /// <returns>An IActionResult containing the transaction if found, otherwise a NotFound or error response.</returns>
    [HttpGet("transaction/{id}")]
    public async Task<IActionResult> GetTransactionById(Guid id)
    {
        try
        {
            var transaction = await transactionRepo.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(transaction);
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
    ///     Deletes an existing transaction from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction to be deleted.</param>
    /// <returns>An IActionResult object representing the result of the operation.</returns>
    [HttpDelete("transaction/{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        try
        {
            var existingTransaction = await transactionRepo.GetTransactionByIdAsync(id);
            if (existingTransaction == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await transactionRepo.DeleteTransactionAsync(existingTransaction);
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
    ///     Updates an existing transaction in the database asynchronously.
    /// </summary>
    /// <param name="transactionToUpdate">The transaction object containing updated information.</param>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    [HttpPut("transaction")]
    public async Task<IActionResult> UpdateTransaction([FromBody] Transaction transactionToUpdate)
    {
        try
        {
            var existingTransaction = await transactionRepo.GetTransactionByIdAsync(transactionToUpdate.TransactionId);
            if (existingTransaction == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingTransaction.TransTypeId = transactionToUpdate.TransTypeId;
            existingTransaction.UserId = transactionToUpdate.UserId;
            existingTransaction.AnimalId = transactionToUpdate.AnimalId;

            await transactionRepo.UpdateTransactionAsync(existingTransaction);
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