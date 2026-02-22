using InsuranceProject.Domain.Models;

namespace InsuranceProject.Domain.Validators
{
    public static class PolicyValidator
    {
        public static Error? ValidatePolicyPurchase(PurchaseQuoteRequest purchaseQuoteRequest, QuotedPolicy quotedPolicy)
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

        public static Error? ValidateCancellationRequest(CancelPolicyRequest cancelPolicyRequest, Policy policy)
        {
            if (policy.Id == Guid.Empty) return Error.PolicyNotFound;
            if (policy.HasClaims == true) return Error.ClaimsOnPolicyCancellation;
            if (policy.Payment!.PaymentType != cancelPolicyRequest.OriginalPaymentType) return Error.OriginalPaymentMethodMismatch(policy.Payment.PaymentType.ToString());
            return null;
        }
    }
}
