using AutoMapper;
using ExchangeRatesAssignment.Api.DTOs;
using ExchangeRatesAssignment.Core.Entities;
using ExchangeRatesAssignment.Core.Interfaces;

namespace ExchangeRatesAssignment.Api.profiles
{
    public class CountryCodeResolver : IValueResolver<PartnerRate, PangeaExchangeRate, string>
    {
        private readonly ICountryCodeService _countryCodeService;

        public CountryCodeResolver(ICountryCodeService countryCodeService)
        {
            _countryCodeService = countryCodeService;
        }

        public string Resolve(PartnerRate source, PangeaExchangeRate destination, string destMember, ResolutionContext context)
        {
            return _countryCodeService.CountryCode;
        }
    }
}
