using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models;

public class CorporateClient : Client
{
    private CorporateClient(){}
    public CorporateClient(string krs)
    {
        KRS = krs;
    }

    [Required, MaxLength(14)]
    public string KRS { get; private set; }
    [Required, MaxLength(30)]
    public string CompanyName { get; set; }
    
    
}