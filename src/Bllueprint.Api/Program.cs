using System.Text.Json.Serialization;
using Bllueprint.Application;
using Bllueprint.Domain;
using Bllueprint.Infrastructure;
using Scalar.AspNetCore;

namespace Bllueprint.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services);

        WebApplication app = builder.Build();

        Configure(app);

        await app.RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.MaxDepth = 128;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IAccessValidator).Assembly));
        services.ConfigureApplication();
        services.ConfigureInfrastructure();
        services.ConfigureDomain();
        services.AddOpenApi();
    }

    private static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}
