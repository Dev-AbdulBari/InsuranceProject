namespace InsuranceProject.Domain.Models
{
    public class ClaimResponse
    {
        public Guid GuidId { get; set; }
        public string Message { get; private set; }

        public ClaimResponse()
        {
            Message = "Claim successfully registered on policy";
        }
    }
}
