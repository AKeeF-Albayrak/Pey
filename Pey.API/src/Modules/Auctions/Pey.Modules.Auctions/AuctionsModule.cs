using Microsoft.Extensions.DependencyInjection;
using Pey.Shared;

namespace Pey.Modules.Auctions;

public static class AuctionsModule
{
    public static IServiceCollection AddAuctionsModule(this IServiceCollection services)
    {
        services.AddModule(typeof(AuctionsModule).Assembly);

        return services;
    }
}
