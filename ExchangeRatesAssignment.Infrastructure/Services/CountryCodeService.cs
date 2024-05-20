using ExchangeRatesAssignment.Core.Interfaces;
namespace ExchangeRatesAssignment.Infrastructure.Services
{
    public class CountryCodeService : ICountryCodeService
    {
        public string CountryCode { get; set; } = "";
    }
}
