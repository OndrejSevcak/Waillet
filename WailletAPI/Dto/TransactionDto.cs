namespace WailletAPI.Dto
{
    public class TransactionDto
    {
        public long TxKey { get; set; }
        public long FromAccKey { get; set; }
        public long ToAccKey { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public decimal Rate { get; set; }
        public string? CurrencyFrom { get; set; }
        public string? CurrencyTo { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public long InitiatorUserKey { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
