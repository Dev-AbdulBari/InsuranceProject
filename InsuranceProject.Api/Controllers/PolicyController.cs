using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyController : BaseController
    {
        private IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpGet]
        [Route("retrieve/{id}")]
        public IActionResult GetPolicy(Guid id)
        {
            var policy = _policyService.GetPolicy(id.ToString());

            return ProcessResponse(policy);
        }

        [HttpPost]
        [Route("sell")]
        public IActionResult CreatePolicy([FromBody] PurchaseQuoteRequest purchaseQuoteRequest)
        {
            var quoteResponse = _policyService.CreatePolicy(purchaseQuoteRequest);

            return ProcessResponse(quoteResponse);
        }

        [HttpPost]
        [Route("cancel")]
        public IActionResult CancelPolicy(CancelPolicyRequest cancelPolicyRequest)
        {
            var cancelPolicyResponse = _policyService.CancelPolicy(cancelPolicyRequest, isQuote: false);

            return ProcessResponse(cancelPolicyResponse);
        }
    }
}
