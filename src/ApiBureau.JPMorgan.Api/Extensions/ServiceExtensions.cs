using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace ApiBureau.JPMorgan.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddJPMorgan(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JPMorganSettings>(options => configuration.GetSection(nameof(JPMorganSettings)).Bind(options));

        services.AddHttpClient<ApiConnection>()
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(20))
            .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(3) }));

        services.AddSingleton<IJPMorganClient, JPMorganClient>();

        return services;
    }
}