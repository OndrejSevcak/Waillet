namespace WailletAPI.Dto;

public class WalletBalanceDto
{
    public long AccKey { get; set; }

    public string Asset { get; set; } = null!;

    public decimal Balance { get; set; }
}