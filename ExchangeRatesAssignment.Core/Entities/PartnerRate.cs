namespace ExchangeRatesAssignment.Core.Entities
{
    public class PartnerRate
    {
        public required string Currency { get; set; }
        public required string PaymentMethod { get; set; }
        public required string DeliveryMethod { get; set; }
        public decimal Rate { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }
}