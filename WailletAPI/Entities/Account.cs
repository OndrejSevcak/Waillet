using System;
using System.Collections.Generic;

namespace WailletAPI.Entities;

public partial class Account
{
    public long AccKey { get; set; }

    public long UserKey { get; set; }

    public string Asset { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Ledger> Ledgers { get; set; } = new List<Ledger>();

    public virtual User UserKeyNavigation { get; set; } = null!;
}
