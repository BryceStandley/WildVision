using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     The SubscriptionController class provides API endpoints for managing subscriptions in the system.
/// </summary>
[Route("api/db")]
[ApiController]
public class SubscriptionController(
    ISubscriptionRepository subscriptionRepo,
    ILogger<SubscriptionController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Asynchronously adds a new subscription to the database.
    /// </summary>
    /// <param name="subscription">The subscription details to add to the database.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an ActionResult representing the
    ///     outcome of the operation.
    /// </returns>
    [HttpPost("subscription")]
    public async Task<ActionResult> AddSubscriptionAsync([FromBody] Subscription subscription)
    {
        try
        {
            subscription.SubscriptionId = Guid.NewGuid();
            return Ok(await subscriptionRepo.CreateSubscriptionAsync(subscription));
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
    ///     Asynchronously retrieves all subscriptions from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an ActionResult with a list of
    ///     subscriptions.
    /// </returns>
    [HttpGet("subscriptions")]
    public async Task<ActionResult> GetSubscriptionsAsync()
    {
        try
        {
            var subscriptions = await subscriptionRepo.GetSubscriptionsAsync();
            return Ok(subscriptions);
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
    ///     Asynchronously retrieves a specific subscription by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an IActionResult with the
    ///     subscription if found, or a 404 Not Found status if the subscription does not
    /// </returns>
    [HttpGet("subscription/{id}")]
    public async Task<IActionResult> GetSubscriptionById(Guid id)
    {
        try
        {
            var subscription = await subscriptionRepo.GetSubscriptionByIdAsync(id);
            if (subscription == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(subscription);
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
    ///     Asynchronously deletes a subscription identified by the given ID from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the subscription to be deleted.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains an IActionResult indicating the
    ///     success or failure of the operation.
    /// </returns>
    [HttpDelete("subscription/{id}")]
    public async Task<IActionResult> DeleteSubscription(Guid id)
    {
        try
        {
            var existingSubscription = await subscriptionRepo.GetSubscriptionByIdAsync(id);
            if (existingSubscription == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            await subscriptionRepo.DeleteSubscriptionAsync(existingSubscription);
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
    ///     Updates an existing subscription in the database.
    /// </summary>
    /// <param name="subscriptionToUpdate">The updated subscription details.</param>
    /// <returns>An IActionResult representing the result of the update operation.</returns>
    [HttpPut("subscription")]
    public async Task<IActionResult> UpdateSubscription([FromBody] Subscription subscriptionToUpdate)
    {
        try
        {
            var existingSubscription =
                await subscriptionRepo.GetSubscriptionByIdAsync(subscriptionToUpdate.SubscriptionId);
            if (existingSubscription == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingSubscription.SubscriptionTitle = subscriptionToUpdate.SubscriptionTitle;
            existingSubscription.StorageSize = subscriptionToUpdate.StorageSize;
            existingSubscription.ChargeRate = subscriptionToUpdate.ChargeRate;

            await subscriptionRepo.UpdateSubscriptionAsync(existingSubscription);
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