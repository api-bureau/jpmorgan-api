using ApiBureau.JPMorgan.Api.Dtos;

namespace ApiBureau.JPMorgan.Api.Endpoints;

public class PayByBankEndpoint
{
    protected ApiConnection ApiConnection { get; }

    private const string BaseUrl = "https://api-mock.payments.jpmorgan.com/tsapi/paybybank/v2/payments";

    public PayByBankEndpoint(ApiConnection apiConnection)
    {
        ApiConnection = apiConnection;
    }

    /// <summary>
    /// Retrieves the details of the payment request by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PaymentResponseDto?> RetrievePaymentsAsync(string id)
    {
        var response = await ApiConnection.GetAsync<PaymentResponseDto>($"{BaseUrl}/{id}");

        return response;
    }
}