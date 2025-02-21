using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace ICT302_BackendAPI.Database.Models;

/// <summary>
///     Represents the database context for schema-related operations.
/// </summary>
public class SchemaContext : DbContext
{
    private const int CommandTimeout = 60;
    private readonly string _connectionString;
    private readonly ILogger<SchemaContext> _logger;

    /// <summary>Represents the database context for schema-related operations.</summary>
    public SchemaContext(IConfiguration configuration, ILogger<SchemaContext> logger)
    {
        _logger = logger;

        var fallbackConnectionString = "server=10.51.33.50;port=3306;user=api;password=APIPass!;database=it01-animals";
        var mainConnection = configuration.GetConnectionString("wildVisionDB");
        var backupConnection = configuration.GetConnectionString("wildVisionDB-Backup");
        _connectionString = mainConnection ?? backupConnection ?? fallbackConnectionString;
    }

    /// <summary>
    ///    Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<Model3D> Model3D { get; set; }
    /// <summary>
    ///    Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<AccessType> AccessTypes { get; set; }
    /// <summary>
    ///   Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<Animal> Animals { get; set; }
    /// <summary>
    ///   Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<AnimalAccess> AnimalAccesses { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<Billing> Billings { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<Graphic> Graphics { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<JobDetails> JobDetails { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<JobsCompleted> JobsCompleted { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<JobsPending> JobsPending { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<Organisation> Organisations { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<OrganisationAccess> OrganisationAccesses { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<OrgRequests> OrgRequests { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<Subscription> Subscriptions { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<Transaction> Transaction { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<TransactionType> TransactionType { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<User> Users { get; set; }
    /// <summary>
    ///  Represents the database context for schema-related operations.
    /// </summary>
    public DbSet<UserAccess> UserAccess { get; set; }

    /// <summary>
    ///     Configures the database context options such as the connection string and retry policies.
    /// </summary>
    /// <param name="optionsBuilder">An options builder used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            // Perform a preliminary connection check to ensure the database is available
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
            }


            optionsBuilder.UseMySQL(_connectionString, op =>
            {
                op.CommandTimeout(CommandTimeout);
                op.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            optionsBuilder.EnableSensitiveDataLogging();
        }
        catch (MySqlException e)
        {
            _logger.LogError(e, "Error while connecting to MySQL");
        }
    }

    /// <summary>
    ///     Configures the model to map it to the actual database tables and columns.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobsPending>()
            .Property(job => job.Status)
            .HasConversion(
                v => v.ToString(), // Converts job status enum to string
                v => (JobStatus)Enum.Parse(typeof(JobStatus), v) // Converts string to job status
            );
    }

    /**
     * <summary>Checks if the Database is available.</summary>
     * <returns>True if the database is available and false if not</returns>
     * <remarks>
     *     This function only trys to open a connection to the database and returns if the database is available.
     *     It does not check if the database is up-to-date with the current schema/creation
     * </remarks>
     */
    public async Task<bool> CheckDbIsAvailable()
    {
        try
        {
            await using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
            }
            _logger.LogInformation("Database is available");
        }
        catch (MySqlException e)
        {
            _logger.LogError("Database dependant functionality disabled...");
            _logger.LogError("Cannot open connection to MySQL: {message}", e.Message);
            if (e.Number == -2)
                _logger.LogError("MySQL connection timed out: {message}", e.Message);
            return false;
        }

        return true;
    }
}