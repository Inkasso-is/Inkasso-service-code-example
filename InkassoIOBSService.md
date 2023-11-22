# Inkasso IOBS Service
Code example for inkasso demo,

## Table of Contents

- [Description](#description)
- [Service Instances](#service-instances)
- [DEMO Info](#demo-info)
- [Hints and Tips](#hints-and-tips)
- [Bindings](#bindings)
- [Authentication](#authentication)
- [Ignored Fields](#ignored-fields)
- [Invoice Information](#invoice-information)
  - [Multiple Invoices – Single Claim](#multiple-invoices--single-claim)
  - [Info on NES and BII Standards](#info-on-nes-and-bii-standards)
- [Paging in Query Services](#paging-in-query-services)
- [Error Codes](#error-codes)


## Description

The Inkasso IOBS service is an implementation of the standard IOBS services that the banks provide. This documents explains how to use the Inkasso instance of the service and the requirements that go beyond what the banks provide. 

Each bank has its own documentation for their service: 

•	Arion banki - https://ws.b2b.is/services/  
•	Landsbanki - https://www.landsbankinn.is/fyrirtaeki/rafraenarlausnir/b2b/thjonustusidurb2b/sambankaskema/  
•	Íslandsbanki - https://www.islandsbanki.is/fyrirtaeki/netlausnir/vefthjonusta/  
• Sparisjóðir - http://www.spar.is/fyrirtaekjathjonusta/innheimtuthjonusta  

## Service instances 
|  Instance | URL  |  
|---|---|
|  Live | https://secure.inkasso.is/SOAP/IOBS/IcelandicOnlineBankingClaimsSoap.svc  |   
|  DEMO | https://demo-inkasso.azurewebsites.net/SOAP/IOBS/IcelandicOnlineBankingClaimsSoap.svc   |  

## DEMO info 

Inkasso provides a testing environment on DEMO but acceptance testing should be made using LIVE. 
Username for service is servicetest and password is znvwYV5  
 
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

## Hints and tips 

It’s important that the clock on the client that is calling IOBS WS is correct. Use http://time.is to make sure that client is not more than 5 minutes off. 
 
Claim cannot be altered by AlterClaim(s) call if there were partial payments added to the claim. Such call will result in error being returned to the caller. 

## Bindings 
Example of bindings. Make sure to change the default values on these parameters 
``` 
  MaxBufferPoolSize = 104857600 
  MaxReceivedMessageSize = 104857600 
  MaxStringContentLength = 128192   MaxArrayLength = 65000 
 
  var binding = new WSHttpBinding 
  { 
    CloseTimeout = new TimeSpan(0, 3, 0), 
    OpenTimeout = new TimeSpan(0, 3, 0), 
    ReceiveTimeout = new TimeSpan(0, 3, 0), 
    SendTimeout = new TimeSpan(0, 3, 0), 
    BypassProxyOnLocal = false, 
    TransactionFlow = false, 
    HostNameComparisonMode = HostNameComparisonMode.StrongWildcard, 
    MaxBufferPoolSize = 104857600, 
    MaxReceivedMessageSize = 104857600, 
    MessageEncoding = WSMessageEncoding.Text, 
    TextEncoding = Encoding.UTF8, 
    UseDefaultWebProxy = true, 
    AllowCookies = false, 
    ReaderQuotas = 
      { 
        MaxDepth = 32, 
        MaxStringContentLength = 128192, 
        MaxArrayLength = 65000, 
        MaxBytesPerRead = 4096,         MaxNameTableCharCount = 16384 
      }, 
    ReliableSession = 
      { 
        Ordered = true, 
        InactivityTimeout = new TimeSpan(0, 10, 0), 
        Enabled = false 
      }, 
    Security = 
      { 
        Mode = SecurityMode.TransportWithMessageCredential, 
        Transport = 
          { 
            ClientCredentialType = HttpClientCredentialType.None,             ProxyCredentialType = HttpProxyCredentialType.None, 
            Realm = "" 
          }, 
        Message = 
          { 
            ClientCredentialType = MessageCredentialType.UserName, 
            EstablishSecurityContext = true, 
            NegotiateServiceCredential = true, 
            AlgorithmSuite = SecurityAlgorithmSuite.Default 
          } 
      }   }; 

```
## Authentication 

Service is using username authentication. Below is included C# sample code illustrating authentication and calling of service methods: 
 ```
    string str = client.CreateClaims(claims);  
    IcelandicOnlineBankingClaimsSoapClient client =         new IcelandicOnlineBankingClaimsSoapClient(); 
     client.ClientCredentials.UserName.UserName = "username";     client.ClientCredentials.UserName.Password = "password"; 
 
    ...  
    string str = client.CreateClaims(claims); 
```

## Ignored fields 
```
In CreateClaim(s) and AlterClaim(s) methods we ignore following fields from the Claim class : 
 	Claim.BillPresentmentSystem.Type 
 	Claim.CurrencyInformation 
 	Claim.Discount 
 	Claim.Printing 
In method AlterClaim(s) following three additional fields are ignored: 
 	Claim.BillPresentmentSystem.Parameters 
 	Claim.OtherCosts 
 	Claim.OtherDefaultCosts 
```

## Invoice information  

System supports both NES and BII BasicInvoice standards. NES or BII document in the form of NES Basic Invoice (Profile 4) or BII Basic Invoice (Profile 4) should be passed into the CreateClaim(s) call using Claim.BillPresentmentSystem.Parameters field. Content should be base64 encoded.  
 
In AlterClaim(s) call this field is ignored. Apart from the fields described in the standard there are three new attributes on the <Invoice> tag: email, print and bankdocument. They can contain ‘true’ or ‘false’ and determine if the bill should be printed and how it should be delivered. They are optional and if not provided they are assumed to be ‘false’. If all attributes are ‘false’ then no bill will be sent (though a PDF will be generated in Inkasso backend). If all are ‘true’ then the bill will be sent by post, by email and displayed in home bank portal under ‘Rafræn skjöl’. 
 
For the email to be sent a valid email address must be in 
/Invoice/cac:AccountingCustomerParty/cac:Party/cac:Contact/cbc:ElectronicMail  
 
For the bank document to work then claimant must be set up for the service in Inkasso. 
 
Example of the XML structure: 
```
<Invoice xmlns=... email="false" print="true" bankdocument="true">         … 
    </Invoice> 
```

### Multiple invoices – single claim 

System supports sending multiple invoice when creating a single claim (IS: Safnreikningur).  
 
Simply concatenate all invoices (including credit notes) under a <root> element as shown in the following example: 
 ```
<?xml version="1.0" encoding="UTF-8"?> 
<root>
  <Invoice xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateCompon ents-2" xmlns="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" email="false" print="false" bankdocument="false"> 
    … 
    </Invoice>
      <Invoice xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateCompon ents-2" xmlns="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" email="false" print="false" bankdocument="false">
    … 
    </Invoice>
      <Invoice xmlns:cbc="urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" xmlns:cac="urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateCompon ents-2" xmlns="urn:oasis:names:specification:ubl:schema:xsd:Invoice-2" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" email="false" print="false" bankdocument="false"> 
    … 
    </Invoice>
</root> 
```
###  Info on NES and BII standards 
Reference to the standard of NES Basic Invoice (Profile 4): 
http://www.nesubl.eu/?page_id=8#profile3Table 
 
A detailed description of BII BasicInvoice and how it should be used in Iceland can be found on FUT website  http://stadlar.is/stadlastarf/fagstadlarad-iupplysingataekni/taekniforskriftir/rafraenar-taekniforskriftir.aspx  
 
BII validation tools 
•	http://midran.is/index.php/taeknilausnir/validex-profunarverkfaeri  
•	http://www.invinet.org/recursos/conformance/invoice-validation.html  
 
BII is currently the de facto eInvoice standard in Iceland. NES is its predecessor. 

## Paging in query services 
For QueryPayments and QueryClaims the results should be paged 1000 entries per page.  
 
If no value is provided for EntryFrom and EntryTo, then the system will default to a 1000 entry page. 
 
EntryFrom is inclusive and starts on 1. EntryTo is inclusive. 
 
It’s also a very good idea to include dateFrom and dateTo in order to minimize paging. Claimants that can expect a lot of results should always use date paging. 
 
Here is sample code for paging: 
 ```
foreach (var dateRange in fromDate.BatchRange(toDate)) 
    { 
     uint EntryFrom = 1;      uint EntryTo = BatchSize; 
      while (true) 
     { 
      logger.Debug($"Query Claims batch 
[{dateRange.DateFrom.ToShortDateString()}-
{dateRange.DateTo.ToShortDateString()}] {EntryFrom} - {EntryTo}");       sw.Reset();       sw.Start(); 
      var ret = client.QueryClaims(new InkassoIOBSClaims.ClaimsQuery() 
      { 
       EntryFrom = EntryFrom, 
       EntryFromSpecified = true, 
       EntryTo = EntryTo,        EntryToSpecified = true, 
 
       Period = new InkassoIOBSClaims.ClaimsQueryDateSpan 
       { 
        DateFrom = dateRange.DateFrom, 
        DateTo = dateRange.DateTo, 
        DateFromSpecified = true, 
        DateToSpecified = true, 
        DateSpanReferenceDate = 
InkassoIOBSClaims.DateSpanReferenceDate.CreationDate, 
        DateSpanReferenceDateSpecified = true 
       } 
      }); 
```
## Error codes 

| #  | Name                                     | Description                                                                                                               |
|----|------------------------------------------|---------------------------------------------------------------------------------------------------------------------------|
| 1  | InvalidRegistryId                        | For some reason, we could not create a debtor and client in our system. Currently, the only realistic reason is an invalid kennitala. (Only returned from CreateClaim(s).) |
| 2  | ClaimAlreadyExists                       | Returned when a claim requested to be created already exists in the Inkasso system (duplicate claims in the same request also return this). (Only returned from CreateClaim(s).) |
| 3  | ClaimantNotSupported                     | Returned either when the claimant of the claim from the request does not exist in the Inkasso system or is inactive |
| 4  | AccessDenied                             | The user that sent the request does not have access to the claim's claimant |
| 5  | OperationTerminated                      | Operation was terminated on the side of Inkasso. Reasons may vary. |
| 6  | BankOperationFailure                     | Operation failed in the bank. Reasons may vary. More detailed reason returned by the bank should be in the error description field |
| 7  | ClaimDoesNotExist                        | Bank claim that the operation was requested on does not exist in the Inkasso system. |
| 8  | ClaimIsNotActive                         | Bank claim that the operation was requested on is inactive in the Inkasso system, but needs to be active for the operation to succeed |
| 9  | ClaimPartiallyPaid                       | Bank claim that the operation was requested on is partially paid either in the bank or Inkasso, and the operation is not supported on partially paid bank claims. Currently only relevant for AlterClaim(s). |
| 10 | PayorIdDiffers                            | Currently only relevant for AlterClaim(s). We do not support changing payor id with alters. |
| 11 | InvalidInputData                          | Catch-all error code for data validation errors. More detailed reasons should be in the error description field. |
| 12 | CompanyHasNoCollectionConfig              | Indicates a lack of collection config for the company representing the claimant of the requested claim. (Only returned from CreateClaim(s).) Currently only used in services other than IOBS, but will probably be introduced into IOBS as currently IOBS fails the whole request in such a scenario. |
| 13 | CannotFindPayment                        | Not used in IOBS, indicates an internal Inkasso system error |
| 14 | ClaimCannotBePaidWithSpecifiedParameters | Not used in IOBS |
| 15 | PaymentAmountNotExact                     | Not used in IOBS |
| 16 | OperationNotSupported                     | Catch-all error code for not supported scenarios. More detailed reasons should be in the error description field. Not used currently in IOBS |
| 17 | BankClaimWithActiveAlter                  | Returned when the operation requires the bank claim not to be in a state of active alter. Currently not used. |
| 18 | InvalidIdentifier                         | The identifier specified in the request is either not set up in the Inkasso system or there is inconsistent setup in the Inkasso system between the identifier and collection config |
| 19 | BankClaimNotAssociatedWithClaim           | Basically says that the request is for a payment plan bank claim, and the requested operation is not supported for those. Used currently only in claim ownership service calls. |
| 20 | OverridingCompanyIsNotSetupCorrectly      | Basically currently says that Faktoria is either not set up in the Inkasso system or does not have a payment account set up correctly. Used currently only in claim ownership service calls. |

