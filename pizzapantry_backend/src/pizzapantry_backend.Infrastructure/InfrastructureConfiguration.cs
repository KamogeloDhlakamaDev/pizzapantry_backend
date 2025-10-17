
using pizzapantry_backend.Application.Features.AdjustItem.Repository;
using pizzapantry_backend.Application.Features.Inventory.Repository;
using pizzapantry_backend.Infrastructure.Repositories;

namespace pizzapantry_backend.Infrastructure;

public static class InfrastructureConfiguration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddTransient<IInventoryRepository, InventoryRepository>();
        services.AddTransient<IAdjustItemRespositry, AdjustItemRepository>();

        //Add Token
    }
}