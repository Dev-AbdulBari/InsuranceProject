using InsuranceProject.Domain.Models;

namespace InsuranceProject.Domain.Interfaces
{
    public interface IClaimService
    {
        public Result<ClaimResponse> CreatePolicyClaim(ClaimRequest claimRequest);
    }
}
