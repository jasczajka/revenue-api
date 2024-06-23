using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models;

public class IndividualClient : Client, ISoftDeletable
{
    private IndividualClient(){}
    public IndividualClient(string pesel)
    {
        Pesel = pesel;
        IsDeleted = false;
    }

    [Required, StringLength(11)]
    public string Pesel { get; private set; }
    [Required, MaxLength(30)]
    public string FirstName { get; set; }
    [Required, MaxLength(30)]
    public string LastName { get; set; }
    [Required]
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}