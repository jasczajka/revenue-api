using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using revenue_api.Models;
using revenue_api.Repositories;

namespace RevenueApiTests.Fakes
{
    public class FakeSoftwareRepository : ISoftwareRepository
    {
        private readonly List<Software> _softwares;

        public static List<Software> GetTestSoftwares()
        {
            return new FakeSoftwareRepository()._softwares;
        }
        public FakeSoftwareRepository()
        {
            _softwares = new List<Software>
            {
                new Software
                {
                    SoftwareId = 1,
                    Name = "Software A",
                    YearlyPrice = 1000,
                    CurrentVersion = 1.0f,
                    Discounts = new List<Discount>
                    {
                        new Discount
                        {
                            From = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                            To = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                            Value = 10,
                            DiscountType = "PUR"
                        },
                        new Discount
                        {
                            From = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                            To = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                            Value = 15,
                            DiscountType = "PUR"
                        }
                    }
                },
                new Software
                {
                    SoftwareId = 2,
                    Name = "Software B",
                    YearlyPrice = 1000,
                    CurrentVersion = 1.0f,
                    Discounts = new List<Discount>()
                },
                new Software
                {
                    SoftwareId = 3,
                    Name = "Software C",
                    YearlyPrice = 1500,
                    CurrentVersion = 1.0f,
                    Discounts = new List<Discount>()
                },
                new Software
                {
                    SoftwareId = 4,
                    Name = "Software C",
                    YearlyPrice = 1500,
                    CurrentVersion = 1.0f,
                    Discounts = new List<Discount>
                    {
                        new Discount
                        {
                            From = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                            To = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                            Value = 15,
                            DiscountType = "SUB"
                        }
                    }
                },
                new Software
                {
                    SoftwareId = 5,
                    Name = "Software D",
                    YearlyPrice = 1500,
                    CurrentVersion = 1.0f,
                    
                },
                new Software
                {
                    SoftwareId = 7,
                    Name = "Software E",
                    YearlyPrice = 1500,
                    CurrentVersion = 1.0f,
                    
                }
                
            };
        }

        public Task<Software?> GetSoftwareByIdAsync(int softwareId, CancellationToken cancellationToken)
        {
            var software = _softwares.FirstOrDefault(s => s.SoftwareId == softwareId);
            return Task.FromResult(software);
        }
        
    }
}