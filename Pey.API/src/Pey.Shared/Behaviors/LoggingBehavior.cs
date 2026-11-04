using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Pey.Shared.Messaging;
using Pey.Shared.Results;

namespace Pey.Shared.Behaviors;

// Zincirin en dışındaki adım: her isteğin adını, süresini ve (varsa) hata kodunu loglar.
internal sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var startedAt = Stopwatch.GetTimestamp();

        logger.LogInformation("Processing {RequestName}", requestName);

        var response = await next();
        var elapsedMs = Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds;

        if (response is Result { IsFailure: true } result)
        {
            logger.LogWarning("{RequestName} failed with {ErrorCode} in {ElapsedMs:0} ms", requestName, result.Error.Code, elapsedMs);
        }
        else
        {
            logger.LogInformation("{RequestName} completed in {ElapsedMs:0} ms", requestName, elapsedMs);
        }

        return response;
    }
}
