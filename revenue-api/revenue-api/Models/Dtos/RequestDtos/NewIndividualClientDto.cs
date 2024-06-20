using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Dtos.RequestDtos;

public class NewIndividualClientDto
{
    [Required, StringLength(11)]
    public string Pesel { get;  set; }
    [Required, MaxLength(30)]
    public string FirstName { get; set; }
    [Required, MaxLength(30)]
    public string LastName { get; set; }
    [Required, EmailAddress]
    public string EmailAddress { get; set; }
    [Required, Phone]
    public string PhoneNumber { get; set; }
    [Required, MaxLength(300)]
    public string Address { get; set; }
    
    
}