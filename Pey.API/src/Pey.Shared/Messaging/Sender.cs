using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Pey.Shared.Messaging;

internal sealed class Sender(IServiceProvider serviceProvider) : ISender
{
    // Send<TResponse> sadece TResponse'u bilir, isteğin somut tipini (CreateAuctionCommand) bilmez.
    // Somut tipe göre kapalı generic bir wrapper'ı reflection ile bir kez oluşturup saklıyoruz;
    // sonraki çağrılar reflection'sız çalışır.
    private static readonly ConcurrentDictionary<Type, object> Wrappers = new();

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var wrapper = (RequestHandlerWrapper<TResponse>)Wrappers.GetOrAdd(
            request.GetType(),
            requestType => Activator.CreateInstance(
                typeof(RequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse)))!);

        return wrapper.Handle(request, serviceProvider, cancellationToken);
    }
}

internal abstract class RequestHandlerWrapper<TResponse>
{
    public abstract Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
}

internal sealed class RequestHandlerWrapper<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
    where TRequest : IRequest<TResponse>
{
    public override Task<TResponse> Handle(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var typedRequest = (TRequest)request;
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        // Zinciri içten dışa kuruyoruz: en içte handler, onun etrafında son kayıtlı behavior...
        // Sonuçta ilk kayıtlı behavior en dışta kalır ve ilk o çalışır.
        RequestHandlerDelegate<TResponse> next = () => handler.Handle(typedRequest, cancellationToken);

        foreach (var behavior in serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>().Reverse())
        {
            var inner = next;
            next = () => behavior.Handle(typedRequest, inner, cancellationToken);
        }

        return next();
    }
}
