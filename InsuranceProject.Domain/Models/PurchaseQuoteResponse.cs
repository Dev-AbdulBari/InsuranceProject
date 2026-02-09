using InsuranceProject.Domain.Models.Enums;

namespace InsuranceProject.Domain.Models
{
    public class PurchaseQuoteResponse
    {
        public Guid GuidId { get; set; }
        public string Message { get; private set; }

        public PurchaseQuoteResponse(PolicyType policyType)
        {
            Message = $"Purchase successful! Your {policyType.ToString()} policy is now active";
        }
    }
}
