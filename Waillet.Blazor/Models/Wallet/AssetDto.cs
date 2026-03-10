namespace Waillet.Blazor.Models.Wallet;

public class AssetDto
{
    public string Symbol { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public byte Decimals { get; set; }
}
