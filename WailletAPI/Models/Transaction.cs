using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WailletAPI.Models
{
    public class Transaction
    {
        [Key]
        [Column("tx_key")]
        public long TxKey { get; set; }

        [Required]
        [Column("from_acc_key")]
        public long FromAccKey { get; set; }

        [Required]
        [Column("to_acc_key")]
        public long ToAccKey { get; set; }

        // Amount in source currency
        [Column("amount_from", TypeName = "decimal(19,8)")]
        public decimal AmountFrom { get; set; }

        // Amount in destination currency (after conversion)
        [Column("amount_to", TypeName = "decimal(19,8)")]
        public decimal AmountTo { get; set; }

        [Column("currency_from", TypeName = "varchar(10)")]
        public string? CurrencyFrom { get; set; }

        [Column("currency_to", TypeName = "varchar(10)")]
        public string? CurrencyTo { get; set; }

        [Column("rate", TypeName = "decimal(19,8)")]
        public decimal Rate { get; set; }

        [Column("type", TypeName = "varchar(50)")]
        public string? Type { get; set; }

        [Column("status", TypeName = "varchar(50)")]
        public string? Status { get; set; }

        [Column("initiator_user_key")]
        public long InitiatorUserKey { get; set; }

        [Column("created_at", TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
