using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using InsuranceProject.Domain.Models.Enums;
using InsuranceProject.Domain.Validators;

namespace InsuranceProject.Domain.Services
{
    public class QuoteService : IQuoteService
    {
        private IDatabase _database;

        public QuoteService(IDatabase database)
        {
            _database = database;
        }

        public Result<QuotePolicyResponse> CreatePolicyQuote(QuotePolicyRequest quotePolicyRequest)
        {
            var validationError = QuoteValidator.ValidateQuoteDetails(quotePolicyRequest);
            if (validationError != null) return validationError;

            var dateInThirtyDays = DateTime.UtcNow.AddDays(30);

            var quotedPolicy = new QuotedPolicy()
            {
                StartDate = quotePolicyRequest.StartDate,
                EndDate = quotePolicyRequest.EndDate,
                LegalPolicyHolders = quotePolicyRequest.LegalPolicyHolders,
                Property = quotePolicyRequest.Property,
                PolicyType = quotePolicyRequest.PolicyType,
                Amount = GeneratePrice(quotePolicyRequest.PolicyType),
                QuoteExpirationDate = dateInThirtyDays
            };

            _database.CreateQuote(quotedPolicy);

            return new QuotePolicyResponse
            {
                GuidId = quotedPolicy.Id.ToString(),
                Amount = quotedPolicy.Amount,
                QuoteExpirationDate = quotedPolicy.QuoteExpirationDate
            };
        }

        public Result<CancelPolicyResponse> GetPolicyCancelQuote(CancelPolicyRequest cancelPolicyRequest)
        {
            var policy = _database.GetPolicy(cancelPolicyRequest.PolicyId);

            var validationError = QuoteValidator.ValidateCancellationRequest(cancelPolicyRequest, policy);
            if (validationError != null) return validationError;

            decimal refundAmount;

            refundAmount = CalculateRefundAmount(policy);

            return new CancelPolicyResponse(policy.PolicyType, true) { GuidId = policy.Id.ToString(), RefundAmount = refundAmount };
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

        private decimal GeneratePrice(PolicyType policyType)
        {
            switch (policyType)
            {
                case PolicyType.Household:
                    return 400;
                case PolicyType.BuyToLet:
                    return 600;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
