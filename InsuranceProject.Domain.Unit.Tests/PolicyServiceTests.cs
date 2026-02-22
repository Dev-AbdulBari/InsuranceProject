using InsuranceProject.Domain.Interfaces;
using InsuranceProject.Domain.Models;
using InsuranceProject.Domain.Models.Enums;
using InsuranceProject.Domain.Services;
using Moq;

namespace InsuranceProject.Domain.Unit.Tests
{
    public class PolicyServiceTests
    {
        private PolicyService _policyService;
        private Mock<IDatabase> _mockDatabase;

        [SetUp]
        public void Setup()
        {
            _mockDatabase = new Mock<IDatabase>();
            _policyService = new PolicyService(_mockDatabase.Object);
        }

        [Test]
        public void CreateQuote_WhenCalledWithValidData_ShouldStoreAndReturnQuote()
        {
            var quotePolicyRequest = new QuotePolicyRequest()
            {
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>()
                {
                    new PolicyHolder()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-24))
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "100 Home Street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                }
            };

            //var policyQuote = _policyRepository.CreateQuote(quotePolicyRequest);

            _mockDatabase.Verify(x => x.CreateQuote(It.IsAny<QuotedPolicy>()), Times.Once);
            //Assert.That(policyQuote.IsSuccess, Is.True);
        }

        [Test]
        public void CreateQuote_WhenCalledWithStartDateMoreThanSixtyDays_ShouldFailAndReturnCorrectErrorResponse()
        {
            var quotePolicyRequest = new QuotePolicyRequest()
            {
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(61)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(61).AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>()
                {
                    new PolicyHolder()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-24))
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "100 Home Street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                }
            };

            //var policyQuote = _policyRepository.CreateQuote(quotePolicyRequest);

            //Assert.That(policyQuote.IsSuccess, Is.False);
            //Assert.That(policyQuote.Error == Error.StartDateOutOfRange);
        }

        [Test]
        public void CreateQuote_WhenCalledWithStartDateInThePast_ShouldFailAndReturnCorrectErrorResponse()
        {
            var quotePolicyRequest = new QuotePolicyRequest()
            {
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1).AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>()
                {
                    new PolicyHolder()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-24))
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "100 Home Street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                }
            };

            //var policyQuote = _policyRepository.CreateQuote(quotePolicyRequest);

            //Assert.That(policyQuote.IsSuccess, Is.False);
            //Assert.That(policyQuote.Error == Error.StartDateOutOfRange);
        }

        [Test]
        public void CreateQuote_WhenCalledWithEndDateNotExactlyOneYearInLength_ShouldFailAndReturnCorrectErrorResponse()
        {
            var quotePolicyRequest = new QuotePolicyRequest()
            {
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1).AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>()
                {
                    new PolicyHolder()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-24))
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "100 Home Street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                }
            };

            //var policyQuote = _policyRepository.CreateQuote(quotePolicyRequest);

            //Assert.That(policyQuote.IsSuccess, Is.False);
            //Assert.That(policyQuote.Error == Error.EndDateOutOfRange);
        }

        [Test]
        public void CreateQuote_WhenCalledWithUnderagePolicyHolders_ShouldFailAndReturnCorrectErrorResponse()
        {
            var quotePolicyRequest = new QuotePolicyRequest()
            {
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                LegalPolicyHolders = new List<PolicyHolder>()
                {
                    new PolicyHolder()
                    {
                        FirstName = "John",
                        LastName = "Smith",
                        DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-15))
                    }
                },
                Property = new Property
                {
                    AddressLine1 = "100 Home Street",
                    AddressLine2 = "London",
                    Postcode = "L1 1RS"
                }
            };

            //var policyQuote = _policyRepository.CreateQuote(quotePolicyRequest);

            //Assert.That(policyQuote.IsSuccess, Is.False);
            //Assert.That(policyQuote.Error == Error.PolicyHolderAge);
        }

        [Test]
        public void CancelQuote_WhenCalledWithPolicyThatHasClaim_ShouldFailAndReturnCorrectResponse()
        {
            var policy = new Policy()
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
            };

            policy.AddClaim(new Claim 
            { 
                Reason = "Roof repairs"
            });

            var cancellationRequest = new CancelPolicyRequest
            {
                PolicyId = "33ef1a2d-b16c-428b-990d-fc00c3bef18d",
                OriginalPaymentType = 0
            };

            _mockDatabase.Setup(x => x.GetPolicy(It.IsAny<string>())).Returns(policy);

            //var cancellationResponse = _policyRepository.CancelPolicy(cancellationRequest, isQuote: true);

            //Assert.That(cancellationResponse.IsSuccess, Is.False);
            //Assert.That(cancellationResponse.Error == Error.ClaimsOnPolicyCancellation);
        }

        [Test]
        public void CancelQuote_WhenCalledWithPolicyWithinCoolingPeriod_ShouldRefundFullAmount()
        {
            var policyCost = 400;

            var policy = new Policy()
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
                    Amount = policyCost,
                    PaymentType = PaymentType.Card
                },
                AutoRenew = false
            };

            var cancellationRequest = new CancelPolicyRequest
            {
                PolicyId = "33ef1a2d-b16c-428b-990d-fc00c3bef18d",
                OriginalPaymentType = 0
            };

            _mockDatabase.Setup(x => x.GetPolicy(It.IsAny<string>())).Returns(policy);

            //var cancellationResponse = _policyRepository.CancelPolicy(cancellationRequest, isQuote: true);

            //Assert.That(cancellationResponse.IsSuccess, Is.True);
            //Assert.That(cancellationResponse.Value!.RefundAmount == policyCost);
        }

        [Test]
        public void CancelQuote_WhenCalledWithPolicyAfterCoolingPeriod_ShouldRefundBasedOnProRata()
        {
            var policyCost = 400;
            var daysInAYear = 365;
            var daysInALeapYear = 366;
            var daysPolicyUsed = 16;

            var expectedRefundAmountForANormalYear = Math.Round(((decimal)(daysInAYear - daysPolicyUsed) / daysInAYear) * policyCost, 2);
            var expectedRefundAmountForALeapYear = Math.Round(((decimal)(daysInALeapYear - daysPolicyUsed) / daysInALeapYear) * policyCost, 2);

            var policy = new Policy()
            {
                Id = Guid.Parse("33ef1a2d-b16c-428b-990d-fc00c3bef18d"),
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-daysPolicyUsed)),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-daysPolicyUsed).AddYears(1)),
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
                    Amount = policyCost,
                    PaymentType = PaymentType.Card
                },
                AutoRenew = false
            };

            var cancellationRequest = new CancelPolicyRequest
            {
                PolicyId = "33ef1a2d-b16c-428b-990d-fc00c3bef18d",
                OriginalPaymentType = 0
            };

            _mockDatabase.Setup(x => x.GetPolicy(It.IsAny<string>())).Returns(policy);

            //var cancellationResponse = _policyRepository.CancelPolicy(cancellationRequest, isQuote: true);

            //Assert.That(cancellationResponse.IsSuccess, Is.True);
            //Assert.That(cancellationResponse.Value!.RefundAmount == expectedRefundAmountForANormalYear || cancellationResponse.Value!.RefundAmount == expectedRefundAmountForALeapYear);
        }
    }
}
