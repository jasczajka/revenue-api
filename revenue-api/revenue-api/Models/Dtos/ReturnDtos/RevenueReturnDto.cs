using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Dtos.ReturnDtos;

public class RevenueReturnDto
{
    [Required]
    public decimal Revenue { get; set; }
    [Required, StringLength(3)]
    public string Currency { get; set; }
}