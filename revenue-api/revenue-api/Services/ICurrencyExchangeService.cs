namespace revenue_api.Services;

public interface ICurrencyExchangeService
{
    Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency);
}