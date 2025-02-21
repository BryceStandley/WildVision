using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface for managing billing records in the database.
/// </summary>
public interface IBillingRepository
{
    /// <summary>
    /// Asynchronously creates a new billing entry in the database.
    /// If the database is not available, returns null.
    /// </summary>
    /// <param name="billing">The Billing object to be created in the database.</param>
    /// <returns>
    /// Returns a task that represents the asynchronous operation. The task result contains the created Billing object
    /// or null if the database is not available.
    /// </returns>
    Task<Billing?> CreateBillingAsync(Billing billing);

    /// <summary>
    /// Asynchronously deletes a billing record from the database.
    /// If the database is not available, returns null.
    /// </summary>
    /// <param name="billing">The billing record to be deleted.</param>
    /// <returns>
    /// Returns a task that represents the asynchronous operation. The task result contains the number of state
    /// entries written to the database or null if the database is not available.
    /// </returns>
    Task<int?> DeleteBillingAsync(Billing billing);

    /// <summary>
    /// Asynchronously retrieves a billing entry by its unique identifier from the database.
    /// If the billing entry does not exist or the database is unavailable, returns null.
    /// </summary>
    /// <param name="id">The unique identifier of the billing entry to be retrieved.</param>
    /// <returns>
    /// Returns a task that represents the asynchronous operation. The task result contains the Billing object
    /// or null if the billing entry does not exist or the database is unavailable.
    /// </returns>
    Task<Billing?> GetBillingByIDAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves a list of billings from the database.
    /// If the database is not available, returns null.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation. The task result contains an IEnumerable of Billing
    /// or null if the database is not available.
    /// </returns>
    Task<IEnumerable<Billing>?> GetBillingsAsync();

    /// <summary>
    /// Asynchronously updates a billing record in the database.
    /// If the database is not available, returns null.
    /// </summary>
    /// <param name="billing">The billing record to update.</param>
    /// <returns>
    /// Returns a task that represents the asynchronous operation. The task result contains the updated Billing object
    /// or null if the database is not available.
    /// </returns>
    Task<Billing?> UpdateBillingAsync(Billing billing);
}