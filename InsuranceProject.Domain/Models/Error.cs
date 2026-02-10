namespace InsuranceProject.Domain.Models
{
    public enum ErrorType { NotFound, Validation }

    public record Error(string Id, ErrorType Type, string Description)
    {
        public static Error StartDateOutOfRange { get; } = new("StartDateOutOfRange", ErrorType.Validation, "Start date must be within the next 60 days");
        public static Error EndDateOutOfRange { get; } = new("EndDateOutOfRange", ErrorType.Validation, "End date must be exactly one year from start date");
        public static Error PolicyHolderAge { get; } = new("PolicyHolderAge", ErrorType.Validation, "Policy holder age must 16 or older from start date");
        public static Error PolicyType { get; } = new("PolicyType", ErrorType.NotFound, "The policy type is not valid");
        public static Error PaymentType { get; } = new("PaymentType", ErrorType.NotFound, "The payment type is not valid");
        public static Error PolicyNotFound { get; } = new("PolicyNotFound", ErrorType.NotFound, "The policy could not be found");
        public static Error QuotedPolicyNotFound { get; } = new("QuotedPolicyNotFound", ErrorType.NotFound, "The policy quote could not be found");
        public static Error IncorrectPaymentAmount(decimal paymentAmount) => new("IncorrectPaymentAmount", ErrorType.Validation, $"Incorrect payment amount. Please pay the exact quote amount of {paymentAmount}");
        public static Error ClaimsOnPolicyCancellation { get; } = new("ClaimsOnPolicyCancellation", ErrorType.Validation, "Cannot cancel a policy that has claims");
        public static Error OriginalPaymentMethodMismatch(string originalPaymentType) => new("OriginalPaymentMethodMismatch", ErrorType.Validation, $"Payment method mismatch. The policy was paid via {originalPaymentType} and a refund can only be issued through the same payment type.");
    }
}
