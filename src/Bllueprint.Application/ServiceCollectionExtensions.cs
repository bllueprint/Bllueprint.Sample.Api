using Microsoft.Extensions.DependencyInjection;

namespace Bllueprint.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        services.AddSingleton<IAccessValidator, AccessValidator>();
        return services;
    }
}
