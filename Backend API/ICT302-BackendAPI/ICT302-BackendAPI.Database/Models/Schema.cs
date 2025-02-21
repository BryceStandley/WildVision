using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618
namespace ICT302_BackendAPI.Database.Models;

/// <summary>
///     Represents an access type in the application.
/// </summary>
[Table("accesstype")]
public class AccessType
{
    /// <summary>
    ///    Gets or sets the unique identifier for the access type.
    /// </summary>
    [Key]
    [Column("AccessType_ID", TypeName = "binary(16)")]
    public Guid AccessTypeID { get; set; }

    /// <summary>
    ///   Gets or sets the details of the access type.
    /// </summary>
    [Required]
    [Column("AccessType_Details", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string AccessTypeDetails { get; set; }
}

/// <summary>
///     Represents an animal and its related properties.
/// </summary>
[Table("animal")]
public class Animal
{
    /// <summary>
    ///    Gets or sets the unique identifier for the animal.
    /// </summary>
    [Key]
    [Column("Animal_ID", TypeName = "binary(16)")]
    public Guid AnimalID { get; set; }

    /// <summary>
    ///   Gets or sets the name of the animal.
    /// </summary>
    [Required]
    [Column("Animal_Name", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string AnimalName { get; set; }

    /// <summary>
    ///   Gets or sets the date of birth of the animal.
    /// </summary>
    [Required]
    [Column("Animal_DOB", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime AnimalDOB { get; set; }

    /// <summary>
    ///  Gets or sets the type of the animal.
    /// </summary>
    [Required]
    [Column("Animal_Type", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string AnimalType { get; set; }

    // Navigation property
    /// <summary>
    ///    Gets or sets the collection of graphics associated with the animal.
    /// </summary>
    public virtual ICollection<Graphic> Graphics { get; set; } = new List<Graphic>();
}

/// <summary>
///     Represents an access record for an animal.
/// </summary>
[Table("animalaccess")]
public class AnimalAccess
{
    /// <summary>
    ///   Gets or sets the unique identifier for the access record.
    /// </summary>
    [Key]
    [Column("Access_ID", TypeName = "binary(16)")]
    public Guid AccessID { get; set; }

    /// <summary>
    ///  Gets or sets the type of access granted.
    /// </summary>
    [Required]
    [Column("Access_Type", TypeName = "varchar(25)")]
    [StringLength(25)]
    public string AccessType { get; set; }

    /// <summary>
    /// Gets or sets the date the access was granted.
    /// </summary>
    [Required]
    [Column("Assigned_Date", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime AssignedDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the animal.
    /// </summary>
    [Required]
    [Column("Animal_ID", TypeName = "binary(16)")]
    public Guid AnimalID { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    [Required]
    [Column("User_ID", TypeName = "binary(16)")]
    public Guid UserID { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("AnimalID")] public virtual Animal Animal { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("UserID")] public virtual User User { get; set; }
}

/// <summary>
///     Represents a billing record in the application.
/// </summary>
[Table("billing")]
public class Billing
{
    /// <summary>
    ///   Gets or sets the unique identifier for the billing record.
    /// </summary>
    [Key]
    [Column("Billing_ID", TypeName = "binary(16)")]
    public Guid BillingID { get; set; }

    /// <summary>
    /// Gets or sets the date the billing record was created.
    /// </summary>
    [Required]
    [Column("GPC_ID", TypeName = "binary(16)")]
    public Guid GPCID { get; set; }

    /// <summary>
    /// Gets or sets the date the billing record was created.
    /// </summary>
    [Required]
    [Column("Job_ID", TypeName = "binary(16)")]
    public Guid JobID { get; set; }

    /// <summary>
    /// Gets or sets the date the billing record was created.
    /// </summary>
    [Required]
    [Column("User_ID", TypeName = "binary(16)")]
    public Guid UserID { get; set; }

    /// <summary>
    /// Gets or sets the date the billing record was created.
    /// </summary>
    [Required]
    [Column("Subscription_ID", TypeName = "binary(16)")]
    public Guid SubscriptionID { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("GPCID")] public virtual Graphic Graphic { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("JobID")] public virtual JobsCompleted JobsCompleted { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("UserID")] public virtual User User { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("SubscriptionID")] public virtual Subscription Subscription { get; set; }
}

/// <summary>
///     Represents a graphic entity associated with an animal.
/// </summary>
[Table("graphic")]
public class Graphic
{
    /// <summary>
    ///  Gets or sets the unique identifier for the graphic entity.
    /// </summary>
    [Key]
    [Column("GPC_ID", TypeName = "binary(16)")]
    public Guid GPCID { get; set; }

    /// <summary>
    ///     Gets or sets the name of the graphic entity.
    /// </summary>
    [Required]
    [Column("GPC_Name", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string GPCName { get; set; }

    /// <summary>
    ///    Gets or sets the date the graphic entity was uploaded.
    /// </summary>
    [Required]
    [Column("GPC_Date_Upload", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime GPCDateUpload { get; set; }

    /// <summary>
    ///   Gets or sets the file path of the graphic entity.
    /// </summary>
    [Required]
    [Column("File_Path", TypeName = "varchar(255)")]
    [StringLength(45)]
    public string FilePath { get; set; }

    /// <summary>
    ///  Gets or sets the unique identifier of the animal.
    /// </summary>
    [Required]
    [Column("Animal_ID", TypeName = "binary(16)")]
    public Guid AnimalID { get; set; }

    /// <summary>
    /// Gets or sets the size of the graphic entity.
    /// </summary>
    [Required]
    [Column("GPC_Size", TypeName = "int")]
    public int GPCSize { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("AnimalID")] public virtual Animal? Animal { get; set; }
}

/// <summary>
///     Represents the details of a job in the system.
/// </summary>
[Table("jobdetails")]
public class JobDetails
{
    /// <summary>
    ///   Gets or sets the unique identifier for the job details.
    /// </summary>
    [Key]
    [Column("JD_ID", TypeName = "binary(16)")]
    public Guid JDID { get; set; }

    /// <summary>
    ///  Gets or sets graphic id related to the job.
    /// </summary>
    [Required]
    [Column("GPC_ID", TypeName = "binary(16)")]
    public Guid GPCID { get; set; }

    /// <summary>
    /// Gets or sets model id of the model used for the job.
    /// </summary>
    [Required]
    [Column("Model_ID", TypeName = "binary(16)")]
    public Guid ModelID { get; set; }

    /// <summary>
    /// Gets or sets the gen type of the job.
    /// </summary>
    [Required]
    [Column("Model_Gen_Type", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string ModelGenType { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("GPCID")] public virtual Graphic? Graphic { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("ModelID")] public virtual Model3D? Model3D { get; set; }
}

/// <summary>
///     Represents a completed job in the system.
/// </summary>
[Table("jobscompleted")]
public class JobsCompleted
{
    /// <summary>
    ///  Gets or sets the unique identifier for the job.
    /// </summary>
    [Key]
    [Column("Job_ID", TypeName = "binary(16)")]
    public Guid JobID { get; set; }

    /// <summary>
    /// Gets or sets the type of job.
    /// </summary>
    [Required]
    [Column("Job_Type", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string JobType { get; set; }

    /// <summary>
    /// Gets or sets the date the job started.
    /// </summary>
    [Required]
    [Column("Jobs_Start", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime JobsStart { get; set; }

    /// <summary>
    /// Gets or sets the date the job ended.
    /// </summary>
    [Required]
    [Column("Jobs_End", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime JobsEnd { get; set; }

    /// <summary>
    /// Gets or sets the size of the job.
    /// </summary>
    [Required]
    [Column("Job_Size", TypeName = "int")]
    public int JobSize { get; set; }

    /// <summary>
    /// Gets or sets the job details of the job.
    /// </summary>
    [Required]
    [Column("JD_ID", TypeName = "binary(16)")]
    public Guid JDID { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("JDID")] public virtual JobDetails? JobDetails { get; set; }
}

/// <summary>
///     Represents a pending job in the system.
/// </summary>
[Table("jobspending")]
public class JobsPending
{
    /// <summary>
    /// Gets or sets the queue position for the job.
    /// </summary>
    [Key]
    [Column("Queue_Number", TypeName = "int")]
    public int QueueNumber { get; set; } = -1;

    /// <summary>
    /// Gets or sets the date the job was added.
    /// </summary>
    [Required]
    [Column("Job_Added", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime JobAdded { get; set; }

    /// <summary>
    /// Gets or sets the status of the job.
    /// </summary>
    [Required]
    [Column("Status", TypeName = "char(35)")]
    [StringLength(35)]
    public JobStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the job details.
    /// </summary>
    [Required]
    [Column("JD_ID", TypeName = "binary(16)")]
    public Guid JobDetailsId { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("JDID")] public virtual JobDetails JobDetails { get; set; }
}

/// <summary>
///     Represents a 3D model associated with a graphic.
/// </summary>
[Table("model3d")]
public class Model3D
{
    /// <summary>
    /// Gets or sets the unique identifier for the model.
    /// </summary>
    [Key]
    [Column("Model_ID", TypeName = "binary(16)")]
    public Guid ModelID { get; set; }

    /// <summary>
    /// Gets or sets the title of the model.
    /// </summary>
    [Required]
    [Column("Model_Title", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string ModelTitle { get; set; }

    /// <summary>
    /// Gets or sets the date the model was generated.
    /// </summary>
    [Required]
    [Column("Model_Date_Gen", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime ModelDateGen { get; set; }

    /// <summary>
    /// Gets or sets the file path of the model.
    /// </summary>
    [Required]
    [Column("File_Path", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string FilePath { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the graphic.
    /// </summary>
    [Required]
    [Column("GPC_ID", TypeName = "binary(16)")]
    public Guid GPCID { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("GPCID")] public virtual Graphic? Graphic { get; set; }
}

/// <summary>
///     Represents an organization's request in the application.
/// </summary>
[Table("org_requests")]
public class OrgRequests
{
    /// <summary>
    /// Gets or sets the unique identifier for the request.
    /// </summary>
    [Key]
    [Column("Request_ID", TypeName = "binary(16)")]
    public Guid RequestID { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the organization.
    /// </summary>
    [Required]
    [Column("Org_ID", TypeName = "binary(16)")]
    public Guid OrgID { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    [Required]
    [Column("User_ID", TypeName = "binary(16)")]
    public Guid UserID { get; set; }

    /// <summary>
    /// Gets or sets the date the request was made.
    /// </summary>
    [Required]
    [Column("Date_Requested", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime DateRequested { get; set; }

    /// <summary>
    /// Gets or sets the date the request was processed.
    /// </summary>
    [Column("Date_Processed", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime? DateProcessed { get; set; }

    /// <summary>
    /// Gets or sets the status of the request.
    /// </summary>
    [Column("Status", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string Status { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("OrgID")] public virtual Organisation Organisation { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("UserID")] public virtual User User { get; set; }
}

/// <summary>
///     Represents an organisation within the application.
/// </summary>
[Table("organisation")]
public class Organisation
{
    /// <summary>
    ///     Gets or sets the unique identifier for the organisation.
    /// </summary>
    [Key]
    [Column("Org_ID", TypeName = "binary(16)")]
    public Guid OrgID { get; set; }

    /// <summary>
    ///    Gets or sets the name of the organisation.
    /// </summary>
    [Required]
    [Column("Org_Name", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string OrgName { get; set; }

    /// <summary>
    ///   Gets or sets the email address of the organisation.
    /// </summary>
    [Column("Org_Email", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string? OrgEmail { get; set; }
}

/// <summary>
///     Represents the access levels assigned to an organisation within the application.
/// </summary>
[Table("organisationaccess")]
public class OrganisationAccess
{
    /// <summary>
    ///    Gets or sets the unique identifier for the organisation access record.
    /// </summary>
    [Key]
    [Column("OrgAccess_ID", TypeName = "binary(16)")]
    public Guid OrgAccessID { get; set; }

    /// <summary>
    ///  Gets or sets the type of access granted.
    /// </summary>
    [Required]
    [Column("Access_Type", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string AccessType { get; set; }

    /// <summary>
    /// Gets or sets the date the access was granted.
    /// </summary>
    [Required]
    [Column("Assigned_Date", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime AssignedDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the organisation.
    /// </summary>
    [Required]
    [Column("Org_ID", TypeName = "binary(16)")]
    public Guid OrgID { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the access type.
    /// </summary>
    [Required]
    [Column("Access_ID", TypeName = "binary(16)")]
    public Guid AccessID { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("OrgID")] public virtual Organisation Organisation { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("AccessID")] public virtual AnimalAccess AnimalAccess { get; set; }
}

/// <summary>
///     Represents a subscription plan available in the application.
/// </summary>
[Table("subscription")]
public class Subscription
{
    /// <summary>
    ///   Gets or sets the unique identifier for the subscription.
    /// </summary>
    [Key]
    [Column("Subscription_ID", TypeName = "binary(16)")]
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// Gets or sets the title of the subscription.
    /// </summary>
    [Required]
    [Column("Subscription_Title", TypeName = "varchar(45)")]
    [StringLength(45)]
    public string SubscriptionTitle { get; set; }

    /// <summary>
    /// Gets or sets the size of the storage available in the subscription.
    /// </summary>
    [Required]
    [Column("Storage_Size", TypeName = "int")]
    public int StorageSize { get; set; }

    /// <summary>
    /// Gets or sets the rate charged for the subscription.
    /// </summary>
    [Required]
    [Column("Charge_Rate", TypeName = "int")]
    public int ChargeRate { get; set; }
}

/// <summary>
///     Represents a financial or operational transaction in the application.
/// </summary>
[Table("transaction")]
public class Transaction
{
    /// <summary>
    ///  Gets or sets the unique identifier for the transaction.
    /// </summary>
    [Key]
    [Column("Transaction_ID", TypeName = "binary(16)")]
    public Guid TransactionId { get; set; }

    /// <summary>
    /// Gets or sets the date the transaction was made.
    /// </summary>
    [Required]
    [Column("TransType_ID", TypeName = "binary(16)")]
    public Guid TransTypeId { get; set; }

    /// <summary>
    ///  Gets or sets the unique identifier of the user.
    /// </summary>
    [Required]
    [Column("User_ID", TypeName = "binary(16)")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the animal.
    /// </summary>
    [Required]
    [Column("Animal_ID", TypeName = "binary(16)")]
    public Guid AnimalId { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("TransTypeId")] public virtual TransactionType TransactionType { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("UserID")] public virtual User User { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("AnimalID")] public virtual Animal Animal { get; set; }
}

/// <summary>
///     Represents a type of transaction in the application.
/// </summary>
[Table("transactiontype")]
public class TransactionType
{
    /// <summary>
    /// Gets or sets the unique identifier for the transaction type.
    /// </summary>
    [Key]
    [Column("TransType_ID", TypeName = "binary(16)")]
    public Guid TransTypeId { get; set; }

    /// <summary>
    /// Gets or sets the details of the transaction type.
    /// </summary>
    [Required]
    [Column("Trans_Details", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string TransDetails { get; set; }
}

/// <summary>
///     Represents a user in the application.
/// </summary>
[Table("user")]
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [Key]
    [Column("User_ID", TypeName = "binary(16)")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the permission level of the user.
    /// </summary>
    [Required]
    [Column("Permission_Level", TypeName = "char(10)")]
    [StringLength(10)]
    public string PermissionLevel { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    [Required]
    [Column("User_Name", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [Required]
    [Column("User_Email", TypeName = "varchar(255)")]
    [StringLength(255)]
    public string UserEmail { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    [Required]
    [Column("User_Password", TypeName = "varchar(12)")]
    [StringLength(12)]
    public string UserPassword { get; set; }

    /// <summary>
    /// Gets or sets the date the user joined the application.
    /// </summary>
    [Required]
    [Column("User_Date_Join", TypeName = "date")]
    [DataType(DataType.Date)]
    public DateTime UserDateJoin { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the subscription.
    /// </summary>
    [Required]
    [Column("subscription_ID", TypeName = "binary(16)")]
    public Guid SubscriptionID { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("SubscriptionID")] public virtual Subscription Subscription { get; set; }
}

/// <summary>
///     Represents the access rights assigned to a user within an organization.
/// </summary>
[Table("useraccess")]
[PrimaryKey(nameof(OrgId), nameof(UserId))]
public class UserAccess
{
    /// <summary>
    /// Gets or sets the unique identifier of the organization.
    /// </summary>
    [Column("Org_ID", TypeName = "binary(16)", Order = 0)]
    public Guid OrgId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    [Column("User_ID", TypeName = "binary(16)", Order = 1)]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the access type.
    /// </summary>
    [Required]
    [Column("AccessType_ID", TypeName = "binary(16)")]
    public Guid AccessTypeId { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("OrgID")] public virtual Organisation Organisation { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("UserID")] public virtual User User { get; set; }

    /// <summary>
    /// Navigation property
    /// </summary>
    [ForeignKey("AccessTypeID")] public virtual AccessType AccessType { get; set; }
}