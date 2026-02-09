namespace InsuranceProject.Domain.Models
{
    public class QuotedPolicy : Policy
    {
        public QuotedPolicy(bool generateId = true)
        {
            if (generateId) base.Id = Guid.NewGuid();
        }

        public DateTime QuoteExpirationDate {  get; set; }
    }
}
