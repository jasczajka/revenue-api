using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Dtos.RequestDtos;

public class NewSubscriptionRequestDto
{
    [Required]
    public int SubscriptionOfferId { get; set; }
    [Required]
    public int SoftwareId { get; set; }
    [Required]
    public float SoftwareVersion { get;  set; }
    [Required]
    public int ClientId { get; set; }
    [Required] 
    public DateOnly From { get; set; }
    
}