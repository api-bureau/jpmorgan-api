namespace ApiBureau.JPMorgan.Api.Interfaces;

public interface IJPMorganClient
{
    PayByBankEndpoint FxRateSheet { get; }
}