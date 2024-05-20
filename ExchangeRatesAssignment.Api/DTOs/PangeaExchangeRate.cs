namespace ExchangeRatesAssignment.Api.DTOs
{
    public class PangeaExchangeRate
    {
        public required string CurrencyCode { get; set; }
        public required string CountryCode { get; set; }
        public decimal PangeaRate { get; set; }
        public required string PaymentMethod { get; set; }
        public required string DeliveryMethod { get; set; }
    }
}
