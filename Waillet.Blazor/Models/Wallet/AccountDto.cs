namespace Waillet.Blazor.Models.Wallet;

public class AccountDto
{
    public long AccKey { get; set; }
    public long UserKey { get; set; }
    public string Asset { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
