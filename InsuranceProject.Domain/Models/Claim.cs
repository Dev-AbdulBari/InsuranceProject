namespace InsuranceProject.Domain.Models
{
    public class Claim
    {
        public required string Reason { get; set; }
        public DateTime Date { get; private set; }

        public Claim()
        {
            Date = DateTime.UtcNow;
        }
    }
}
