using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;

namespace InsuranceProject.Domain.Services
{
    public class ClaimService : IClaimService
    {
        private IDatabase _database;

        public ClaimService(IDatabase database)
        {
            _database = database;
        }

        public Result<ClaimResponse> CreatePolicyClaim(ClaimRequest claimRequest)
        {
            var policy = _database.GetPolicy(claimRequest.PolicyId);

            if (policy.Id == Guid.Empty) return Error.PolicyNotFound;

            policy.AddClaim(claimRequest.Claim);

            return new ClaimResponse()
            {
                GuidId = policy.Id
            };
        }
    }
}
