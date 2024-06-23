using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace revenue_api.Models;

public class Payment
{
    [Key]
    public int PaymentId { get; set; }
    [Required, Column(TypeName="money")]
    public decimal AmountPaid { get; set; }
    [Required]
    public virtual Contract Contract { get; set; }
    [Required]
    public virtual Client Client { get; set; }
}