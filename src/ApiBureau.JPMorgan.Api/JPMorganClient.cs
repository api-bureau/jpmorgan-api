namespace ApiBureau.JPMorgan.Api;

public class JPMorganClient : IJPMorganClient
{
    public PayByBankEndpoint FxRateSheet { get; }

    public JPMorganClient(ApiConnection apiConnection)
    {
        FxRateSheet = new PayByBankEndpoint(apiConnection);
    }
}