using ICT302_BackendAPI.Database.Models;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Interface representing a repository for managing transaction types.
/// </summary>
public interface ITransactionTypeRepository
{
    /// <summary>
    /// Creates a new transaction type asynchronously.
    /// </summary>
    /// <param name="transactionType">The transaction type entity to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a TransactionType object if the transaction type is successfully created,
    /// otherwise, it returns null if the database is unavailable.
    /// </returns>
    Task<TransactionType?> CreateTransactionTypeAsync(TransactionType transactionType);

    /// <summary>
    /// Deletes a specified transaction type asynchronously.
    /// </summary>
    /// <param name="transactionType">
    /// The TransactionType object to be deleted.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the number of state entries written to the database if the database is available,
    /// otherwise, it returns null.
    /// </returns>
    Task<int?> DeleteTransactionTypeAsync(TransactionType transactionType);

    /// <summary>
    /// Retrieves a transaction type by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction type to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// a TransactionType object if the transaction type is found; otherwise, null if not found or if the database is unavailable.
    /// </returns>
    Task<TransactionType?> GetTransactionTypeByIdAsync(Guid id);

    /// <summary>
    /// Retrieves a list of transaction types asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// an IEnumerable of TransactionType representing the transaction types if the database is available,
    /// otherwise, it returns null.
    /// </returns>
    Task<IEnumerable<TransactionType>?> GetTransactionTypesAsync();

    /// <summary>
    /// Updates an existing transaction type asynchronously.
    /// </summary>
    /// <param name="transactionType">
    /// The transaction type entity to be updated.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the updated TransactionType if the database update is successful, otherwise, it returns null.
    /// </returns>
    Task<TransactionType?> UpdateTransactionTypeAsync(TransactionType transactionType);
}