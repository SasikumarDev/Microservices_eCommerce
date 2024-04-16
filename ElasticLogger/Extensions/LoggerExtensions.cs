using ElasticLogger.Middleware;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElasticLogger.Extensions;

public static class LoggerExtensions
{
    public static IServiceCollection AddElasticLogger(this IServiceCollection services, string ElasticURL)
    {
        Console.WriteLine($"Elastic Logger {ElasticURL}");
        try
        {
            // var pool = new SingleNodeConnectionPool(new Uri(ElasticURL));
            // var settings = new ConnectionSettings(pool);
            // services.AddSingleton(new SingleNodeConnectionPool(new Uri(ElasticURL)));
            services.AddSingleton(new ConnectionSettings(new Uri(ElasticURL)));
        }
        catch(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error {ex.Message}");
            Console.ResetColor();
            throw;
        }
        return services;
    }

    public static IApplicationBuilder UseElasticLogger(this IApplicationBuilder app)
    {
        app.UseMiddleware<Logger>();
        return app;
    }
}