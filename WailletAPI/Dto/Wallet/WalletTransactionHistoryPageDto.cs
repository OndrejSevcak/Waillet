namespace WailletAPI.Dto;

public class WalletTransactionHistoryPageDto
{
    public int TotalCount { get; set; }

    public int CurrentPage { get; set; }

    public int AvailablePages { get; set; }

    public IReadOnlyList<WalletTransactionHistoryItemDto> Transactions { get; set; } = [];
}