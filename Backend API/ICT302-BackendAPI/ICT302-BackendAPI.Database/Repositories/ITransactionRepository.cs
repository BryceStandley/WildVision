using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Defines methods for interacting with the transactions in the database.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Asynchronously creates a new transaction in the database.
    /// </summary>
    /// <param name="transaction">
    /// The Transaction object that needs to be created.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the created Transaction object if the database is available; otherwise, null.
    /// </returns>
    Task<Transaction?> CreateTransactionAsync(Transaction transaction);

    /// <summary>
    /// Asynchronously deletes a transaction from the database.
    /// </summary>
    /// <param name="transaction">
    /// The transaction object to be deleted.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the number of state entries written to the database if the database is available; otherwise, null.
    /// </returns>
    Task<int?> DeleteTransactionAsync(Transaction transaction);

    /// <summary>
    /// Asynchronously retrieves a transaction from the database by its unique identifier.
    /// </summary>
    /// <param name="id">
    /// The unique identifier of the transaction to retrieve.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the
    /// Transaction object if found; otherwise, null.
    /// </returns>
    Task<Transaction?> GetTransactionByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves all transactions from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// an IEnumerable of Transaction objects if the database is available; otherwise, null.
    /// </returns>
    Task<IEnumerable<Transaction>?> GetTransactionsAsync();

    /// <summary>
    /// Asynchronously updates an existing transaction in the database.
    /// </summary>
    /// <param name="transaction">
    /// The Transaction object with updated values.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the updated Transaction object if the database is available; otherwise, null.
    /// </returns>
    Task<Transaction?> UpdateTransactionAsync(Transaction transaction);
}