using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using InsuranceProject.Domain.Models.Enums;

namespace InsuranceProject.Database.Services
{
    public class MemoryDbService: IDatabase
    {
        private List<Policy> _policies;
        private List<QuotedPolicy> _quotedPolicies;

        public MemoryDbService()
        {
            _policies = new List<Policy>();
            _quotedPolicies = new List<QuotedPolicy>();

            PopulateDemoDataInPolicies();
        }

        public Policy GetPolicy(string guidId)
        {
            return _policies.FirstOrDefault(x => x.Id.ToString() == guidId, new Policy());
        }

        public QuotedPolicy GetQuotedPolicy(string guidId)

        {
            return _quotedPolicies.FirstOrDefault(x => x.Id.ToString() == guidId, new QuotedPolicy(false));
        }

        public void CreatePolicy(Policy policy)
        {
           _policies.Add(policy);
        }

        public void CreateQuote(QuotedPolicy quotedPolicy)
        {
            _quotedPolicies.Add(quotedPolicy);
        }

        public void DeleteQuotedPolicy(QuotedPolicy quotedPolicy)
        {
            _quotedPolicies.Remove(quotedPolicy);
        }

        public void DeletePolicy(Policy policy)
        {
            _policies.Remove(policy);
        }

        private void PopulateDemoDataInPolicies()
        {
            _policies.Add(new Policy()
            {
                Id = Guid.Parse("33ef1a2d-b16c-428b-990d-fc00c3bef18d"),
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10).AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>
                {
                    new PolicyHolder()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(4).AddYears(-16)),
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "100 Home street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                },
                PolicyType = PolicyType.Household,
                Payment = new Payment
                {
                    Amount = 400,
                    PaymentType = PaymentType.Card
                },
                AutoRenew = false
            });

            _policies.Add(new Policy()
            {
                Id = Guid.Parse("5912698a-0511-4062-a073-edd2c7216fa3"),
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14).AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>
                {
                    new PolicyHolder()
                    {
                        FirstName = "Jack",
                        LastName = "Sanders",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14).AddYears(-16)),
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "102 Home street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                },
                PolicyType = PolicyType.BuyToLet,
                Payment = new Payment
                {
                    Amount = 600,
                    PaymentType = PaymentType.Card
                },
                AutoRenew = false
            });

            _policies.Add(new Policy()
            {
                Id = Guid.Parse("79ab1293-d213-47de-8aa2-70fb5794702c"),
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30).AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>
                {
                    new PolicyHolder()
                    {
                        FirstName = "Harris",
                        LastName = "Sultan",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30).AddMonths(-1).AddYears(-34)),
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "104 Home street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                },
                PolicyType = PolicyType.BuyToLet,
                Payment = new Payment
                {
                    Amount = 600,
                    PaymentType = PaymentType.Card
                },
                AutoRenew = false
            });

            _policies.Add(new Policy()
            {
                Id = Guid.Parse("c7044db7-8cb3-4a81-a03a-3c7ebfb9e643"),
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-320)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-320).AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>
                {
                    new PolicyHolder()
                    {
                        FirstName = "Adam",
                        LastName = "Khan",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-320).AddMonths(-1).AddYears(-34)),
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "106 Home street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                },
                PolicyType = PolicyType.Household,
                Payment = new Payment
                {
                    Amount = 400,
                    PaymentType = PaymentType.Card
                },
                AutoRenew = false
            });
        }
    }
}
