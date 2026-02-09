using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyController : Controller
    {
        private IPolicyRepository _policyRepository;

        public PolicyController(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        [HttpPost]
        [Route("quote")]
        public IActionResult QuotePolicy([FromBody] QuotePolicyRequest policy)
        {
            try
            {
                var quoteResponse = _policyRepository.CreateQuote(policy);

                return ProcessResponse(quoteResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("retrieve/{id}")]
        public IActionResult GetPolicy(Guid id)
        {
            try
            {
                var policy = _policyRepository.GetPolicy(id.ToString());

                return ProcessResponse(policy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("sell")]
        public IActionResult CreatePolicy([FromBody] PurchaseQuoteRequest purchaseQuoteRequest)
        {
            try
            {
                var quoteResponse  = _policyRepository.CreatePolicy(purchaseQuoteRequest);

                return ProcessResponse(quoteResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("claim")]
        public IActionResult CreatePolicyClaim([FromBody] ClaimRequest claimRequest)
        {
            try
            {
                var claimResponse = _policyRepository.CreateClaimOnPolicy(claimRequest);

                return ProcessResponse(claimResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("cancelQuote")]
        public IActionResult CancelPolicyQuote(CancelPolicyRequest cancelPolicyQuoteRequest)
        {
            try
            {
                var cancelPolicyResponse = _policyRepository.CancelPolicy(cancelPolicyQuoteRequest, isQuote: true);

                return ProcessResponse(cancelPolicyResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("cancel")]
        public IActionResult CancelPolicy(CancelPolicyRequest cancelPolicyRequest)
        {
            try
            {
                var cancelPolicyResponse = _policyRepository.CancelPolicy(cancelPolicyRequest, isQuote: false);

                return ProcessResponse(cancelPolicyResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        private IActionResult ProcessResponse<T>(Result<T> response)
        {
            if (!response.IsSuccess)
            {
                return response.Error!.Type switch
                {
                    ErrorType.NotFound => NotFound(response.Error.Description),
                    ErrorType.Validation => BadRequest(response.Error.Description),
                    _ => StatusCode(500, response.Error.Description)
                };
            }
            else
            {
                return Ok(response.Value);
            }
        }
    }
}
