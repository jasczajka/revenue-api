using revenue_api.Exceptions;

namespace revenue_api.Services;

public class CurrencyExchangeService : ICurrencyExchangeService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiBaseUrl;
    
    public CurrencyExchangeService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["CurrencyApi:ApiKey"];
        _apiBaseUrl = configuration["CurrencyApi:BaseUrl"];
    }

    public async Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(
                _apiBaseUrl + "/" + _apiKey + "/latest/" + fromCurrency
            );
            if (response == null  )
            {
                throw new CurrencyExchangeServiceException(
                    $"something went wrong getting the exchange rate from {fromCurrency} to {toCurrency}");
            }

            if (!response.Conversion_Rates.TryGetValue(toCurrency, out var exchangeRate))
            {
                throw new CurrencyExchangeServiceException(
                    $"no exchange rate {fromCurrency} to {toCurrency} found");
            }
            return response.Conversion_Rates[toCurrency];
        }
        catch (HttpRequestException exc)
        {
            throw new CurrencyExchangeServiceException(
                $"something went wrong getting the exchange rate from {fromCurrency} to {toCurrency}"
            );
        }
    }
    public class ExchangeRateResponse
    {
        public string Result { get; set; }
        public Dictionary<string, decimal> Conversion_Rates { get; set; }
    }
}