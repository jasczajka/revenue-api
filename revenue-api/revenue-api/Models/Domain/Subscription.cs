using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace revenue_api.Models;

public class Subscription
{
   [Key] 
   public int SubscriptionId { get; set; }
   [Required]
   public DateOnly ActiveUntil { get; set; }
   [Required]
   public bool IsCurrentPeriodPaid { get; set; }
   [Required]
   public virtual SubscriptionOffer SubscriptionOffer { get; set; }
   
   [Required]
   public virtual Client Client { get; set; }
   public virtual List<Payment> Payments { get; set; } = new List<Payment>();
}