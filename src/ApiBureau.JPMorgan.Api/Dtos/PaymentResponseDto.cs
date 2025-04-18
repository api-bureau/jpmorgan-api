namespace ApiBureau.JPMorgan.Api.Dtos;

public class PaymentResponseDto
{
    public required string Id { get; set; }
    public required string Status { get; set; }
    public required DateTime StatusUpdatedAt { get; set; }
    public required string StatusMessage { get; set; }
    public required DateTime CreatedAt { get; set; }
    public string? RedirectURL { get; set; }
    public string? PaymentLinkURL { get; set; }
    public string? PaymentQRCode { get; set; }
    public AccountEntityDto? Debtor { get; set; }
    public required AccountEntityDto Creditor { get; set; }
    public required PaymentAmountDto PaymentAmount { get; set; }
}