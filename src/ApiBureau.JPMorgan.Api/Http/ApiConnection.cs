using IdentityModel.Client;
using System.Net.Http.Json;
using System.Text.Json;

namespace ApiBureau.JPMorgan.Api.Http;

public class ApiConnection
{
    private readonly HttpClient _client;
    private readonly ILogger<ApiConnection> _logger;
    private readonly JPMorganSettings _settings;
    private string? _accessToken;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public ApiConnection(HttpClient httpClient, IOptions<JPMorganSettings> settings, ILogger<ApiConnection> logger)
    {
        _settings = settings.Value;
        _client = httpClient;
        _logger = logger;

        //JPMorganValidator.ValidateSettings(_settings, _logger);
    }

    private async Task CheckConnectionAsync()
    {
        //ToDo Check Expiry Time

        if (_accessToken == null)
        {
            await AuthenticateAsync();
        }
    }

    public async Task AuthenticateAsync()
    {
        var request = new ClientCredentialsTokenRequest
        {
            Address = _settings.AccessTokenUrl,
            ClientId = _settings.ClientId,
            ClientSecret = _settings.ClientSecret,
            Scope = "jpm:payments:sandbox"
        };

        var token = await _client.RequestClientCredentialsTokenAsync(request);

        if (token.IsError)
        {
            throw new Exception($"{token.HttpErrorReason}, {token.Raw}");
        }

        if (token is null) return;

        _accessToken = token.AccessToken;

        // Validate access token

        _client.SetBearerToken(_accessToken!);
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        await CheckConnectionAsync();

        try
        {
            return await _client.GetFromJsonAsync<T>($"{url}");
        }
        catch (JsonException e) when (e.Data.Count == 0)
        {
            return default;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
