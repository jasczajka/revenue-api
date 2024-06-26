namespace revenue_api.Exceptions;

[Serializable]
public class CurrencyExchangeServiceException : Exception
{
    public CurrencyExchangeServiceException ()
    {}

    public CurrencyExchangeServiceException (string message) 
        : base(message)
    {}

    public CurrencyExchangeServiceException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}