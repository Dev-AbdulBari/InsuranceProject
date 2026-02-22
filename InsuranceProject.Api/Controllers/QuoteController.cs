using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceProject.Api.Controllers
{
    public class QuoteController : BaseController
    {

        private IQuoteService _quoteService;

        public QuoteController(IQuoteService policyService)
        {
            _quoteService = policyService;
        }

        [HttpPost]
        [Route("quote")]
        public IActionResult QuotePolicy([FromBody] QuotePolicyRequest policy)
        {
            var quoteResponse = _quoteService.CreatePolicyQuote(policy);

            return ProcessResponse(quoteResponse);
        }

        [HttpPost]
        [Route("cancelQuote")]
        public IActionResult CancelPolicyQuote(CancelPolicyRequest cancelPolicyQuoteRequest)
        {
            var cancelPolicyResponse = _quoteService.GetPolicyCancelQuote(cancelPolicyQuoteRequest);

            return ProcessResponse(cancelPolicyResponse);
        }
    }
}
