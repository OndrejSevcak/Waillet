namespace WailletAPI.Dto;

public class AccountDto
{
    public long AccKey { get; set; }

    public long UserKey { get; set; }

    public string Asset { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}