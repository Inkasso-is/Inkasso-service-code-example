# Inkasso IOBS Service
Code example for inkasso demo,

## Table of Contents
# Inkasso IOBS Service
- [Description](#description)
- [AddFinalPayment](#addfinalpayment)
- [AddPayment](#addpayment)
- [GetAddPaymentResult](#getaddpaymentresult)
- [GetFinalPayments](#getfinalpayments)
- [GetPostponeDate](#getpostponedate)
- [QueryPaymentPlans](#querypaymentplans)
- [SetPostponementDate](#setpostponementdate)
- [QueryPostponeDate](#querypostponedate)
- [StopCollection](#stopcollection)
- [StartCollection](#startcollection)
- [ReturnClaim](#returnclaim)
- [GetReturnClaimResult](#getreturnclaimresult)
- [AddCommentToClaim](#addcommenttoclaim)
- [AddCommentToClient](#addcommenttoclient)
- [GetClaimHistory](#getclaimhistory)
- [SendClaimDocument](#sendclaimdocument)
- [Complex objects](#complex-objects)
- [BankClaimKey](#bankclaimkey)
- [PaymentPlanRequestResult](#paymentplanrequestresult)
- [FinalPaymentRecord](#finalpaymentrecord)
- [PaymentPlanWS](#paymentplanws)
- [InstallmentWS](#installmentws)
- [PlanClaimWS](#planclaimws)
- [PaymentWS](#paymentws)
- [InkassoServicePaymentSuccess](#inkassoservicepaymentsuccess)
- [InkassoServicePaymentError](#inkassoservicepaymenterror)
- [PaymentAmountData](#paymentamountdata)
- [InkassoServiceReturnClaimResult](#inkassoservicereturnclaimresult)
- [InkassoServiceClaimError](#inkassoserviceclaimerror)
- [Enums](#enums)
- [PaymentPlanItemStatus](#paymentplanitemstatus)
- [ClaimStatus](#claimstatus)
- [OperationStatus](#operationstatus)
- [InkassoServiceErrorCode](#inkassoserviceerrorcode)
- [URL redirect service](#url-redirect-service)
  - [Service Instances](#service-instances)
- [Redirect to client](#redirect-to-client)
- [Redirect to claim](#redirect-to-claim)

## Description

A collection of services that Inkasso provides to claimants that have requirements that go beyond the IOBS service.
|  Instance | URL  |  
|---|---|
|  Live | [https://secure.inkasso.is/SOAP/IOBS/IcelandicOnlineBankingClaimsSoap.svc](https://secure.inkasso.is/SOAP/InkassoBasicService/InkassoBasicService.svc)  |   
|  DEMO | [https://demo-inkasso.azurewebsites.net/SOAP/IOBS/IcelandicOnlineBankingClaimsSoap.svc](https://demo.inkasso.is/SOAP/InkassoBasicService/InkassoBasicService.svc)https://demo.inkasso.is/SOAP/InkassoBasicService/InkassoBasicService.svc   |  

For demo service the username for the service is servicetest and password is znvwYV5
ClaimantId to be used is 1021021020

Supported payor ids are any legal kennitala and the following dummy ids:  
•	Individual 
- 2902880029
- 3112551189
- 0606972218
- 0505233309
- 0101059940 
    - Payor is under 18 years of age.
    - System will collect parent 3112551189 

•	Company 
- 4101104580
- 4505145510
- 5612056630
- 6412877769
- 7108558899 
 
Primary collection identifier:  	1102-XXX 
Secondary collection identifier:  1102-MIX 

## AddFinalPayment

Adds final payment with the given amount. Claim marked as paid. Bank claim is cancelled
If service returns Status In progress (0) then you need to check later using service GetAddPaymentResult.

### Input
| Type                   | Name          | Description                                                                                                                                                                                |
|------------------------|---------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| decimal                | BankClaimKey  | Account ID of bank claim.                                                                                                                                                                   |
| decimal                | amount        | Amount that was paid as a full payment.                                                                                                                                                     |
| decimal                | splitCoef     | It is a number from [0,1]. Indicates the proportion of the payment that should be allocated to Inkasso. If the ratio cannot be fulfilled, it assigns the remaining money to the other side. There is one caveat, unconfirmed fees are assigned only if both confirmed fees and claimant’s part are already satisfied. Thus unconfirmed fees fall outside of the coefficient. Basically, you should have it as an editable variable on your end (part of setup) and have it set to 1. That means that the payment is distributed in the traditional way, first towards Annar vanskilakostnaður (AVAN), then to Annar kost. |
### Output
| Type                                      | Name                  | Description                                                                                                                                                        |
|-------------------------------------------|-----------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| long                                      | Id                    | The identifier for the operation.                                                                                                                                 |
| OperationStatus                           | Status                | The status of the operation.                                                                                                                                      |
| string                                    | Error                 | An error message associated with the operation.                                                                                                                   |
| List\<InkassoServicePaymentSuccess\>       | Success               | A list of successful payment information for the Inkasso service.                                                                                                 |
| List\<InkassoServicePaymentError\>         | Errors                | A list of payment errors for the Inkasso service.                                                                                                                 |

## AddPayment

Can be partial or full payment. Based on the provided amount.
If full then Claim marked as paid. Bank claim is cancelled
If partial then claim has repaid increased and bank claim is altered, cost is lowered or capital or both. Interest and charges are not touched.
If service returns Status In progress (0) then you need to check later using service GetAddPaymentResult.

### Input
| Type                   | Name          | Description                                                                                                                                                                                                                                                                                                                                                                    |
|------------------------|---------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| decimal                | BankClaimKey  | Account ID of the bank claim.                                                                                                                                                                                                                                                                                                                                                   |
| decimal                | amount        | The amount that was paid.                                                                                                                                                                                                                                                                                                                                                      |
| decimal                | splitCoef     | It is a number from [0,1]. Indicates the proportion of the payment that should be allocated to Inkasso. If the ratio cannot be fulfilled (for example, Inkasso or claimant part is too low to satisfy the percentage), it assigns the remaining money to the other side. There is one caveat: unconfirmed fees are assigned only if both confirmed fees and claimants part are already satisfied. Thus unconfirmed fees fall outside of the coefficient. Basically, you should have it as an editable variable on your end (part of setup) and have it set to 1. That means that the payment is distributed in the traditional way, first towards Annar vanskilakostnaður (AVAN), then to Annar kostnaður (AKOS), then to default interest, and then to capital. |
### Output
| Type                                      | Name                  | Description                                                                                                                                                        |
|-------------------------------------------|-----------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| long                                      | Id                    | The identifier for the operation.                                                                                                                                 |
| OperationStatus                           | Status                | The status of the operation.                                                                                                                                      |
| string                                    | Error                 | An error message associated with the operation.                                                                                                                   |
| List\<InkassoServicePaymentSuccess\>       | Success               | A list of successful payment information for the Inkasso service.                                                                                                 |
| List\<InkassoServicePaymentError\>         | Errors                | A list of payment errors for the Inkasso service.                                                                                                                 |

## GetAddPaymentResult
Service to get respond for AddFinalPayment and AddPayment
### Input
| Type | Name | Description |
|------|------|-------------|
| long | Id   | The identifier.|

### Output
| Type             | Name   | Description                 |
|------------------|--------|-----------------------------|
| long             | Id     | The identifier.             |
| OperationStatus  | Status | The status of the operation. |
| string           | Error  | An error message associated with the operation. |
| List\<InkassoServicePaymentSuccess\> | Success | A list of successful payment information for the Inkasso service. |
| List\<InkassoServicePaymentError\>   | Errors  | A list of payment errors for the Inkasso service. |

## GetFinalPayments
Gets a list of claims that got final payment during period given. Entry to and from is for batching. 0 based. In response there is a count of total entries for the given period.
### Input
| Type    | Name      | Description                                                      |
|---------|-----------|------------------------------------------------------------------|
| DateTime| DateFrom   | Inclusive. The starting date and time, e.g., 2015-08-07T00:00:00.|
| DateTime| DateTo     | Exclusive. The ending date and time, e.g., 2015-08-07T00:00:00.  |
| int     | EntryFrom  | 1-based. Inclusive, represents the starting entry.               |
| int     | EntryTo    | 1-based. Inclusive, represents the ending entry.                 |

### Output
| Type                             | Name     | Description                                       |
|----------------------------------|----------|---------------------------------------------------|
| List\<FinalPaymentRecord\>        | Payments | A list of FinalPaymentRecord items.               |
| int                              | TotalCount| The total count of payments in the list.          |

## GetPostponeDate

Query by bank claim id. Returns a date if claim has postponement or is in active payment plan. For active payment plan it returns date of final installment.
### Input
| Type       | Name      | Description                   |
|------------|-----------|-------------------------------|
| BankClaimKey| claimKey  | The key associated with the bank claim.|

### Output
| Type    | Name  | Description                          |
|---------|-------|--------------------------------------|
| DateTime| date  | Represents the date and time, e.g., 2015-08-07T00:00:00.|


## QueryPaymentPlans

### Input
| Type                | Name        | Description                                                      |
|---------------------|-------------|------------------------------------------------------------------|
| string              | ClaimantId  | Identifier associated with the claimant.                          |
| PaymentPlanStatus   | Status      | The status of the payment plan.                                   |
| DateTime            | DateFrom    | Inclusive. The starting date and time, e.g., 2015-08-07T00:00:00.|
| DateTime            | DateTo      | Inclusive. The ending date and time, e.g., 2015-08-07T00:00:00.  |
| int                 | EntryFrom   | 1-based. Inclusive, represents the starting entry.               |
| int                 | EntryTo     | 1-based. Inclusive, represents the ending entry.                 |

### Output
| Type                    | Name           | Description                                      |
|-------------------------|----------------|--------------------------------------------------|
| List\<PaymentPlanWS\>    | PaymentPlans   | A list of PaymentPlanWS items.                   |
| int                     | Count          | The count of payment plans in the list.          |


## SetPostponementDate

Set a postpone date on a claim

### Input
| Type             | Name            | Description                                                                         |
|------------------|-----------------|-------------------------------------------------------------------------------------|
| BankClaimKey     | ClaimKey        | Key of the bank claim to postpone.                                                   |
| string           | ClaimantId      | Kennitala of claimant.                                                              |
| DateTime?        | PostponeDate    | Date to postpone the claim to. Null will clear the postponement.                     |
|                  |                 | Contact information includes the company address, phone number, email, and website. Note: Does not affect payment plans. |

### Output
No output.

## QueryPostponeDate
Returns all claims that have active postponement.

### Input
No input.

### Output [Array]
| Type         | Name         | Description                               |
|--------------|--------------|-------------------------------------------|
| BankClaimKey | ClaimKey     | Key of the bank claim.                     |
| DateTime     | PostponeDate | Date until it's postponed.                 |

## StopCollection
Collection of claim is stopped permanently until StartCollection is called to reactive collection.

### Input
| Type         | Name       | Description                                       |
|--------------|------------|---------------------------------------------------|
| BankClaimKey | ClaimKey   | Key of the bank claim to stop collecting.         |

### Output
No output.

## StartCollection
Manually start collection on a claim that has a) been stopped, see StopCollection or b) is on a collection config that does not start collection automatically.

### Input
| Type         | Name       | Description                                       |
|--------------|------------|---------------------------------------------------|
| BankClaimKey | ClaimKey   | Key of the bank claim to start collecting.        |

### Output
No output.

## ReturnClaim
Collection of claim is stopped, collection cost is removed and responsibility of claim is returned to claimant. Claim gets status Returned (IS: Skilað) which is a final state of a claim. Collection of claim cannot be restarted through web service or web portal.

### Input
| Type         | Name       | Description                                       |
|--------------|------------|---------------------------------------------------|
| BankClaimKey | ClaimKey   | Key of the bank claim to start collecting.        |

### Output
| Type                                | Name   | Description                                       |
|-------------------------------------|--------|---------------------------------------------------|
| InkassoServiceReturnClaimResult     | Result | The result of the Inkasso service return claim.   |

## GetReturnClaimResult

### Input
| Type | Name | Description      |
|------|------|------------------|
| long | Id   | Identifier value.|

### Output
| Type                             | Name   | Description                                       |
|----------------------------------|--------|---------------------------------------------------|
| InkassoServiceReturnClaimResult  | Result | The result of the Inkasso service return claim.   |

## AddCommentToClaim
Add a comment to a claim

### Input
| Type         | Name      | Description                              |
|--------------|-----------|------------------------------------------|
| BankClaimKey | ClaimKey  | Key of the bank claim.                    |
| string       | Comment   | Comment to add to the claim.              |

### Output
No output.

## AddCommentToClient
Add a comment to a client
| Type   | Name       | Description                              |
|--------|------------|------------------------------------------|
| string | ClaimentId  | Registry id of the claimant.              |
| string | clientId    | Registry id of the client.                |
| string | Comment     | Comment to add to the claim.              |

## GetClaimHistory
Returns the claim history of a claim. Same information that is displayed on claim object in Claim history tab in INK web.
Example: https://demo.inkasso.is/8/Admin/EditClaim/22715
![image](https://github.com/Inkasso-is/Inkasso-service-code-example/assets/7527311/7e0855fe-7a05-49e3-b400-1def0ce9a1bc)

### Input
| Type         | Name      | Description                              |
|--------------|-----------|------------------------------------------|
| BankClaimKey | ClaimKey  | Key of the bank claim to postpone.        |

### Output[Array]
| Type         | Name            | Description                                       |
|--------------|-----------------|---------------------------------------------------|
| string       | Status          | Status of the claim.                              |
| string       | CollectingConfig | Name of the collection config.                    |
| DateTime     | FinalDueDate     | Final due date of the claim.                      |
| Amount       | Capital          | Capital of the claim.                             |
| Amount       | PenaltyInterest  | Penalty interest amount.                          |
| Amount       | OtherCost        | Other cost amount.                                |
| Amount       | OtherDefaultCost | Other default cost amount.                         |
| Amount       | Repaid           | Repaid amount.                                   |
| Amount       | TotalAmount      | Total amount to be paid (Remaining amount).      |
| string       | Action           | Description of the action.                        |
| string       | Username         | User performing the action.                      |
| DateTime     | ExpiryDate       | Expiry date of the claim.                         |
| DateTime     | Created          | When the history entry was performed.            |


## SendClaimDocument
Add a document to a claim. Document is stored in INK system and can be viewed on claim object in tab Attachments.

### Input
| Type         | Name        | Description                                               |
|--------------|-------------|-----------------------------------------------------------|
| BankClaimKey | ClaimKey    | Key of the bank claim to postpone.                        |
| string       | FileContent | Content of the file as a Base64 encoded string (up to 2MB).|
| string       | FileName    | Name of the file with extension.                           |

### Output

| Type | Name   | Description                                           |
|------|--------|-------------------------------------------------------|
| bool | Result | Boolean indicating the result (true if successful).   |
| string | Error | If failed, could contain an error description.        |

## Complex objects

### BankClaimKey
| Type   | Name       | Description                                                                        |
|--------|------------|------------------------------------------------------------------------------------|
| string | Account    | Bank claim number. Format: \|bank branch, 4 digits\|66\|claim number, 6 digits\| Example: 110266012345. Bank branch and claim number are padded with 0. Total length of string is always 12 digits.|
| string | ClaimantId | Claimant Kennitala.                                                                |
| DateTime | DueDate   | Claim due date. Example: 2015-08-07T00:00:00                                       |


### PaymentPlanRequestResult
| Type   | Name       | Description                                           |
|--------|------------|-------------------------------------------------------|
| string | ClaimantId | Identifier of the claimant.                           |
| PaymentPlanStatus | PaymentPlanStatus | Status of the payment plan. |
| Status | Status     | Status of the payment plan.                            |
| DateTime | DateFrom  | Inclusive start date of the payment plan. Example: 2015-08-07T00:00:00 |
| DateTime | DateTo    | Exclusive end date of the payment plan. Example: 2015-08-07T00:00:00 |
| int    | EntryFrom  | 1-based inclusive entry from position in the payment plan. |
| int    | EntryTo    | 1-based inclusive entry to position in the payment plan.   |

### FinalPaymentRecord
| Type         | Name            | Description                                         |
|--------------|-----------------|-----------------------------------------------------|
| BankClaimKey | Key             | Key of the bank claim.                              |
| DateTime     | TransactionDate | Date and time of the transaction. Example: 2015-08-07T00:00:00 |
| DateTime     | Created         | Date and time when the entry was created. Example: 2015-08-07T00:00:00 |
| string       | Reference       | Reference string associated with the entry.         |

### PaymentPlanWS
| Type              | Name              | Description                                              |
|-------------------|-------------------|----------------------------------------------------------|
| long              | Id                | Identifier of the payment plan.                           |
| string            | PayorId           | Identifier of the payor.                                  |
| PaymentPlanStatus | Status            | Status of the payment plan.                                |
| DateTime          | Created           | Date and time when the payment plan was created. Example: 2015-08-07T00:00:00 |
| DateTime          | Cancelled         | Date and time when the payment plan was cancelled. Example: 2015-08-07T00:00:00 |
| List<InstallmentWS> | Installments     | List of installment objects associated with the payment plan. |
| List<PlanClaimWS> | Claims             | List of plan claim objects associated with the payment plan.  |
| List<PaymentWS>   | Payments           | List of payment objects associated with the payment plan.    |

### InstallmentWS
| Type           | Name      | Description                                              |
|----------------|-----------|----------------------------------------------------------|
| long           | Id        | Identifier of the payment plan item.                     |
| DateTime       | Date      | Date associated with the payment plan item. Example: 2015-08-07T00:00:00 |
| decimal        | Amount    | Amount associated with the payment plan item.            |
| bool           | SendBill  | Boolean indicating whether a bill should be sent for the payment plan item. |
| PaymentPlanItemStatus | Status | Status of the payment plan item.                          |

### PlanClaimWS
| Type       | Name         | Description                                              |
|------------|--------------|----------------------------------------------------------|
| long       | Id           | Identifier of the claim.                                 |
| string     | Account      | Bank claim number. Format: |bank branch, 4 digits|66|claim number, 6 digits| Example: 110266012345 (padded with 0, total length is always 12 digits) |
| DateTime   | DueDate      | Due date of the claim. Example: 2015-08-07T00:00:00       |
| DateTime   | FinalDueDate | Final due date of the claim. Example: 2015-08-07T00:00:00 |
| decimal    | Amount       | Amount associated with the claim.                        |
| ClaimStatus| Status       | Status of the claim.                                      |
| string     | Reference    | Reference string associated with the claim.             |

### PaymentWS
| Type   | Name               | Description                                          |
|--------|--------------------|------------------------------------------------------|
| decimal| PaidAmount         | Amount paid for the installment.                     |
| long   | InstallmentId      | Identifier of the installment.                       |
| DateTime| TransactionDate   | Date and time of the transaction. Example: 2015-08-07T00:00:00 |
| decimal| CapitalInterestTax | Amount representing capital interest tax for the installment. |

### InkassoServicePaymentSuccess
| Type              | Name                       | Description                                            |
|-------------------|----------------------------|--------------------------------------------------------|
| BankClaimKey      | Key                        | Key of the bank claim associated with the payment.     |
| PaymentAmountData | RepaidAmounts              | Object containing information about repaid amounts.    |
| decimal           | CapitalGainsTaxToBePaid    | Amount representing capital gains tax to be paid for this payment. |

### InkassoServicePaymentError
| Type                        | Name                     | Description                                            |
|-----------------------------|--------------------------|--------------------------------------------------------|
| BankClaimKey                | Key                      | Key of the bank claim associated with the error.       |
| decimal                     | RequiredPaymentAmountLow | Required low payment amount associated with the error. |
| Nullable decimal            | RequiredPaymentAmountHigh| Nullable required high payment amount associated with the error. |
| InkassoServiceErrorCode     | ErrorCode                | Error code from the Inkasso service.                    |
| string                      | Error                    | Description of the error.                               |

### PaymentAmountData
| Type   | Name                    | Description                                  |
|--------|-------------------------|----------------------------------------------|
| decimal| CapitalRepaid            | Amount repaid towards the capital.           |
| decimal| OtherDefaultCostRepaid   | Amount repaid towards other default costs.  |
| decimal| OtherCostRepaid          | Amount repaid towards other costs.          |
| decimal| DefaultInterestRepaid    | Amount repaid towards default interest.     |
| decimal| NoticeChargeRepaid       | Amount repaid towards notice charges.       |
| decimal| DefaultChargeRepaid      | Amount repaid towards default charges.      |

### InkassoServiceReturnClaimResult
| Type                       | Name    | Description                                            |
|----------------------------|---------|--------------------------------------------------------|
| Long                       | id      | Identifier associated with the operation.              |
| OperationStatus            | Status  | Status of the operation.                               |
| String                     | Error   | Description of the error if the operation failed.     |
| List\<BankClaimKey\>        | Success | List of BankClaimKey objects representing successful operations. |
| InkassoServiceClaimError   | Errors  | Object representing errors associated with the operation. |

### InkassoServiceClaimError
| Type           | Name    | Description                                   |
|----------------|---------|-----------------------------------------------|
| BankClaimKey   | Key     | Key of the bank claim associated with the error. |
| String         | Error   | Description of the error.                     |

## Enums

### PaymentPlanItemStatus
| #  | Name       | Description                               |
|----|------------|-------------------------------------------|
| 0  | Pending    | Before bank claim is created              |
| 1  | Unpaid     | Bank claim created but not paid            |
| 2  | Paid       | Bank claim paid                            |
| 3  | Cancelled  | Bank claim cancelled                       |

### ClaimStatus
| #  | Name                   | Description                               |
|----|------------------------|-------------------------------------------|
| 0  | PrimaryCollection      |                                           |
| 1  | Paid                   |                                           |
| 2  | Cancelled              |                                           |
| 3  | Collection             |                                           |
| 4  | LegalCollection        |                                           |
| 5  | Returned               |                                           |
| 6  | LegalProceedings        |                                          |
| 7  | ClaimMonitoring        |                                          |

### OperationStatus
| #  | Name               | Description                                      |
|----|--------------------|--------------------------------------------------|
| 0  | InProgress         | If operation is not finished, then it has this status. |
| 1  | Completed          | Success                                          |
| 2  | CompletedWithErrors | Unsuccessful operation. Failure of at least 1 request of the group sent. |
| 3  | NonConfirmed       | Not used                                         |
| 4  | Cancelled           | Not used                                         |
| 5  | OnHold             | Not used                                         |
| 6  | Unknown            | Not used                                         |

### InkassoServiceErrorCode
| #  | Name                                    | Description                                                                                                               |
|----|-----------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| 1  | InvalidRegistryId                       | For some reason, we could not create a debtor and client in our system. Currently, the only realistic reason is an invalid kennitala. (Only returned from CreateClaim(s).) |
| 2  | ClaimAlreadyExists                      | Returned when a claim requested to be created already exists in the Inkasso system (duplicate claims in the same request also return this). (Only returned from CreateClaim(s).) |
| 3  | ClaimantNotSupported                    | Returned either when the claimant of the claim from the request does not exist in the Inkasso system or is inactive |
| 4  | AccessDenied                            | The user that sent the request does not have access to the claim's claimant |
| 5  | OperationTerminated                     | Operation was terminated on the side of Inkasso. Reasons may vary. |
| 6  | BankOperationFailure                    | Operation failed in the bank. Reasons may vary. More detailed reason returned by the bank should be in the error description field |
| 7  | ClaimDoesNotExist                       | Bank claim that the operation was requested on does not exist in the Inkasso system. |
| 8  | ClaimIsNotActive                        | Bank claim that the operation was requested on is inactive in the Inkasso system, but needs to be active for the operation to succeed |
| 9  | ClaimPartiallyPaid                      | Bank claim that the operation was requested on is partially paid either in the bank or Inkasso, and the operation is not supported on partially paid bank claims. Currently only relevant for AlterClaim(s). |
| 10 | PayorIdDiffers                           | Currently only relevant for AlterClaim(s). We do not support changing payor id with alters. |
| 11 | InvalidInputData                         | Catch-all error code for data validation errors. More detailed reasons should be in the error description field. |
| 12 | CompanyHasNoCollectionConfig             | Indicates a lack of collection config for the company representing the claimant of the requested claim. (Only returned from CreateClaim(s).) Currently only used in services other than IOBS, but will probably be introduced into IOBS as currently IOBS fails the whole request in such a scenario. |
| 13 | CannotFindPayment                       | Not used in IOBS, indicates an internal Inkasso system error |
| 14 | ClaimCannotBePaidWithSpecifiedParameters | Not used in IOBS |
| 15 | PaymentAmountNotExact                    | Not used in IOBS |
| 16 | OperationNotSupported                    | Catch-all error code for not supported scenarios. More detailed reasons should be in the error description field. Not used currently in IOBS |
| 17 | BankClaimWithActiveAlter                 | Returned when the operation requires the bank claim not to be in a state of active alter. Currently not used. |
| 18 | InvalidIdentifier                        | The identifier specified in the request is either not set up in the Inkasso system or there is inconsistent setup in the Inkasso system between the identifier and collection config |
| 19 | BankClaimNotAssociatedWithClaim          | Basically says that the request is for a payment plan bank claim, and the requested operation is not supported for those. Used currently only in claim ownership service calls. |
| 20 | OverridingCompanyIsNotSetupCorrectly     | Basically currently says that Faktoria is either not set up in the Inkasso system or does not have a payment account set up correctly. Used currently only in claim ownership service calls. |


## URL redirect service
A collection of simple URL redirect services that simplify URL linking from external system to Inkasso.
## Service Instances

| Instance | URL                               |
|----------|-----------------------------------|
| Live     | [https://secure.inkasso.is/go](https://secure.inkasso.is/go) |
| Demo     | [https://demo.inkasso.is/go](https://demo.inkasso.is/go)   |

### Redirect to client
User is redirected to client.
Instance/Client/<ClaimantRegistryId>/<DebtorRegistryId>
Example: https://secure.inkasso.is/go/Client/4604100450/1611784629
#### Input
## Registry IDs

| Name                 | Description                                     |
|----------------------|-------------------------------------------------|
| ClaimantRegistryId   | The kennitala of the claimant. 10 digits and no hyphen (-). Example: 1234567890 |
| DebtorRegistryId     | The kennitala of the debtor. 10 digits and no hyphen (-). Example: 1234567890 |

### Redirect to claim
User is redirected to claim based on bank claim information.
Instance/ClaimBC/<ClaimantRegistryId>/<AccountNumber>/<Duedate>
Example: https://secure.inkasso.is/go/ClaimBC/4604100450/080066221033/2016-07-20

#### Input
## Registry IDs and Account Number

| Name                 | Description                                     |
|----------------------|-------------------------------------------------|
| ClaimantRegistryId   | The kennitala of the claimant. 10 digits and no hyphen (-). Example: 1234567890 |
| AccountNumber        | 12 digits Account Number                        |
| DueDate              | Date in yyyy-MM-dd format for Due Date          |
