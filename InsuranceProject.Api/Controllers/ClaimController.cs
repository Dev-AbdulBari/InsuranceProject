using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceProject.Api.Controllers
{
    public class ClaimController : BaseController
    {
        private IClaimService _claimService;

        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpPost]
        [Route("claim")]
        public IActionResult CreatePolicyClaim([FromBody] ClaimRequest claimRequest)
        {
            var claimResponse = _claimService.CreatePolicyClaim(claimRequest);

            return ProcessResponse(claimResponse);

        }
    }
}
