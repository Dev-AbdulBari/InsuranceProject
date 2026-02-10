# Insurance Project

A REST API project written in C# .NET 9 to meet the requirements of a technical assessment. I built this project strongly adhering to S.O.L.I.D principles and domain focused hexagonal architecture. Also included are unit tests to demonstrate testing capabilities within the NUnit framework (+ Moq library)

## Functionality Overview
The API has a list of endpoints to help manage policies:
- Quote - Get a quote for your home insurance policy
- Sell - Purchase your home insurance policy
- Retrieve - Retrieve details for an active home insurance policy
- Claim - Add a claim to a policy
- CancelQuote - Get a quote for cancelling your active policy with details for refund amount without actually cancelling
- Cancel - Cancel an active policy

## Instructions
### Running the project:
- Simply open up the `InsuranceProject.sln` file in visual studio
- In the solution explorer, right click on the `InsuranceProject.Api` and select `Set as Startup Project` from the action menu
- Click the green play icon to run the application

**Please note your localhost URL and port in the cmd pop-up as this can differ from examples**
Throughout the remainder of the readme I will use https://localhost:7156/ as the example application URL and port.

I recommend **Postman** for testing this API
### Quote Endpoint - [Post] `https://localhost:7156/api/policy/quote`

Details must be inputted as in the same JSON format as the example and must follow these rules:
- Start date must not be in the past
- Start date must not be more than 60 days from today
- End date must be exactly 1 year from the start date
- All legal policy holders must be aged 16 or older from the start date
- Minimum of 1 legal policy holders but no more than 3
- Policy type must be valid
	- 0 - Household
	- 1 - Buy To Let

<details>
<summary>Click to Show Example JSON request</summary>

```json
{

"StartDate": "2026-04-10",

"EndDate" : "2027-04-10",

"LegalPolicyHolders" : [

{

"FirstName" : "John",

"LastName" : "Doe",

"DateOfBirth" : "2010-04-10"

}
],

"Property" : {

"AddressLine1" : "100 Home street",

"AddressLine2" : "London",

"Postcode" : "L1 1RS"

},

"PolicyType" : 1

}
```
</details>

If accepted, the API will respond with:
- guidId - A uniquely generated policy id
- amount - A quote cost for the policy
- quoteExpirationDate - A date 30 days from today for when the quote should expire 
	- Not currently used but inserted as an idea for how this project could shape

<details>
<summary>Click to Show Example JSON response</summary>

```json
{

"guidId": "c37feb84-0d17-4a21-8369-bb5de0eceea5",

"amount": 600,

"quoteExpirationDate": "2026-03-12T11:45:30.9954981Z"

}
```
</details>

**Please note:** This is just a quote and not an actual policy, you will need to use this **guidId** in the sell endpoint when making a request to actually purchase the quote

### Sell Endpoint - [Post] `https://localhost:7156/api/policy/sell`

Details must be inputted as in the same JSON format as the example and must follow these rules:

#### Format
- PolicyId -  Input here the previously-generated **guidId** from the quote endpoint
- Payment
	- PaymentType - choose a value from the following
		- 0 - Card
		- 1 - Direct Debit
		- 2 - Cheque
	- Amount - please enter here the amount from the quote
- AutoRenew - true if you wish to auto renew policy otherwise false
	- Not currently used but inserted as an idea for how this project could shape
#### Rules
- PolicyId must be a valid policy id
- PaymentType must be a valid option
- Payment amount must be exactly the quote amount

<details>
<summary>Click to Show Example JSON request</summary>

```json
{

"PolicyId" : "c37feb84-0d17-4a21-8369-bb5de0eceea5",

"Payment" : {

"PaymentType": 0,

"Amount" : 600

},

"AutoRenew" : false

}
```
</details>

If accepted, the API will respond with:
- guidId - The same policy id
- message - A message that confirms, by naming the policy type purchased, a successful purchase and activation of policy

<details>
<summary>Click to Show Example JSON response</summary>

```json

{

"guidId": "c37feb84-0d17-4a21-8369-bb5de0eceea5",

"message": "Purchase successful! Your BuyToLet policy is now active"

}
```
</details>

