using InsuranceProject.Domain.Models.Enums;
using System.Text.Json.Serialization;

namespace InsuranceProject.Domain.Models
{
    public class Policy
    {
        [JsonPropertyName("UniqueReference")]
        public Guid Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal Amount { get; set; }
        public IList<Claim> Claims { get; private set; }
        public bool HasClaims => Claims.Count > 0;
        public bool AutoRenew { get; set; }
        public IEnumerable<PolicyHolder>? LegalPolicyHolders { get; set; }
        public Property? Property { get; set; }
        public PolicyType PolicyType { get; set; }
        public string PolicyTypeName => PolicyType.ToString();
        public Payment? Payment { get; set; }

        public Policy()
        {
            Claims = new List<Claim>();
        }

        public void AddClaim(Claim claim)
        {
            Claims.Add(claim);
        }
    }
}
