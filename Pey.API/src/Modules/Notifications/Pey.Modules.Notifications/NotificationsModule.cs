using Microsoft.Extensions.DependencyInjection;
using Pey.Shared;

namespace Pey.Modules.Notifications;

public static class NotificationsModule
{
    public static IServiceCollection AddNotificationsModule(this IServiceCollection services)
    {
        services.AddModule(typeof(NotificationsModule).Assembly);

        return services;
    }
}
