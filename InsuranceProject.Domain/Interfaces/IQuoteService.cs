using InsuranceProject.Domain.Models;

namespace InsuranceProject.Domain.Interfaces
{
    public interface IQuoteService
    {
        public Result<QuotePolicyResponse> CreatePolicyQuote(QuotePolicyRequest quotePolicyRequest);
        public Result<CancelPolicyResponse> GetPolicyCancelQuote(CancelPolicyRequest cancelPolicyRequest);
    }
}
