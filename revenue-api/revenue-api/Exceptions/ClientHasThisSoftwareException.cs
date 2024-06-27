namespace revenue_api.Exceptions;

[Serializable]
public class ClientHasThisSoftwareException : Exception
{
    public ClientHasThisSoftwareException ()
    {}

    public ClientHasThisSoftwareException (string message) 
        : base(message)
    {}

    public ClientHasThisSoftwareException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}