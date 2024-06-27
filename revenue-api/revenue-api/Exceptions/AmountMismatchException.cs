namespace revenue_api.Exceptions;

[Serializable]
public class AmountMismatchException : Exception
{
    public AmountMismatchException ()
    {}

    public AmountMismatchException (string message) 
        : base(message)
    {}

    public AmountMismatchException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}