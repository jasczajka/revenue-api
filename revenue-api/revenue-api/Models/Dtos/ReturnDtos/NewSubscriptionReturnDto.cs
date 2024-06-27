using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Dtos.ReturnDtos;

public class NewSubscriptionReturnDto
{
    [Key]
    public int SubscriptionId { get; set; }
    [Required]
    public float SoftwareVersion { get;  set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public int SoftwareId { get; set; }

    [Required]
    public int RenewalPeriod { get; set; }
    [Required]
    public DateOnly NextPaymentDate { get; set; }
}