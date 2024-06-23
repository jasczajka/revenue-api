using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Dtos.ReturnDtos;

public class NewContractReturnDto
{
    [Key]
    public int ContractId { get; set; }
    [Required] 
    public DateOnly From { get; set; }
    [Required]
    public DateOnly To { get; set; }
    [Required, Range(0,3)]
    public int YearsOfUpdateSupport { get;  set; }
    [Required]
    public float SoftwareVersion { get;  set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public int SoftwareId { get; set; }
}