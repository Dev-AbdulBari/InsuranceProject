using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using InsuranceProject.Domain.Validators;

namespace InsuranceProject.Domain.Services
{
    public class PolicyService : IPolicyService
    {
        private IDatabase _database;

        public PolicyService(IDatabase database)
        {
            _database = database;
        }

        public Result<Policy> GetPolicy(string guidId)
        {
            var policy = _database.GetPolicy(guidId);

            if (policy.Id == Guid.Empty) return Error.PolicyNotFound;

            return policy;
        }

        public Result<PurchaseQuoteResponse> CreatePolicy(PurchaseQuoteRequest purchaseQuoteRequest)
        {
            var quotedPolicy = _database.GetQuotedPolicy(purchaseQuoteRequest.PolicyId);

            var validationError = PolicyValidator.ValidatePolicyPurchase(purchaseQuoteRequest, quotedPolicy);

            if (validationError != null) return validationError;

            quotedPolicy.AutoRenew = purchaseQuoteRequest.AutoRenew;
            quotedPolicy.Payment = purchaseQuoteRequest.Payment;

            _database.DeleteQuotedPolicy(quotedPolicy);

            _database.CreatePolicy(quotedPolicy);

            return new PurchaseQuoteResponse(quotedPolicy.PolicyType)
            {
                GuidId = quotedPolicy.Id,
            };
        }

        public Result<CancelPolicyResponse> CancelPolicy(CancelPolicyRequest cancelPolicyRequest, bool isQuote)
        {
            var policy = _database.GetPolicy(cancelPolicyRequest.PolicyId);

            var validationError = PolicyValidator.ValidateCancellationRequest(cancelPolicyRequest, policy);
            if (validationError != null) return validationError;

            decimal refundAmount;

            refundAmount = CalculateRefundAmount(policy);

            if (!isQuote)
            {
                _database.DeletePolicy(policy);
            }

            return new CancelPolicyResponse(policy.PolicyType, isQuote) { GuidId = policy.Id.ToString(), RefundAmount = refundAmount };
        }

        private static decimal CalculateRefundAmount(Policy policy)
        {
            decimal refundAmount;

            if (policy.StartDate > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                refundAmount = policy.Payment!.Amount;
            }
            else if (policy.StartDate.AddDays(14) >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                refundAmount = policy.Payment!.Amount;
            }
            else
            {
                var totalDaysInPolicy = policy.EndDate.DayNumber - policy.StartDate.DayNumber;
                var totalDaysSinceStartDate = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - policy.StartDate.DayNumber;

                var refundProRataCalculation = (decimal)(totalDaysInPolicy - totalDaysSinceStartDate) / totalDaysInPolicy * policy.Payment!.Amount;

                refundAmount = Math.Round(refundProRataCalculation, 2);
            }

            return refundAmount;
        }
    }
}
