namespace revenue_api.Exceptions;

[Serializable]
public class PaymentOverdueException : Exception
{
    public PaymentOverdueException ()
    {}

    public PaymentOverdueException (string message) 
        : base(message)
    {}

    public PaymentOverdueException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}