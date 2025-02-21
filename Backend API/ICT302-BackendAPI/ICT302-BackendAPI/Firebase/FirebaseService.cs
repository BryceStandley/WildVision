using System.Text.Json;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;

namespace ICT302_BackendAPI.Firebase;

/**
 * <summary>Firebase Service class for loading and setting up connections to Firebase</summary>
 */
public class FirebaseService(IOptions<FirebaseConfig> firebaseConfig)
{
    private readonly FirebaseConfig _firebaseConfig = firebaseConfig.Value;

    /**
     * <summary>Gets a Google Credential object from the firebase config</summary>
     * <returns>GoogleCredential object for use when loading the Firebase Admin service</returns>
     */
    public GoogleCredential GetGoogleCredential()
    {
        var json = new
        {
            type = _firebaseConfig.Type,
            project_id = _firebaseConfig.ProjectId,
            private_key_id = _firebaseConfig.PrivateKeyId,
            private_key = _firebaseConfig.PrivateKey!.Replace("\\n", "\n"),
            client_email = _firebaseConfig.ClientEmail,
            client_id = _firebaseConfig.ClientId,
            auth_uri = _firebaseConfig.AuthUri,
            token_uri = _firebaseConfig.TokenUri,
            auth_provider_x509_cert_url = _firebaseConfig.AuthProviderX509CertUrl,
            client_x509_cert_url = _firebaseConfig.ClientX509CertUrl
        };

        var jsonCredentials = JsonSerializer.Serialize(json);
        return GoogleCredential.FromJson(jsonCredentials);
    }
}