namespace revenue_api.Exceptions;

[Serializable]
public class NoSuchResourceException : Exception
{
    public NoSuchResourceException ()
    {}

    public NoSuchResourceException (string message) 
        : base(message)
    {}

    public NoSuchResourceException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}