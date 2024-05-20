using AutoMapper;
using ExchangeRatesAssignment.Api.DTOs;
using ExchangeRatesAssignment.Core.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExchangeRatesAssignment.Api.profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PartnerRate, PangeaExchangeRate>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.PangeaRate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom<CountryCodeResolver>());
        }
    }
}
