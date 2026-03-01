namespace WailletAPI.Dto;

public class WalletTransactionHistoryItemDto
{
    public long Id { get; set; }

    public long AccKey { get; set; }

    public string Asset { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Type { get; set; } = null!;

    public long ReferenceId { get; set; }

    public string ReferenceType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
