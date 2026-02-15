using System;
using System.Collections.Generic;

namespace WailletAPI.Entities;

public partial class WithdrawalRequest
{
    public long WithId { get; set; }

    public long UserKey { get; set; }

    public string Asset { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal FeeAmount { get; set; }

    public string ToAddress { get; set; } = null!;

    public string? BlockchainTxId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User UserKeyNavigation { get; set; } = null!;
}
