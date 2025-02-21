using ICT302_BackendAPI.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT302_BackendAPI.Database.Repositories;

/// <summary>
///     Provides methods for managing billing records in the database.
/// </summary>
public class BillingRepository(SchemaContext ctx) : IBillingRepository
{
    /// <summary>
    ///     Asynchronously retrieves a list of billings from the database.
    ///     If the database is not available, returns null.
    /// </summary>
    /// <returns>
    ///     Returns a task that represents the asynchronous operation. The task result contains an IEnumerable of Billing
    ///     or null if the database is not available.
    /// </returns>
    public async Task<IEnumerable<Billing>?> GetBillingsAsync()
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var billings = await ctx.Billings.ToListAsync();
        billings.ForEach(a => ctx.Billings.Attach(a));
        return billings;
    }

    /// <summary>
    ///     Asynchronously retrieves a billing record by its unique identifier.
    ///     If the database is not available, returns null.
    /// </summary>
    /// <param name="id">The unique identifier of the billing record to retrieve.</param>
    /// <returns>
    ///     Returns a task that represents the asynchronous operation. The task result contains the Billing object if
    ///     found, otherwise null.
    /// </returns>
    public async Task<Billing?> GetBillingByIDAsync(Guid id)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        var billing = await ctx.Billings.FindAsync(id);
        return billing;
    }

    /// <summary>
    ///     Asynchronously creates a new billing entry in the database.
    ///     If the database is not available, returns null.
    /// </summary>
    /// <param name="billing">The Billing object to be created in the database.</param>
    /// <returns>
    ///     Returns a task that represents the asynchronous operation. The task result contains the created Billing object
    ///     or null if the database is not available.
    /// </returns>
    public async Task<Billing?> CreateBillingAsync(Billing billing)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Billings.Attach(billing);
        ctx.Billings.Add(billing);
        await ctx.SaveChangesAsync();
        return billing;
    }

    /// <summary>
    ///     Asynchronously updates a billing record in the database.
    ///     If the database is not available, returns null.
    /// </summary>
    /// <param name="billing">The billing record to update.</param>
    /// <returns>
    ///     Returns a task that represents the asynchronous operation. The task result contains the updated Billing object
    ///     or null if the database is not available.
    /// </returns>
    public async Task<Billing?> UpdateBillingAsync(Billing billing)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Billings.Update(billing);
        await ctx.SaveChangesAsync();
        return billing;
    }

    /// <summary>
    ///     Asynchronously deletes a billing record from the database.
    ///     If the database is not available, returns null.
    /// </summary>
    /// <param name="billing">The billing record to be deleted.</param>
    /// <returns>
    ///     Returns a task that represents the asynchronous operation. The task result contains the number of state
    ///     entries written to the database or null if the database is not available.
    /// </returns>
    public async Task<int?> DeleteBillingAsync(Billing billing)
    {
        if (!await ctx.CheckDbIsAvailable())
            return null;

        ctx.Billings.Remove(billing);
        return await ctx.SaveChangesAsync();
    }
}