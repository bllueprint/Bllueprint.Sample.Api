using Bllueprint.Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Bllueprint.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDomain(this IServiceCollection services)
    {
        services.AddBllueprintDomainServices();
        return services;
    }
}
