using System.ComponentModel.DataAnnotations;
using revenue_api.Exceptions;

namespace revenue_api.Models;

public class Contract
{
    private Contract()
    {
    }

    public Contract(DateOnly from, DateOnly to, int yearsOfUpdateSupport, float softwareVersion, Client client, Software software)
    {
        var lengthInDays = to.DayNumber - from.DayNumber;
        if (lengthInDays < 3 || lengthInDays > 30)
        {
            throw new InvalidContractLengthException("contract length must be between 3 and 30 days in length");
        }
        From = from;
        To = to;
        YearsOfUpdateSupport = yearsOfUpdateSupport;
        SoftwareVersion = softwareVersion;
        Client = client;
        Software = software;
        IsSigned = false;
    }

    [Key]
    public int ContractId { get; set; }
    [Required]
    public DateOnly From { get; private set; }
    [Required]
    public DateOnly To { get; private set; }
    [Required, Range(0,3)]
    public int YearsOfUpdateSupport { get; private set; }
    
    [Required]
    public float SoftwareVersion { get; private set; }
    [Required]
    public bool IsSigned { get; set; }
    
    [Required]
    public virtual Client Client { get; private set; }
    [Required]
    public virtual Software Software { get; private set; }
    public virtual List<Payment> Payments { get; set; } = [];
    
    
    [Timestamp]
    public byte [] RowVersion { get; set; }
    
}