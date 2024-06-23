namespace revenue_api.Exceptions;


[Serializable]
public class InvalidContractLengthException : Exception
{
    public InvalidContractLengthException ()
    {}

    public InvalidContractLengthException (string message) 
        : base(message)
    {}

    public InvalidContractLengthException (string message, Exception innerException)
        : base (message, innerException)
    {}  
}
