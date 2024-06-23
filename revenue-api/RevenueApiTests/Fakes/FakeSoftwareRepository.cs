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

        public FakeSoftwareRepository()
        {
            _softwares = new List<Software>
            {
                new Software
                {
                    SoftwareId = 1,
                    Name = "Software A",
                    YearlyPrice = 1000,
                    Discounts = new List<Discount>
                    {
                        new Discount
                        {
                            From = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                            To = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                            Value = 10
                        },
                        new Discount
                        {
                            From = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                            To = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                            Value = 15
                        }
                    }
                },
                new Software
                {
                    SoftwareId = 2,
                    Name = "Software B",
                    YearlyPrice = 1000,
                    Discounts = new List<Discount>()
                },
                new Software
                {
                    SoftwareId = 3,
                    Name = "Software C",
                    YearlyPrice = 1500,
                    Discounts = new List<Discount>()
                }
            };
        }

        public Task<Software?> GetSoftwareByIdAsync(int softwareId, CancellationToken cancellationToken)
        {
            var software = _softwares.FirstOrDefault(s => s.SoftwareId == softwareId);
            return Task.FromResult(software);
        }

        public static List<Software> GetTestSoftwares()
        {
            return 
                new List<Software>
                {
                    new Software
                    {
                        SoftwareId = 1,
                        Name = "Software A",
                        YearlyPrice = 1000,
                        Discounts = new List<Discount>
                        {
                            new Discount
                            {
                                From = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                                To = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                                Value = 10
                            },
                            new Discount
                            {
                                From = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                                To = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
                                Value = 15
                            }
                        }
                    },
                    new Software
                    {
                        SoftwareId = 2,
                        Name = "Software B",
                        YearlyPrice = 1000,
                        Discounts = new List<Discount>()
                    },
                    new Software
                    {
                        SoftwareId = 3,
                        Name = "Software C",
                        YearlyPrice = 1500,
                        Discounts = new List<Discount>()
                    }
                };
        }
    }
}