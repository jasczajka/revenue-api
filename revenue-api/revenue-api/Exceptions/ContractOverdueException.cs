namespace revenue_api.Exceptions;

[Serializable]
public class ContractOverdueException : Exception
{
    public ContractOverdueException ()
    {}

    public ContractOverdueException (string message) 
        : base(message)
    {}

    public ContractOverdueException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}