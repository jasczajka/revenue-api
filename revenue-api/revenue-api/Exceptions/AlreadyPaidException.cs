namespace revenue_api.Exceptions;

[Serializable]
public class AlreadyPaidException : Exception
{
    public AlreadyPaidException ()
    {}

    public AlreadyPaidException (string message) 
        : base(message)
    {}

    public AlreadyPaidException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}