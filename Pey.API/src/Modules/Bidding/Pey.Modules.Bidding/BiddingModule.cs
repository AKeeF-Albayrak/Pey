using Microsoft.Extensions.DependencyInjection;
using Pey.Shared;

namespace Pey.Modules.Bidding;

public static class BiddingModule
{
    public static IServiceCollection AddBiddingModule(this IServiceCollection services)
    {
        services.AddModule(typeof(BiddingModule).Assembly);

        return services;
    }
}
