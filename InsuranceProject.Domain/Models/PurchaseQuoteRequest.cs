using InsuranceProject.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace InsuranceProject.Domain.Models
{
    public class PurchaseQuoteRequest
    {
        [Required]
        public required string PolicyId { get; set; }
        [Required]
        public required Payment Payment { get; set; }
        [Required]
        public bool AutoRenew { get; set; }
    }
}
