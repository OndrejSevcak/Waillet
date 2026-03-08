namespace WailletAPI.Dto;

public class AssetDto
{
    public string Symbol { get; set; } = null!;

    public string Name { get; set; } = null!;

    public byte Decimals { get; set; }
}
