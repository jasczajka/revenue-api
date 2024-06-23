using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models;

public class Client
{
    [Key]
    public int ClientId { get; set; }
    [Required, EmailAddress]
    public string EmailAddress { get; set; }
    [Required, Phone]
    public string PhoneNumber { get; set; }
    [Required, MaxLength(300)]
    public string Address { get; set; }
    
    
    public virtual List<Contract> Contracts { get; set; } = [];
    public virtual List<Payment> Payments { get; set; } = [];
    
    
    [Timestamp]
    public byte [] RowVersion { get; set; }

}