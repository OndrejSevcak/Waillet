namespace WailletAPI.Services;

public class DummyExchangeRateService : IExchangeRateService
{
    // Very simple fixed rates for now. 1 EUR = 0.00005 BTC, reverse is 1 BTC = 20000 EUR
    public Task<decimal> GetRateAsync(string currencyFrom, string currencyTo)
    {
        if (string.Equals(currencyFrom, currencyTo, StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(1m);

        var from = currencyFrom?.Trim().ToUpperInvariant() ?? "";
        var to = currencyTo?.Trim().ToUpperInvariant() ?? "";

        if (from == "EUR" && to == "BTC")
            return Task.FromResult(0.00005m);

        if (from == "BTC" && to == "EUR")
            return Task.FromResult(20000m);

        // Default fallback: 1:1
        return Task.FromResult(1m);
    }
}
