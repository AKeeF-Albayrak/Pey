using Microsoft.Extensions.DependencyInjection;
using Pey.Shared;

namespace Pey.Modules.Identity;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        services.AddModule(typeof(IdentityModule).Assembly);

        return services;
    }
}
