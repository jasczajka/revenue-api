using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Dtos.ReturnDtos;

public class ProductRevenueReturnDto : RevenueReturnDto
{
    [Required]
    public int ProductId { get; set; }
    
    
}