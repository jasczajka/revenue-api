using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace revenue_api.Models;

public class SubscriptionOffer
{
    [Key]
    public int SubscriptionOfferId { get; set; }
    [Required, MaxLength(50)]
    public string Name { get; set; }
    [Required, Column(TypeName="money")]
    public decimal Price { get; set; }
    [Required, Range(30, 730)]
    //in days, under assumption that one month is 30 days and one year is 365 days
    public int RenewalPeriod { get; set; }
    [Required]
    public float SoftwareVersion { get; set; }
    [Required]
    public virtual Software Software { get; set; }
    

    
}