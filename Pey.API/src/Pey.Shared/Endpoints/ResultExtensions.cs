using Microsoft.AspNetCore.Http;
using Pey.Shared.Results;

namespace Pey.Shared.Endpoints;

public static class ResultExtensions
{
    // Başarısız bir Result'u ErrorType'a göre doğru durum koduyla ProblemDetails yanıtına çevirir.
    // Kullanım: return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblem();
    public static IResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Başarılı bir sonuç ProblemDetails'e çevrilemez.");
        }

        var error = result.Error;

        var extensions = new Dictionary<string, object?> { ["code"] = error.Code };

        if (error is ValidationError validationError)
        {
            extensions["errors"] = validationError.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());
        }

        return TypedResults.Problem(
            statusCode: GetStatusCode(error.Type),
            detail: error.Description,
            extensions: extensions);
    }

    private static int GetStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}
