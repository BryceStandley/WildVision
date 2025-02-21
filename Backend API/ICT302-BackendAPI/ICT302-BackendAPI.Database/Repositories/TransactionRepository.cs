using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Provides CRUD operations for managing transactions in the database.
/// </summary>
public class TransactionRepository(SchemaContext ctx) : ITransactionRepository
{
    /// <summary>
    ///     Asynchronously retrieves all transactions from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     an IEnumerable of Transaction objects if the database is available; otherwise, null.
    /// </returns>
    public async Task<IEnumerable<Transaction>?> GetTransactionsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var transactions = await ctx.Transaction.ToListAsync();
        transactions.ForEach(a => ctx.Transaction.Attach(a));
        return transactions;
    }

    /// <summary>
    ///     Asynchronously retrieves a transaction by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the transaction to retrieve.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the Transaction object if found; otherwise, null.
    /// </returns>
    public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var transaction = await ctx.Transaction.FindAsync(id);
        return transaction;
    }

    /// <summary>
    ///     Asynchronously creates a new transaction in the database.
    /// </summary>
    /// <param name="transaction">
    ///     The Transaction object that needs to be created.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the created Transaction object if the database is available; otherwise, null.
    /// </returns>
    public async Task<Transaction?> CreateTransactionAsync(Transaction transaction)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Transaction.Attach(transaction);
        ctx.Transaction.Add(transaction);
        await ctx.SaveChangesAsync();
        return transaction;
    }

    /// <summary>
    ///     Asynchronously updates the specified transaction in the database.
    /// </summary>
    /// <param name="transaction">
    ///     The transaction to be updated.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the updated Transaction object if the database is available; otherwise, null.
    /// </returns>
    public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Transaction.Update(transaction);
        await ctx.SaveChangesAsync();
        return transaction;
    }

    /// <summary>
    ///     Asynchronously deletes a transaction from the database.
    /// </summary>
    /// <param name="transaction">
    ///     The transaction object to be deleted.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     the number of state entries written to the database if the database is available; otherwise, null.
    /// </returns>
    public async Task<int?> DeleteTransactionAsync(Transaction transaction)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Transaction.Remove(transaction);
        return await ctx.SaveChangesAsync();
    }
}