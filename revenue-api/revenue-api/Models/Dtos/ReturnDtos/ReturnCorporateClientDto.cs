using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Dtos.ReturnDtos;

public class ReturnCorporateClientDto
{
    [Key]
    public int ClientId { get; set; }
    [Required, EmailAddress]
    public string EmailAddress { get; set; }
    [Required, Phone]
    public string PhoneNumber { get; set; }
    [Required, MaxLength(300)]
    public string Address { get; set; }
    [Required, MaxLength(14)]
    public string KRS { get;  set; }
    [Required, MaxLength(30)]
    public string CompanyName { get; set; }
}