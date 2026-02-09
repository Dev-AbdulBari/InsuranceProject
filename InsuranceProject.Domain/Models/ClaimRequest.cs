namespace InsuranceProject.Domain.Models
{
    public class ClaimRequest
    {
        public required string PolicyId { get; set; }
        public required Claim Claim { get; set; }
    }
}
