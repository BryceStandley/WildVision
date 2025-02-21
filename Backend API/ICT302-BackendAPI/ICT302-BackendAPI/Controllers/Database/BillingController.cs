using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Handles HTTP requests related to billing operations.
/// </summary>
[Route("api/db")]
[ApiController]
public class BillingController(IBillingRepository billingRepo, ILogger<BillingController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Asynchronously adds a new billing record to the database.
    /// </summary>
    /// <param name="billing">The billing object to be added.</param>
    /// <returns>
    ///     An action result indicating the outcome of the operation. If successful, returns the created billing object;
    ///     otherwise, returns a 500 status code with an error message.
    /// </returns>
    [HttpPost("billing")]
    public async Task<ActionResult> AddBillingAsync([FromBody] Billing billing)
    {
        try
        {
            billing.BillingID = Guid.NewGuid();
            return Ok(await billingRepo.CreateBillingAsync(billing));
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
    ///     Asynchronously retrieves all billing records from the database.
    /// </summary>
    /// <returns>
    ///     An action result containing a list of all billing records. If successful, returns an OK status with the list;
    ///     otherwise, returns a 500 status code with an error message.
    /// </returns>
    [HttpGet("billings")]
    public async Task<ActionResult> GetBillingsAsync()
    {
        try
        {
            var billings = await billingRepo.GetBillingsAsync();
            return Ok(billings);
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
    ///     Asynchronously retrieves a billing record by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the billing record.</param>
    /// <returns>
    ///     An action result containing the billing record. If successful, returns an OK status with the billing record;
    ///     otherwise, returns a 404 status code if the record is not found, or a 500 status code with an error message if an
    ///     error occurs.
    /// </returns>
    [HttpGet("billing/{id}")]
    public async Task<IActionResult> GetBillingById(Guid id)
    {
        try
        {
            var billing = await billingRepo.GetBillingByIDAsync(id);
            if (billing == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(billing);
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
    ///     Asynchronously deletes a billing record from the database based on the provided ID.
    /// </summary>
    /// <param name="id">The unique identifier of the billing record to be deleted.</param>
    /// <returns>
    ///     An action result indicating the outcome of the operation. If the record is successfully deleted, returns a
    ///     NoContent status; if the record is not found, returns a 404 status code with an error message; otherwise, returns a
    ///     500 status code with an error message.
    /// </returns>
    [HttpDelete("billing/{id}")]
    public async Task<IActionResult> DeleteBilling(Guid id)
    {
        try
        {
            var existingBilling = await billingRepo.GetBillingByIDAsync(id);
            if (existingBilling == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await billingRepo.DeleteBillingAsync(existingBilling);
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
    ///     Asynchronously updates an existing billing record in the database.
    /// </summary>
    /// <param name="billingToUpdate">The billing object containing updated information.</param>
    /// <returns>
    ///     An action result indicating the outcome of the operation. If successful, returns NoContent; otherwise, returns
    ///     a status code with an error message.
    /// </returns>
    [HttpPut("billing")]
    public async Task<IActionResult> UpdateBilling([FromBody] Billing billingToUpdate)
    {
        try
        {
            var existingBilling = await billingRepo.GetBillingByIDAsync(billingToUpdate.BillingID);
            if (existingBilling == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingBilling.UserID = billingToUpdate.UserID;

            await billingRepo.UpdateBillingAsync(existingBilling);
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