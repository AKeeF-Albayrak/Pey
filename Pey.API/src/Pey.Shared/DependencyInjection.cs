using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pey.Shared.Behaviors;
using Pey.Shared.Endpoints;
using Pey.Shared.Exceptions;
using Pey.Shared.Messaging;

namespace Pey.Shared;

public static class DependencyInjection
{
    // Tüm modüllerin ortak kullandığı altyapı; Program.cs'te bir kez çağrılır.
    public static IServiceCollection AddShared(this IServiceCollection services)
    {
        services.AddScoped<ISender, Sender>();

        // Kayıt sırası = çalışma sırası: Logging en dışta, Validation onun içinde.
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            };
        });
        services.AddExceptionHandler<GlobalExceptionHandler>();

        // DateTime.UtcNow yerine TimeProvider: testlerde zamanı sabitleyebilmek için.
        services.AddSingleton(TimeProvider.System);

        return services;
    }

    // Bir modülün assembly'sindeki handler, validator ve endpoint'leri kaydeder.
    // Her modülün AddXModule metodu bunu çağırır.
    public static IServiceCollection AddModule(this IServiceCollection services, Assembly assembly)
    {
        services.AddRequestHandlers(assembly);
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        services.AddEndpoints(assembly);

        return services;
    }

    private static void AddRequestHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlers = assembly.DefinedTypes
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.ImplementedInterfaces
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(i => (Service: i, Implementation: t.AsType())));

        foreach (var (service, implementation) in handlers)
        {
            services.AddScoped(service, implementation);
        }
    }

    private static void AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var endpoints = assembly.DefinedTypes
            .Where(t => t is { IsAbstract: false, IsInterface: false } && t.IsAssignableTo(typeof(IEndpoint)))
            .Select(t => ServiceDescriptor.Transient(typeof(IEndpoint), t));

        services.TryAddEnumerable(endpoints);
    }
}
