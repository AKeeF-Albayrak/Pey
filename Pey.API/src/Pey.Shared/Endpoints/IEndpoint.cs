using Microsoft.AspNetCore.Routing;

namespace Pey.Shared.Endpoints;

// Her feature'ın endpoint sınıfı bunu uygular; açılışta otomatik bulunup /api grubuna eklenir.
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
