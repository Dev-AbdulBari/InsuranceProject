using InsuranceProject.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InsuranceProject.Domain.Models
{
    public class QuotePolicyRequest
    {
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(3)]
        public required IEnumerable<PolicyHolder> LegalPolicyHolders { get; set; }
        [Required]
        public required Property Property { get; set; }
        [Required]
        [EnumDataType(typeof(PolicyType))]
        public PolicyType PolicyType { get; set; }
    }
}
