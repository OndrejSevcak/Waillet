using System;
using System.Collections.Generic;

namespace WailletAPI.Entities;

public partial class SwapTransaction
{
    public long SwapId { get; set; }

    public long UserKey { get; set; }

    public string FromAsset { get; set; } = null!;

    public string ToAsset { get; set; } = null!;

    public decimal FromAmount { get; set; }

    public decimal ToAmount { get; set; }

    public decimal Rate { get; set; }

    public decimal FeeAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User UserKeyNavigation { get; set; } = null!;
}
