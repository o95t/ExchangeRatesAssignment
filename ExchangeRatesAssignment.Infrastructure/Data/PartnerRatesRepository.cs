using ExchangeRatesAssignment.Core.Entities;
using ExchangeRatesAssignment.Core.Interfaces;
using System.Text.Json;

namespace ExchangeRatesAssignment.Infrastructure.Data
{
    public class PartnerRatesRepository : IPartnerRatesRepository
    {
        // Reads and returns the input partner rates from the provided JSON file PartnerRates.json
        public async Task<IEnumerable<PartnerRate>> GetPartnerRatesAsync(CancellationToken cancellationToken = default)
        {

            string text = await File.ReadAllTextAsync(@"Input/PartnerRates.json", cancellationToken);
            var partnerRates = JsonSerializer.Deserialize<IList<PartnerRate>>(text);

            return partnerRates ?? new List<PartnerRate>();
        }
    }
}
