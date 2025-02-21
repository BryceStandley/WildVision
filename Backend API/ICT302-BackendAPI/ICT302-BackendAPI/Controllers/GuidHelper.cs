using System.Security.Cryptography;
using System.Text;

namespace ICT302_BackendAPI.Controllers;

/**
* <summary>Helper class to convert Firebase UID's to C# GUID objects</summary>
*/
public static class GuidHelper
{
    /**
     * <summary>Hash the Firebase UID and convert it to a Guid</summary>
     * <param name="firebaseUid">The UID of the user from Firebase</param>
     * <returns>A GUID of the user from the Firebase UID</returns>
     */
    public static Guid ConvertFirebaseUidToGuid(string firebaseUid)
    {
        using (var sha1 = SHA1.Create())
        {
            var uidBytes = Encoding.UTF8.GetBytes(firebaseUid);
            var hashBytes = sha1.ComputeHash(uidBytes);

            var guidBytes = new byte[16];
            Array.Copy(hashBytes, guidBytes, 16);

            return new Guid(guidBytes);
        }
    }
}