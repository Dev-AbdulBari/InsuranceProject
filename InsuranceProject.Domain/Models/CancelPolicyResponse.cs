using InsuranceProject.Domain.Models.Enums;

namespace InsuranceProject.Domain.Models
{
    public class CancelPolicyResponse
    {
        public required string GuidId { get; set; }
        public required decimal RefundAmount { get; set; }
        public string Message { get; private set; }

        public CancelPolicyResponse(PolicyType policyType, bool isQuote)
        {
            if (isQuote)
            {
                Message = $"Your cancellation quote has been successfully generated. Your policy is still active and no changes have been applied";
            }
            else
            {
                Message = $"Your {policyType.ToString()} policy has been successfully cancelled.";
            }
        }
    }
}
