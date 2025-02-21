using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Repository class for managing TransactionType entities.
/// </summary>
public class TransactionTypeRepository(SchemaContext ctx) : ITransactionTypeRepository
{
    /// <summary>
    ///     Retrieves a list of transaction types asynchronously.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     an IEnumerable of TransactionType representing the transaction types if the database is available,
    ///     otherwise, it returns null.
    /// </returns>
    public async Task<IEnumerable<TransactionType>?> GetTransactionTypesAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var transactionTypes = await ctx.TransactionType.ToListAsync();
        transactionTypes.ForEach(a => ctx.TransactionType.Attach(a));
        return transactionTypes;
    }

    /// <summary>
    ///     Retrieves a transaction type by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction type.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the TransactionType object if found, otherwise, it returns null.
    /// </returns>
    public async Task<TransactionType?> GetTransactionTypeByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var transactionType = await ctx.TransactionType.FindAsync(id);
        return transactionType;
    }

    /// <summary>
    ///     Creates a new transaction type asynchronously.
    /// </summary>
    /// <param name="transactionType">The transaction type entity to be created.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     a TransactionType object if the transaction type is successfully created,
    ///     otherwise, it returns null if the database is unavailable.
    /// </returns>
    public async Task<TransactionType?> CreateTransactionTypeAsync(TransactionType transactionType)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.TransactionType.Attach(transactionType);
        ctx.TransactionType.Add(transactionType);
        await ctx.SaveChangesAsync();
        return transactionType;
    }

    /// <summary>
    ///     Updates an existing transaction type asynchronously.
    /// </summary>
    /// <param name="transactionType">
    ///     The transaction type entity to be updated.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the updated TransactionType if the database update is successful, otherwise, it returns null.
    /// </returns>
    public async Task<TransactionType?> UpdateTransactionTypeAsync(TransactionType transactionType)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.TransactionType.Update(transactionType);
        await ctx.SaveChangesAsync();
        return transactionType;
    }

    /// <summary>
    ///     Deletes a specified transaction type asynchronously.
    /// </summary>
    /// <param name="transactionType">
    ///     The TransactionType object to be deleted.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the number of state entries written to the database if the database is available,
    ///     otherwise, it returns null.
    /// </returns>
    public async Task<int?> DeleteTransactionTypeAsync(TransactionType transactionType)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.TransactionType.Remove(transactionType);
        return await ctx.SaveChangesAsync();
    }
}