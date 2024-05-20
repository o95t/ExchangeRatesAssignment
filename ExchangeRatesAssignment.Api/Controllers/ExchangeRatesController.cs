using Microsoft.AspNetCore.Mvc;
using ExchangeRatesAssignment.Core.Interfaces;
using ExchangeRatesAssignment.Api.DTOs;
using AutoMapper;
using ExchangeRatesAssignment.Core.Entities;

namespace ExchangeRatesAssignment.Api.Controllers
{
    [Route("api/exchange-rates")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {

        private readonly IPartnerRatesService _partnerRatesService;
        private readonly ICountryCodeService _countryCodeService;
        private readonly IMapper _mapper;
        public ExchangeRatesController(IPartnerRatesService partnerRatesService, IMapper mapper, ICountryCodeService countryCodeService)
        {
            _partnerRatesService = partnerRatesService;
            _mapper = mapper;
            _countryCodeService = countryCodeService;
        }

        [HttpGet("{country}")]
        public async Task<ActionResult<IEnumerable<PangeaExchangeRate>>> GetPartnerRatesAsync(string country)
        {
            try
            {
                if (country.Length != 3)
                {
                    return BadRequest("Country code must be three letters long.");
                }
                _countryCodeService.CountryCode = country;
                var rates = await _partnerRatesService.GetPartnerRatesAsync(country);
                if (rates == null || !rates.Any())
                {
                    return NotFound($"No rates found for country code: {country}.");
                }
                var mappedRates = _mapper.Map<IEnumerable<PangeaExchangeRate>>(rates);
                return Ok(mappedRates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }

        [HttpGet("{url}/{country}")]
        public async Task<ActionResult<IEnumerable<PangeaExchangeRate>>> GetPartnerRatesFromApiAsync(string url, string country)
        {
            try
            {

                if (country.Length != 3)
                {
                    return BadRequest("Country code must be three letters long.");
                }
                _countryCodeService.CountryCode = country;

                IEnumerable<PartnerRate> partnerRatesFromApi = await _partnerRatesService
                                                               .GetPartnerRatesFromApiAsync(url, country);
                if (partnerRatesFromApi == null || !partnerRatesFromApi.Any())
                {
                    return NotFound($"No rates found for country code: {country}.");
                }
                var mappedRates = _mapper.Map<IEnumerable<PangeaExchangeRate>>(partnerRatesFromApi);
                return Ok(mappedRates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
