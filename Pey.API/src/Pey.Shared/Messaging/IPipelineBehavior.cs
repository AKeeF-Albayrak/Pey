namespace Pey.Shared.Messaging;

// Zincirdeki bir sonraki adımı (başka bir behavior ya da en sonda handler'ın kendisi) çağırır.
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

// Handler'ın etrafına sarılan ara adım: loglama, doğrulama, transaction vb.
public interface IPipelineBehavior<in TRequest, TResponse>
    where TRequest : notnull
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}
