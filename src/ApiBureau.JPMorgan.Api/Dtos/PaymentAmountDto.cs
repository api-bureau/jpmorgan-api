using ApiBureau.JPMorgan.Api.Converters;
using System.Text.Json.Serialization;

namespace ApiBureau.JPMorgan.Api.Dtos;

public class PaymentAmountDto
{
    public required string Currency { get; set; }

    [JsonConverter(typeof(JsonStringToDecimalConverter))]
    public required decimal Value { get; set; }
}