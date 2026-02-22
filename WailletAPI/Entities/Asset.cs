using System;
using System.Collections.Generic;

namespace WailletAPI.Entities;

public partial class Asset
{
    public int Id { get; set; }

    public string Symbol { get; set; } = null!;

    public string Name { get; set; } = null!;

    public byte Decimals { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
