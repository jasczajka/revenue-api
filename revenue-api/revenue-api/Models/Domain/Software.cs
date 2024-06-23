using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace revenue_api.Models;

public class Software
{
    [Key]
    public int SoftwareId { get; set; }
    [Required, MaxLength(50)]
    public string Name { get; set; }
    [Required, MaxLength(500)]
    public string Description { get; set; }
    [Required, MaxLength(50)]
    public string Category { get; set; }
    [Required]
    public float CurrentVersion { get; set; }
    [Required, Column(TypeName="money")]
    public decimal YearlyPrice { get; set; }
    [Timestamp]
    public byte [] RowVersion { get; set; }

    public virtual List<Contract> Contracts { get; set; } = [];
    public virtual List<Discount> Discounts { get; set; } = [];
}