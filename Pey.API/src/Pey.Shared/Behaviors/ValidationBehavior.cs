using FluentValidation;
using Pey.Shared.Messaging;
using Pey.Shared.Results;

namespace Pey.Shared.Behaviors;

// İsteğe ait tüm FluentValidation validator'larını çalıştırır; hata varsa handler hiç çağrılmaz.
// TResponse kısıtı sayesinde sadece Result dönen isteklere uygulanır.
internal sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result, IFailureFactory<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errors = results
            .SelectMany(r => r.Errors)
            .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Length == 0)
        {
            return await next();
        }

        return TResponse.Failure(new ValidationError(errors));
    }
}
