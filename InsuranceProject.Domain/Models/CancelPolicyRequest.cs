using InsuranceProject.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InsuranceProject.Domain.Models
{
    public class CancelPolicyRequest
    {
        [Required]
        public required string PolicyId { get; set; }
        [Required]
        [EnumDataType(typeof(PaymentType))]
        public required PaymentType OriginalPaymentType { get; set; }
    }
}
