using InsuranceProject.Domain.Models;

namespace InsuranceProject.Domain.Interfaces
{
    public interface IPolicyService
    {
        public Result<Policy> GetPolicy(string guidId);
        public Result<PurchaseQuoteResponse> CreatePolicy(PurchaseQuoteRequest purchaseQuoteRequest);
        public Result<CancelPolicyResponse> CancelPolicy(CancelPolicyRequest cancelPolicyRequest, bool isQuote);
    }
}
