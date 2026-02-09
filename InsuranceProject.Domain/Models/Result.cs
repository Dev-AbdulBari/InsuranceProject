namespace InsuranceProject.Domain.Models
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(Error error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(T value) : base(true, null) => Value = value;
        private Result(Error error) : base(false, error) { }

        public static implicit operator Result<T>(T value) => new(value);

        public static implicit operator Result<T>(Error error) => new(error);
    }

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
    }
}
