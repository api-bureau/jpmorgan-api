using ApiBureau.JPMorgan.Api.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ApiBureau.JPMorgan.Api.Console.Services;

public class DataService
{
    private readonly IJPMorganClient _client;
    private readonly ILogger<DataService> _logger;

    public DataService(IJPMorganClient client, ILogger<DataService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        //await _client.AuthenticateAsync();

        var result = await _client.FxRateSheet.RetrievePaymentsAsync("a14a49b4-b21e-416e-ab8d-c0e57efbd2ea");

        _logger.LogInformation(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
}