### Retrieve Endpoint - [Get] `https://localhost:7156/api/policy/retrieve/` + guidId

For this endpoint you simply make a request and provide a valid **guidId** in the URL and it will retrieve details for the active policy.
For example;
https://localhost:7156/api/policy/retrieve/c37feb84-0d17-4a21-8369-bb5de0eceea5

The API will then respond with the following details:
<details>
<summary>Click to Show Example JSON response</summary>

```json
{

"quoteExpirationDate": "2026-03-12T11:45:30.9954981Z",

"UniqueReference": "c37feb84-0d17-4a21-8369-bb5de0eceea5",

"startDate": "2026-04-10",

"endDate": "2027-04-10",

"amount": 600,

"claims": [],

"hasClaims": false,

"autoRenew": false,

"legalPolicyHolders": [

{

"firstName": "John",

"lastName": "Doe",

"dateOfBirth": "2010-04-10"

}

],

"property": {

	"addressLine1": "100 Home street",

	"addressLine2": "London",

	"postcode": "L1 1RS"

},

"policyType": 1,

"policyTypeName": "BuyToLet",

"payment": {

	"PaymentReference": "b2b32831-b8a8-4fa2-8002-101c71cde00d",

	"paymentType": 0,

	"paymentTypeName": "Card",

	"amount": 600

	}
}
```
</details>

### Claim Endpoint - [Post] `https://localhost:7156/api/policy/claim`

Details must be inputted as in the same JSON format as the example and must follow these rules:

#### Format
- PolicyId -  Input here the previously-generated **guidId** from the quote endpoint
- Claim
	- Reason - input the reason for the claim
#### Rules
- PolicyId must be a valid policy id

<details>
<summary>Click to Show Example JSON request</summary>

```json

{

"PolicyId" : "c37feb84-0d17-4a21-8369-bb5de0eceea5",

"Claim" : {

	"Reason" : "Roof repairs needed"

}

}
```
</details>

<details>
<summary>Click to Show Example JSON response</summary>

```json
{

"guidId": "c37feb84-0d17-4a21-8369-bb5de0eceea5",

"message": "Claim successfully registered on policy"

}
```
</details>

### CancelQuote Endpoint - [Post] `https://localhost:7156/api/policy/cancelquote`
This endpoint allows you to generate a cancellation quote for an active policy. It will calculate the refund amount you would receive **without actually cancelling** the policy.

#### Format
-   PolicyId - Input here the previously-generated **guidId** from the quote endpoint
#### Rules
-   PolicyId must be a valid active policy id    
-   The policy must not already be cancelled

<details>
<summary>Click to Show Example JSON request</summary>

```json
{
  "PolicyId": "c37feb84-0d17-4a21-8369-bb5de0eceea5"
}
```
</details>

If accepted, the API will respond with:
-   guidId - The policy id    
-   refundAmount - The calculated refund amount    
-   message - Confirmation that the policy is still active

<details>
<summary>Click to Show Example JSON response</summary>

```json
{
  "guidId": "c37feb84-0d17-4a21-8369-bb5de0eceea5",
  "refundAmount": 600,
  "message": "Your cancellation quote has been successfully generated. Your policy is still active and no changes have been applied"
}
```
</details>

### Cancel Endpoint - [Post] `https://localhost:7156/api/policy/cancel`

This endpoint will fully cancel an active policy. Once cancelled, the policy will no longer remain active and cannot be claimed against.

#### Format
-   PolicyId - Input here the active policy **guidId**   
#### Rules
-   PolicyId must be a valid active policy id    
-   The policy must not already be cancelled
<details>
<summary>Click to Show Example JSON request</summary>

```json
{
  "PolicyId": "c37feb84-0d17-4a21-8369-bb5de0eceea5"
}
```
</details>

If accepted, the API will respond with:
-   guidId - The cancelled policy id    
-   refundAmount - The final refund issued    
-   message - Confirmation that the policy has been cancelled

<details>
<summary>Click to Show Example JSON response</summary>

```json
{
  "guidId": "c37feb84-0d17-4a21-8369-bb5de0eceea5",
  "refundAmount": 600,
  "message": "Your BuyToLet policy has been successfully cancelled."
}
```
</details>
