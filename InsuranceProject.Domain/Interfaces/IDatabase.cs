using InsuranceProject.Domain.Models;

namespace InsuranceProject.Domain.Interfaces
{
    public interface IDatabase
    {
        public Policy GetPolicy(string guidId);
        public void CreatePolicy(Policy policy);
        public void CreateQuote(QuotedPolicy quotedPolicy);
        public QuotedPolicy GetQuotedPolicy(string guidId);
        public void DeleteQuotedPolicy(QuotedPolicy quotedPolicy);
        public void DeletePolicy(Policy policy);
    }
}
