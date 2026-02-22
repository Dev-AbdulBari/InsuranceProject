using InsuranceProject.Domain.Models;

namespace InsuranceProject.Domain.Validators
{
    public static class QuoteValidator
    {
        public static Error? ValidateQuoteDetails(QuotePolicyRequest quotePolicyRequest)
        {
            var datesValidationError = DatesValidation(quotePolicyRequest);
            var policyHoldersValidationError = PolicyHoldersValidation(quotePolicyRequest);

            if (datesValidationError != null) return datesValidationError;
            if (policyHoldersValidationError != null) return policyHoldersValidationError;

            return null;
        }

        public static Error? ValidateCancellationRequest(CancelPolicyRequest cancelPolicyRequest, Policy policy)
        {
            if (policy.Id == Guid.Empty) return Error.PolicyNotFound;
            if (policy.HasClaims == true) return Error.ClaimsOnPolicyCancellation;
            if (policy.Payment!.PaymentType != cancelPolicyRequest.OriginalPaymentType) return Error.OriginalPaymentMethodMismatch(policy.Payment.PaymentType.ToString());
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

        private static bool IsPolicyHolderNotAboveValidAgeWhenPolicyStarts(QuotePolicyRequest quotePolicyRequest, PolicyHolder policyHolder)
        {
            var minimumPolicyHolderAge = 16;
            return policyHolder.DateOfBirth > quotePolicyRequest.StartDate.AddYears(-minimumPolicyHolderAge);
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
    }
}