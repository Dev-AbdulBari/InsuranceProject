using InsuranceProject.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceProject.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult ProcessResponse<T>(Result<T> response)
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
