using Microsoft.Extensions.DependencyInjection;
using Pey.Shared;

namespace Pey.Modules.Payments;

public static class PaymentsModule
{
    public static IServiceCollection AddPaymentsModule(this IServiceCollection services)
    {
        services.AddModule(typeof(PaymentsModule).Assembly);

        return services;
    }
}
