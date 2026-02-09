namespace InsuranceProject.Domain.Models
{
    public class PolicyHolder
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
