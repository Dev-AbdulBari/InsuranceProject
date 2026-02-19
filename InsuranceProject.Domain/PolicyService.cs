using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using InsuranceProject.Domain.Models.Enums;

namespace InsuranceProject.Domain
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

            var validationError = ValidatePolicyPurchase(purchaseQuoteRequest, quotedPolicy);

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

        public Result<QuotePolicyResponse> CreateQuote(QuotePolicyRequest quotePolicyRequest)
        {
            var validationError = ValidateQuoteDetails(quotePolicyRequest);
            if (validationError != null) return validationError;

            var quotedPolicy = new QuotedPolicy()
            {
                StartDate = quotePolicyRequest.StartDate,
                EndDate = quotePolicyRequest.EndDate,
                LegalPolicyHolders = quotePolicyRequest.LegalPolicyHolders,
                Property = quotePolicyRequest.Property,
                PolicyType = quotePolicyRequest.PolicyType,
                Amount = GeneratePrice(quotePolicyRequest.PolicyType),
                QuoteExpirationDate = DateTime.UtcNow.AddDays(30)
            };

            _database.CreateQuote(quotedPolicy);

            return new QuotePolicyResponse
            {
                GuidId = quotedPolicy.Id.ToString(),
                Amount = quotedPolicy.Amount,
                QuoteExpirationDate = quotedPolicy.QuoteExpirationDate
            };
        }

        public Result<ClaimResponse> CreateClaimOnPolicy(ClaimRequest claimRequest)
        {
            var policy = _database.GetPolicy(claimRequest.PolicyId);

            if (policy.Id == Guid.Empty) return Error.PolicyNotFound;

            policy.AddClaim(claimRequest.Claim);

            return new ClaimResponse()
            { 
                GuidId = policy.Id
            };
        }

        public Result<CancelPolicyResponse> CancelPolicy(CancelPolicyRequest cancelPolicyRequest, bool isQuote)
        {
            var policy = _database.GetPolicy(cancelPolicyRequest.PolicyId);

            var validationError = ValidateCancellationRequest(cancelPolicyRequest, policy);
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

                var refundProRataCalculation = ((decimal)(totalDaysInPolicy - totalDaysSinceStartDate) / totalDaysInPolicy) * policy.Payment!.Amount;

                refundAmount = Math.Round(refundProRataCalculation, 2);
            }

            return refundAmount;
        }

        private static Error? ValidateCancellationRequest(CancelPolicyRequest cancelPolicyRequest, Policy policy)
        {
            if (policy.Id == Guid.Empty) return Error.PolicyNotFound;
            if (policy.HasClaims == true) return Error.ClaimsOnPolicyCancellation;
            if (policy.Payment!.PaymentType != cancelPolicyRequest.OriginalPaymentType) return Error.OriginalPaymentMethodMismatch(policy.Payment.PaymentType.ToString());
            return null;
        }

        private static Error? ValidateQuoteDetails(QuotePolicyRequest quotePolicyRequest)
        {
            var datesValidationError = DatesValidation(quotePolicyRequest);
            var policyHoldersValidationError = PolicyHoldersValidation(quotePolicyRequest);

            if (datesValidationError != null) return datesValidationError;
            if (policyHoldersValidationError != null) return policyHoldersValidationError;

            return null;
        }

        private static Error? ValidatePolicyPurchase(PurchaseQuoteRequest purchaseQuoteRequest, QuotedPolicy quotedPolicy)
        {
            if (quotedPolicy.Id == Guid.Empty)
            {
                return Error.QuotedPolicyNotFound;
            }

            if (quotedPolicy.Amount != purchaseQuoteRequest.Payment.Amount)
            {
                return Error.IncorrectPaymentAmount(quotedPolicy.Amount);
            }

            return null;
        }

        private static Error? PolicyHoldersValidation(QuotePolicyRequest quotePolicyRequest)
        {
            foreach (var policyHolder in quotePolicyRequest.LegalPolicyHolders)
            {
                if (IsPolicyHolderNotAboveValidAgeWhenPolicyStarts(quotePolicyRequest, policyHolder))
                {
                    return Error.PolicyHolderAge;
                }
            }

            return null;
        }

        private static bool IsPolicyHolderNotAboveValidAgeWhenPolicyStarts(QuotePolicyRequest quotePolicyRequest, PolicyHolder policyHolder)
        {
            var minimumPolicyHolderAge = 16;
            return policyHolder.DateOfBirth > quotePolicyRequest.StartDate.AddYears(-minimumPolicyHolderAge);
        }
        private static Error? DatesValidation(QuotePolicyRequest quotePolicyRequest)
        {
            if (IsStartDateInPast(quotePolicyRequest))
            {
                return Error.StartDateOutOfRange;
            }
            else if (IsStartDateNotWithinAllowedDaysInAdvance(quotePolicyRequest))
            {
                return Error.StartDateOutOfRange;
            }

            if (IsStartDateNotOneYearInLength(quotePolicyRequest))
            {
                return Error.EndDateOutOfRange;
            }

            return null;
        }

        private static bool IsStartDateNotOneYearInLength(QuotePolicyRequest quotePolicyRequest)
        {
            return quotePolicyRequest.StartDate.AddYears(1) != quotePolicyRequest.EndDate;
        }

        private static bool IsStartDateNotWithinAllowedDaysInAdvance(QuotePolicyRequest quotePolicyRequest)
        {
            var allowedDaysInAdvaned = 60;
            return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(allowedDaysInAdvaned)) < quotePolicyRequest.StartDate;
        }

        private static bool IsStartDateInPast(QuotePolicyRequest quotePolicyRequest)
        {
            return DateOnly.FromDateTime(DateTime.Today) > quotePolicyRequest.StartDate;
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
