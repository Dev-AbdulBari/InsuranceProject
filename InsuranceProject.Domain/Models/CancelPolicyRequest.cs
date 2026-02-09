using System.ComponentModel.DataAnnotations;

namespace InsuranceProject.Domain.Models
{
    public class CancelPolicyRequest
    {
        [Required]
        public required string PolicyId { get; set; }
    }
}
