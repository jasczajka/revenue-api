using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace revenue_api.Models;

public class Discount
{
    [Key]
    public int DiscountId { get; set; }
    [Required, MaxLength(50)]
    public string Name { get; set; }
    [Required, MaxLength(3)]
    [RegularExpression("^(SUB|PUR)$", ErrorMessage = "Value must be either 'SUB' or 'PUR'.")]
    //SUB or PUR
    public string DiscountType { get; set; }
    [Required]
    public int Value { get; set; }
    [Required]
    public DateOnly From { get; set; }
    [Required]
    public DateOnly To { get; set; }
    [Timestamp]
    public byte [] RowVersion { get; set; }

    public virtual List<Software> Softwares { get; set; } = [];
}