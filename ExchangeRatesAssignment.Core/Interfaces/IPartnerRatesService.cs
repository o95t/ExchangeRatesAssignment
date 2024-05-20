using ExchangeRatesAssignment.Core.Entities;

namespace ExchangeRatesAssignment.Core.Interfaces
{
    public interface IPartnerRatesService
    {
        Task<IEnumerable<PartnerRate>> GetPartnerRatesAsync(string countryCode);
        Task<IEnumerable<PartnerRate>> GetPartnerRatesFromApiAsync(string url, string countryCode);
    }
}
