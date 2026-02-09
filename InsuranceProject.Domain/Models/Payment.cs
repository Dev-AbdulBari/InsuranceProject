using InsuranceProject.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InsuranceProject.Domain.Models
{
    public class Payment
    {
        [JsonPropertyName("PaymentReference")]
        public Guid Id => Guid.NewGuid();
        [Required]
        [EnumDataType(typeof(PaymentType))]
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeName => PaymentType.ToString();
        [Required]
        public decimal Amount { get; set; }
    }
}
