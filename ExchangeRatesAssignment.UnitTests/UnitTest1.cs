using AutoMapper;
using ExchangeRatesAssignment.Api.Controllers;
using ExchangeRatesAssignment.Api.DTOs;
using ExchangeRatesAssignment.Core.Entities;
using ExchangeRatesAssignment.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ExchangeRatesAssignment.UnitTests
{
    public class UnitTest1
    {
        private readonly Mock<IPartnerRatesService> _mockPartnerRatesService;
        private readonly Mock<ICountryCodeService> _mockCountryCodeService;
        private readonly Mock<IMapper> _mapper;
        private readonly ExchangeRatesController _controller;

        public UnitTest1()
        {
            _mockPartnerRatesService = new Mock<IPartnerRatesService>();
            _mockCountryCodeService = new Mock<ICountryCodeService>();
            _mapper = new Mock<IMapper>();
            _controller = new ExchangeRatesController(_mockPartnerRatesService.Object, _mapper.Object, _mockCountryCodeService.Object);
        }

        [Fact]
        public async Task GetPartnerRatesAsync_ValidCountryCode_ReturnsRates()
        {
            // Arrange
            var country = "MXN";

            var partnerRates = new List<PartnerRate>
            {
                new PartnerRate { Currency = "MXN", Rate = 16.78m, PaymentMethod = "debit", DeliveryMethod = "cash" },
                new PartnerRate { Currency = "MXN", Rate = 16.83m, PaymentMethod = "debit", DeliveryMethod = "deposit" },
                new PartnerRate { Currency = "MXN", Rate = 16.78m, PaymentMethod = "debit", DeliveryMethod = "debit" },
                new PartnerRate { Currency = "MXN", Rate = 16.65m, PaymentMethod = "debit", DeliveryMethod = "cash" },
                new PartnerRate { Currency = "MXN", Rate = 16.81m, PaymentMethod = "bankaccount", DeliveryMethod = "debit" },
                new PartnerRate { Currency = "MXN", Rate = 16.89m, PaymentMethod = "bankaccount", DeliveryMethod = "deposit" },
                new PartnerRate { Currency = "MXN", Rate = 16.79m, PaymentMethod = "cash", DeliveryMethod = "deposit" },
                new PartnerRate { Currency = "MXN", Rate = 16.89m, PaymentMethod = "bankaccount", DeliveryMethod = "deposit" }
            };

            var pangeaExchangeRates = partnerRates.Select(pr => new PangeaExchangeRate
            {
                CountryCode = "MEX",
                PangeaRate = 0.4m,
                PaymentMethod = "debit",
                CurrencyCode = "MXN",
                DeliveryMethod = "cash"
            }).ToList();

            _mockPartnerRatesService.Setup(service => service.GetPartnerRatesAsync(country))
                .ReturnsAsync(partnerRates);

            _mapper.Setup(mapper => mapper.Map<IEnumerable<PangeaExchangeRate>>(partnerRates))
                .Returns(pangeaExchangeRates);

            // Act
            var response = await _controller.GetPartnerRatesAsync(country);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var returnedRates = Assert.IsAssignableFrom<IEnumerable<PangeaExchangeRate>>(okResult.Value);
            Assert.Equal(pangeaExchangeRates.Count, returnedRates.Count());

            foreach (var rate in returnedRates)
            {
                var expectedRate = pangeaExchangeRates.First(r => r.CurrencyCode == rate.CurrencyCode
                                                                && r.PangeaRate == rate.PangeaRate
                                                                && r.PaymentMethod == rate.PaymentMethod
                                                                && r.DeliveryMethod == rate.DeliveryMethod);
                Assert.Equal(expectedRate.CurrencyCode, rate.CurrencyCode);
                Assert.Equal(expectedRate.CountryCode, rate.CountryCode);
                Assert.Equal(expectedRate.PangeaRate, rate.PangeaRate);
                Assert.Equal(expectedRate.PaymentMethod, rate.PaymentMethod);
                Assert.Equal(expectedRate.DeliveryMethod, rate.DeliveryMethod);
            }
        }

        [Fact]
        public async Task GetPartnerRatesAsync_InvalidCountryCode_ReturnsBadRequest()
        {
            // Arrange
            var country = "JOD";
            _mockCountryCodeService.Setup(service => service.CountryCode).Returns(country);

            // Act
            var response = await _controller.GetPartnerRatesAsync(country);

            // Assert
            var badRequestResult = Assert.IsType<NotFoundObjectResult>(response.Result);
            Assert.Equal("No rates found for country code: " + country + ".", badRequestResult.Value);
        }

        [Fact]
        public async Task GetPartnerRatesAsync_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var country = "ME";
            _mockCountryCodeService.Setup(service => service.CountryCode).Returns(country);
            _mockPartnerRatesService.Setup(service => service.GetPartnerRatesAsync(country))
                .ThrowsAsync(new Exception("Service error"));

            // Act
            var response = await _controller.GetPartnerRatesAsync(country);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
            Assert.Equal("Country code must be three letters long.", badRequestResult.Value);
        }
    }
}