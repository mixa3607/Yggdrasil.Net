using ArkProjects.Minecraft.YggdrasilApi.Models;

namespace ArkProjects.Minecraft.YggdrasilApi.Misc;

public static class ErrorResponseFactory
{
    public const string ErrorForbiddenOperationException = "ForbiddenOperationException";
    public const string ErrorIllegalArgumentException = "IllegalArgumentException";
    public const string ErrorTooManyRequestsException = "TooManyRequestsException";
    public const string ErrorUnsupportedMediaType = "Unsupported Media Type";
    public const string ErrorMethodNotAllowed = "Method Not Allowed";
    public const string ErrorNotFound = "Not Found";
    public const string ErrorGoneException = "GoneException";
    public const string ErrorInternalServerError = "InternalServerError";

    /// <summary>
    /// An attempt to send a POST request with incorrect request headers to any endpoint.
    /// </summary>
    public static ErrorResponse UnsupportedMediaType()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status415UnsupportedMediaType,
            Error = ErrorUnsupportedMediaType,
            ErrorMessage =
                "The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method",
        };
    }

    /// <summary>
    /// An attempt to use a request method other than POST to access any of the endpoints.
    /// </summary>
    public static ErrorResponse MethodNotAllowed()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status405MethodNotAllowed,
            Error = ErrorMethodNotAllowed,
            ErrorMessage =
                "The method specified in the request is not allowed for the resource identified by the request URI",
        };
    }

    /// <summary>
    /// An attempt to send a request to a non-existent endpoint.
    /// </summary>
    public static ErrorResponse NotFound()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status404NotFound,
            Error = ErrorNotFound,
            ErrorMessage = "The server has not found anything matching the request URI",
        };
    }

    /// <summary>
    /// A successful attempt to sign in using a migrated Mojang account.
    /// </summary>
    public static ErrorResponse Migrated()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status410Gone,
            Error = ErrorGoneException,
            ErrorMessage = "Migrated",
        };
    }

    /// <summary>
    /// Either a successful attempt to sign in using an account with excessive login attempts or an unsuccessful attempt to sign in using a non-existent account.
    /// </summary>
    public static ErrorResponse InvalidCredentials()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status403Forbidden,
            Error = ErrorForbiddenOperationException,
            ErrorMessage = "Invalid credentials. Invalid username or password.",
        };
    }

    /// <summary>
    /// An unsuccessful attempt to sign in using a Legacy account without a valid Minecraft purchase.
    /// </summary>
    public static ErrorResponse InvalidCredentialsLegacy()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status403Forbidden,
            Error = ErrorForbiddenOperationException,
            ErrorMessage = "Invalid credentials. Legacy account is non-premium account.",
        };
    }

    /// <summary>
    /// An unsuccessful attempt to sign in using a migrated Legacy account.
    /// </summary>
    public static ErrorResponse InvalidCredentialsMigrated()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status403Forbidden,
            Error = ErrorForbiddenOperationException,
            ErrorMessage = "Invalid credentials. Account migrated, use email as username.",
            Cause = "UserMigratedException"
        };
    }

    /// <summary>
    /// An attempt to refresh an access token that has been invalidated, no longer exists, or has been erased.
    /// </summary>
    public static ErrorResponse TokenNotExist()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status403Forbidden,
            Error = ErrorTooManyRequestsException,
            ErrorMessage = "Token does not exist",
        };
    }

    /// <summary>
    /// An attempt to validate an access token obtained from the /authenticate endpoint that has expired or become invalid.
    /// </summary>
    public static ErrorResponse InvalidToken()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status403Forbidden,
            Error = ErrorForbiddenOperationException,
            ErrorMessage = "Invalid token",
        };
    }

    /// <summary>
    /// An attempt to validate an access token obtained from the /authenticate endpoint that has expired or become invalid while under rate-limiting conditions.
    /// </summary>
    public static ErrorResponse InvalidTokenTooManyRequests()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status429TooManyRequests,
            Error = ErrorForbiddenOperationException,
            ErrorMessage = "Invalid token",
        };
    }

    /// <summary>
    /// Attempting to assign a profile to a token already bound to a profile.
    /// </summary>
    public static ErrorResponse ProfileAlreadyAssignedToToken()
    {
        return new ErrorResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Error = ErrorIllegalArgumentException,
            ErrorMessage = "Access token already has a profile assigned.",
        };
    }

    public static ErrorResponse Custom(int status, string error, string message, string? cause = null)
    {
        return new ErrorResponse()
        {
            StatusCode = status,
            Error = error,
            ErrorMessage = message,
            Cause = cause
        };
    }
}