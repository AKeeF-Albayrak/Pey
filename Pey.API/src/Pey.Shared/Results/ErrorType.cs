namespace Pey.Shared.Results;

// Hatanın türü; HTTP durum koduna çevrilirken kullanılır (bkz. ResultExtensions.ToProblem).
public enum ErrorType
{
    Failure,
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden
}
