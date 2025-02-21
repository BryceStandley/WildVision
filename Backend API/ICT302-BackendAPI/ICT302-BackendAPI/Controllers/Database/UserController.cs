using FirebaseAdmin.Auth;
using ICT302_BackendAPI.Database.Models;
using ICT302_BackendAPI.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ICT302_BackendAPI.Controllers.Database;

/// <summary>
///     Represents a request to add a user.
/// </summary>
public class AddUserRequest
{
    /// <summary>
    /// Represents the name of the user.
    /// </summary>
    public string? userName { get; set; }

    /// <summary>
    /// Represents the email of the user.
    /// </summary>
    public string? userEmail { get; set; }

    /// <summary>
    /// Represents the permission level of the user.
    /// </summary>
    public string? permissionLevel { get; set; }
}

/// <summary>
///     Controller responsible for handling user-related operations in the database.
/// </summary>
[Route("api/db")]
[ApiController]
public class UserController(IUserRepository userRepo, ILogger<UserController> logger) : ControllerBase
{
    /// <summary>
    ///     Asynchronously adds a new user to the database after verifying the provided authorization token.
    /// </summary>
    /// <param name="authorization">The authorization token provided in the request header.</param>
    /// <param name="userRequested">The request body containing user details to be added.</param>
    /// <returns>An ActionResult indicating the outcome of the add operation.</returns>
    [HttpPost("user")]
    public async Task<ActionResult> AddUserAsync([FromHeader] string authorization,
        [FromBody] AddUserRequest userRequested)
    {
        var user = new User();
        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
            return Unauthorized(new
            {
                statusCode = 401,
                message = "No token provided"
            });

        try
        {
            // Extract the Firebase token from the Authorization header
            var token = authorization.Substring("Bearer ".Length).Trim();

            // Verify the Firebase token using Firebase Admin SDK
            var firebaseUid = "";
            try
            {
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                firebaseUid = decodedToken.Uid;
            }
            catch (FirebaseAuthException ex)
            {
                logger.LogError(ex.Message);
                return Unauthorized(new
                {
                    statusCode = 401,
                    message = "Invalid Firebase token"
                });
            }


            // Generate the UserID from the Firebase UID (ensuring the user cannot set their own UserID)
            user.UserId = GuidHelper.ConvertFirebaseUidToGuid(firebaseUid);

            // Check if the user already exists in the database
            try
            {
                var existingUser = await userRepo.GetUserByIdAsync(user.UserId);
                if (existingUser == null)
                {
                    // User doesn't exist, so we create a new one
                    user.UserDateJoin = DateTime.UtcNow; // Automatically set the join date
                    user.PermissionLevel =
                        userRequested.permissionLevel ?? "user"; // Default permission level if not provided


                    user.Subscription = (await userRepo.GetDefaultSubscriptionAsync())!;
                    user.SubscriptionID = user.Subscription!.SubscriptionId;

                    user.UserPassword = "";
                    user.UserEmail = userRequested.userEmail!;
                    user.UserName = userRequested.userName!;

                    var createdUser = await userRepo.CreateUserAsync(user);

                    return Ok(new
                    {
                        statusCode = 200,
                        message = "User created successfully",
                        userId = createdUser!.UserId
                    });
                }

                // If the user exists, update only allowed fields
                existingUser.UserName = string.IsNullOrEmpty(user.UserName) ? existingUser.UserName : user.UserName;
                existingUser.UserEmail = string.IsNullOrEmpty(user.UserEmail) ? existingUser.UserEmail : user.UserEmail;
                existingUser.PermissionLevel = string.IsNullOrEmpty(user.PermissionLevel)
                    ? existingUser.PermissionLevel
                    : user.PermissionLevel;

                // Update the user in the database
                await userRepo.UpdateUserAsync(existingUser);

                return Ok(new
                {
                    statusCode = 200,
                    message = "User updated successfully",
                    userId = existingUser.UserId.ToString()
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }


    /// <summary>
    ///     Asynchronously retrieves a list of all users from the database.
    /// </summary>
    /// <returns>
    ///     An ActionResult containing a list of users if successful, or an error status code and message if an exception
    ///     occurs.
    /// </returns>
    [HttpGet("users")]
    public async Task<ActionResult> GetUsersAsync()
    {
        try
        {
            var users = await userRepo.GetUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a user from the database using the provided user ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be retrieved.</param>
    /// <returns>
    ///     An IActionResult containing the user details if found, or an error status code and message if the user is not
    ///     found or an exception occurs.
    /// </returns>
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var user = await userRepo.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });
            return Ok(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Asynchronously deletes a user from both the local database and Firebase service using the provided user ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be deleted.</param>
    /// <returns>An ActionResult indicating the outcome of the delete operation.</returns>
    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            // Retrieve the user's email from the local database
            var email = await userRepo.GetEmailByIdAsync(id);
            if (string.IsNullOrEmpty(email))
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Email not found for this user"
                });

            // Use the Firebase Admin SDK to get the user by email
            var firebaseAuth = FirebaseAuth.DefaultInstance;
            UserRecord userRecord;
            try
            {
                userRecord = await firebaseAuth.GetUserByEmailAsync(email);
            }
            catch (FirebaseAuthException firebaseEx)
            {
                logger.LogError(firebaseEx, "Error occurred while retrieving user from Firebase.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    statusCode = 500,
                    message = "Firebase user retrieval failed"
                });
            }

            // Delete the user from Firebase using the Firebase UID
            try
            {
                await firebaseAuth.DeleteUserAsync(userRecord.Uid);
            }
            catch (FirebaseAuthException firebaseEx)
            {
                logger.LogError(firebaseEx, "Error occurred while deleting user from Firebase.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    statusCode = 500,
                    message = $"Firebase deletion failed: {firebaseEx.Message}"
                });
            }

            // Perform any local database deletion
            var existingUser = await userRepo.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found in the local database"
                });

            await userRepo.DeleteUserAsync(existingUser);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "Error occurred while processing the delete request.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Asynchronously retrieves the email address for a specified user ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user whose email is being requested.</param>
    /// <returns>An IActionResult containing the email address or an appropriate error message.</returns>
    [HttpGet("user/{id}/email")]
    public async Task<IActionResult> GetEmailById(Guid id)
    {
        try
        {
            // Call the repository method to get the email by the userID
            var email = await userRepo.GetEmailByIdAsync(id);

            if (string.IsNullOrEmpty(email))
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Email not found for this user"
                });

            return Ok(new
            {
                statusCode = 200, email
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }

    /// <summary>
    ///     Updates an existing user in the database with the provided user information.
    /// </summary>
    /// <param name="userToUpdate">The user entity with updated information to be saved in the database.</param>
    /// <returns>A status result indicating the outcome of the update operation.</returns>
    [HttpPut("user")]
    public async Task<IActionResult> UpdateUser([FromBody] User userToUpdate)
    {
        try
        {
            var existingUser = await userRepo.GetUserByIdAsync(userToUpdate.UserId);
            if (existingUser == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = "Record not found"
                });

            existingUser.UserName = userToUpdate.UserName;
            existingUser.UserEmail = userToUpdate.UserEmail;
            existingUser.UserPassword = userToUpdate.UserPassword;
            existingUser.PermissionLevel = userToUpdate.PermissionLevel;
            existingUser.UserDateJoin = userToUpdate.UserDateJoin;
            existingUser.SubscriptionID = userToUpdate.SubscriptionID;

            await userRepo.UpdateUserAsync(existingUser);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                statusCode = 500,
                message = ex.Message
            });
        }
    }
}