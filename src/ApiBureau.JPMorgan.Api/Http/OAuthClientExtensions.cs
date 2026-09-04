using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiBureau.JPMorgan.Api.Http;

internal sealed class ClientCredentialsTokenRequest
{
    internal required string Address { get; init; }
    internal required string ClientId { get; init; }
    internal required string ClientSecret { get; init; }
    internal string? Scope { get; init; }
}

internal sealed class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonPropertyName("error")]
    public string? Error { get; init; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; init; }

    [JsonIgnore]
    public bool IsError { get; init; }

    [JsonIgnore]
    public string? HttpErrorReason { get; init; }
}

internal static class OAuthClientExtensions
{
    internal static async Task<TokenResponse> RequestClientCredentialsTokenAsync(
        this HttpClient client,
        ClientCredentialsTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(request);

        var values = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", request.ClientId),
            new("client_secret", request.ClientSecret)
        };

        if (!string.IsNullOrWhiteSpace(request.Scope))
        {
            values.Add(new("scope", request.Scope));
        }

        using var message = new HttpRequestMessage(HttpMethod.Post, request.Address)
        {
            Content = new FormUrlEncodedContent(values)
        };
        using var response = await client.SendAsync(message, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        TokenResponse? tokenResponse = null;
        if (!string.IsNullOrWhiteSpace(content))
        {
            tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);
        }

        return new TokenResponse
        {
            AccessToken = tokenResponse?.AccessToken,
            TokenType = tokenResponse?.TokenType,
            ExpiresIn = tokenResponse?.ExpiresIn ?? 0,
            Error = tokenResponse?.Error ?? tokenResponse?.ErrorDescription,
            ErrorDescription = tokenResponse?.ErrorDescription,
            IsError = !response.IsSuccessStatusCode || !string.IsNullOrWhiteSpace(tokenResponse?.Error),
            HttpErrorReason = response.ReasonPhrase
        };
    }

    internal static void SetBearerToken(this HttpClient client, string token)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}