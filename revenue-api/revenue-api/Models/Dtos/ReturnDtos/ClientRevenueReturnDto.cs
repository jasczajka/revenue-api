using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Dtos.ReturnDtos;

public class ClientRevenueReturnDto : RevenueReturnDto
{
    [Required]
    public int ClientId { get; set; }
}