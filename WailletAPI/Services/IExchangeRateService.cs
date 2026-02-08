namespace WailletAPI.Services;

public interface IExchangeRateService
{
    /// <summary>
    /// Get conversion rate from currencyFrom to currencyTo (amountTo = amountFrom * rate)
    /// </summary>
    Task<decimal> GetRateAsync(string currencyFrom, string currencyTo);
}
