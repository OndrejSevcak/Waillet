using System;
using System.Collections.Generic;

namespace WailletAPI.Entities;

public partial class Ledger
{
    public long Id { get; set; }

    public long AccKey { get; set; }

    public string Asset { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Type { get; set; } = null!;

    public long ReferenceId { get; set; }

    public string ReferenceType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Account AccKeyNavigation { get; set; } = null!;
}
