using ExchangeRatesAssignment.Core.Entities;

namespace ExchangeRatesAssignment.Core.Interfaces
{
    public interface IPartnerRatesRepository
    {
        Task<IEnumerable<PartnerRate>> GetPartnerRatesAsync(CancellationToken cancellationToken = default);
    }
}
