namespace InsuranceProject.Domain.Models
{
    public class QuotePolicyResponse
    {
        public required string GuidId { get; set; }
        public decimal Amount { get; set; }
        public DateTime QuoteExpirationDate { get; set; }
    }
}
