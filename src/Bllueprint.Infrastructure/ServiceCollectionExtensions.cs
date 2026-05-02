using Bllueprint.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Bllueprint.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ITaskRepository, TaskRepository>();
        return services;
    }
}
