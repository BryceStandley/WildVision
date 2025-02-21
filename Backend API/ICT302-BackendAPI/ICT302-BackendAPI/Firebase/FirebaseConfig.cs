namespace ICT302_BackendAPI.Firebase;

/**
 * <summary>Helper class for loading Firebase config strings from the appsettings file</summary>
 */
public class FirebaseConfig
{
    /// <summary>Gets or sets the type of Firebase configuration.</summary>
    /// /
    public string? Type { get; set; }

    /// <summary>Helper class for loading Firebase config strings from the appsettings file</summary>
    /// /
    public string? ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the private key identifier for Firebase configuration.
    /// </summary>
    public string? PrivateKeyId { get; set; }

    /// <summary>
    /// Gets or sets the private key used for Firebase authentication.
    /// </summary>
    public string? PrivateKey { get; set; }

    /// <summary>Gets or sets the client email used for Firebase credentials.</summary>
    public string? ClientEmail { get; set; }

    /// <summary>Gets or sets the client ID for Firebase configuration.</summary>
    public string? ClientId { get; set; }

    /// <summary>Gets or sets the authentication URI for Firebase configuration.</summary>
    public string? AuthUri { get; set; }

    /// <summary>Gets or sets the token URI utilized for authentication with Firebase.</summary>
    public string? TokenUri { get; set; }

    /// <summary>Gets or sets the URL of the X509 certificate for the authentication provider.</summary>
    public string? AuthProviderX509CertUrl { get; set; }

    /// <summary>Gets or sets the URL of the X.509 certificate of the client.</summary>
    public string? ClientX509CertUrl { get; set; }

    /// <summary>
    /// Gets or sets the universe domain for the Firebase configuration.
    /// </summary>
    public string? UniverseDomain { get; set; }
}