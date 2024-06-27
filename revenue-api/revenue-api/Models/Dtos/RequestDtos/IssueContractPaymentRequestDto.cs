using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace revenue_api.Models.Dtos.RequestDtos;

public class IssueContractPaymentRequestDto
{
    [Required]
    public int ContractId { get; set; }
    [Required, Column(TypeName="money")]
    public decimal Amount { get; set; }
    [Required]
    public DateOnly DateOfPayment { get; set; }
}