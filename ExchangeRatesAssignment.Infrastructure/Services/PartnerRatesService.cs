using ExchangeRatesAssignment.Core.Entities;
using ExchangeRatesAssignment.Core.Interfaces;
using System.Text.Json;

namespace ExchangeRatesAssignment.Infrastructure.Services
{
    public class PartnerRatesService : IPartnerRatesService
    {

        private readonly IPartnerRatesRepository _partnerRatesRepository;
        Dictionary<string, Mappings> countryRatesDictionary;

        public PartnerRatesService(IPartnerRatesRepository partnerRatesRepository)
        {
            _partnerRatesRepository = partnerRatesRepository;
            countryRatesDictionary = new Dictionary<string, Mappings>();
            countryRatesDictionary.Add("MEX", new Mappings() { CurrencyCode = "MXN", Rates = 0.024m });
            countryRatesDictionary.Add("IND", new Mappings() { CurrencyCode = "INR", Rates = 3.213m });
            countryRatesDictionary.Add("PHL", new Mappings() { CurrencyCode = "PHP", Rates = 2.437m });
            countryRatesDictionary.Add("GTM", new Mappings() { CurrencyCode = "GTQ", Rates = 0.056m });
        }

        async Task<IEnumerable<PartnerRate>> IPartnerRatesService.GetPartnerRatesAsync(string countryCode)
        {

            List<PartnerRate> partnerRateList = (List<PartnerRate>)await
                            _partnerRatesRepository.GetPartnerRatesAsync();
            return await GetRecentPartnerRatesRounded(partnerRateList, countryCode);

        }

        async Task<IEnumerable<PartnerRate>> IPartnerRatesService.GetPartnerRatesFromApiAsync(string url, string countryCode)
        {
            try
            {
                List<PartnerRate> partnerRates = new List<PartnerRate>();
                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync("endpoint");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                partnerRates = JsonSerializer.Deserialize<List<PartnerRate>>(responseBody);

                return await GetRecentPartnerRatesRounded(partnerRates, countryCode);

            }
            catch (HttpRequestException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }

        }

        async Task<IEnumerable<PartnerRate>> GetRecentPartnerRatesRounded(List<PartnerRate> partnerRates, string countryCode)
        {
            if (partnerRates != null && partnerRates.Count > 0)
            {

                Mappings mappings = countryRatesDictionary[countryCode];

                partnerRates = partnerRates
                        .Where(x => x.Currency == mappings.CurrencyCode)
                        .ToList();

                //Get Latest Date
                var mostRecentDate = partnerRates.Max(pr => pr.AcquiredDate);
                List<PartnerRate> mostRecentRates = partnerRates
                        .Where(pr => pr.AcquiredDate == mostRecentDate)
                        .ToList();

                //multiply and round
                mostRecentRates.Select(pr =>
                {
                    pr.Rate = Math.Round(pr.Rate * mappings.Rates, 2);
                    return pr;
                }).ToList();
                return mostRecentRates;

            }

            return partnerRates;
        }


        class Mappings
        {
            public string? CurrencyCode { get; set; }
            public decimal Rates { get; set; }
        }

    }
}
