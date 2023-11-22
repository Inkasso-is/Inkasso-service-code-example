using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Security;
using System.Net;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace InkassoServiceTest {
    [TestClass]
    public class InkassoIOBSService {

        public string UserName = "test.inkasso";
        public string Password = "$ILove2Code";

        [TestMethod]
        public void CreateClaimSimple() {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // account: 0900-66-000006
            string Account = "090066000007";
            string ClaimantID = "0101307789";
            string PayorID = "0101305069";
            string Identifier = "0TE";
            string Reference = Account.PadLeft(6);
            string BillNumber = Account.PadLeft(6);
            Decimal Amount = 500;
            string CustomerNumber = "0101305069";
            DateTime DueDate = DateTime.Now.AddDays(5);
            DateTime FinalDueDate = DueDate.AddDays(25);
            DateTime CancellationDate = DueDate.AddYears(4);

            var client = new IOBS_serv_DEMO.IcelandicOnlineBankingClaimsSoapClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var claim = new IOBS_serv_DEMO.Claim() {
                Key = new IOBS_serv_DEMO.ClaimKey {
                    Account = Account,
                    ClaimantID = ClaimantID,
                    DueDate = DueDate
                },
                PayorID = PayorID,
                CancellationDate = CancellationDate,
                Identifier = Identifier,
                Amount = Amount,
                Reference = Reference,
                FinalDueDate = FinalDueDate,
                BillNumber = BillNumber,
                CustomerNumber = CustomerNumber,
                NoticeAndPaymentFee = new IOBS_serv_DEMO.NoticeAndPaymentFee {
                    Printing = 100,
                    Paperless = 100
                },
                DefaultCharge = null,
                OtherDefaultCosts = 0,
                DefaultInterest = null,
                CurrencyInformation = null,
                PermitOutOfSequencePayment = false
            };
            var result = client.CreateClaim(claim);

            var builder = new StringBuilder();
            var t = result.GetType();
            var pi = t.GetProperties();

            foreach (PropertyInfo p in pi) {
                builder.AppendLine(p.Name + " : " + p.GetValue(result));
            }
            Debug.WriteLine("Test Finished: " + Environment.NewLine + builder.ToString());
            Debugger.Break();
        }

        [TestMethod]
        public void CheckStatus() {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new IOBS_serv_DEMO.IcelandicOnlineBankingClaimsSoapClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var result = client.GetClaimOperationResult("5");


            var builder = new StringBuilder();
            var t = result.GetType();
            var pi = t.GetProperties();

            foreach (PropertyInfo p in pi) {
                builder.AppendLine(p.Name + " : " + p.GetValue(result));
            }
            Debug.WriteLine("Test Finished: " + Environment.NewLine + builder.ToString());
            Debugger.Break();
        }

        [TestMethod]
        public void QueryClaims() {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new IOBS_serv_DEMO.IcelandicOnlineBankingClaimsSoapClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            //var wsKey = new IOBS_serv_LIVE.ClaimKey()
            //{
            //    Account = accountTxt.Text,
            //    ClaimantID = claimantTxt.Text,
            //    DueDate = new DateTime(2012, 9, 20)
            //};

            //var wsKey = new IOBS_serv_DEMO.ClaimKey() {
            //    Account = accountTxt.Text,
            //    ClaimantID = claimantTxt.Text,
            //    DueDate = new DateTime(2012, 9, 20)
            //};

            var wsKey = new IOBS_serv_DEMO.ClaimKey() {
                Account = "090066100420",
                ClaimantID = "0101307789",
                DueDate = new DateTime(2023, 9, 10)
            };

            var result = client.QueryClaim(wsKey);

            var builder = new StringBuilder();
            var t = result.GetType();
            var pi = t.GetProperties();

            foreach (PropertyInfo p in pi) {
                builder.AppendLine(p.Name + " : " + p.GetValue(result));
            }
            Debug.WriteLine("Test Finished: " + Environment.NewLine + builder.ToString());
            Debugger.Break();
        }

        [TestMethod]
        public void QueryPayments() {

            string ClaimantID = "0101307789";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new IOBS_serv_DEMO.IcelandicOnlineBankingClaimsSoapClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var wsKey = new IOBS_serv_DEMO.PaymentsQuery {
                Claimant = ClaimantID,
                TransactionDateFrom = DateTime.Now.AddDays(-50),
                TransactionDateTo = DateTime.Now,
                EntryFromSpecified = false,
                EntryToSpecified = false,
                EntryFrom = 1,
                EntryTo = 50
            };

            var result = client.QueryPayments(wsKey);

            var builder = new StringBuilder();
            var t = result.GetType();
            var pi = t.GetProperties();

            foreach (PropertyInfo p in pi) {
                builder.AppendLine(p.Name + " : " + p.GetValue(result));
            }
            Debug.WriteLine("Test Finished: " + Environment.NewLine + builder.ToString());
            Debugger.Break();
        }

        [TestMethod]
        public void CreateClaimWithInvoice() {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // account: 0900-66-000006
            string Account = "090066000007";
            string ClaimantID = "0101307789";
            string PayorID = "0101305069";
            string Identifier = "0TE";
            string Reference = Account.PadLeft(6);
            string BillNumber = Account.PadLeft(6);
            Decimal Amount = 500;
            string CustomerNumber = "0101305069";
            DateTime DueDate = DateTime.Now.AddDays(5);
            DateTime FinalDueDate = DueDate.AddDays(25);
            DateTime CancellationDate = DueDate.AddYears(4);
            string StreetName = "Testvegur 1";

            var client = new IOBS_serv_DEMO.IcelandicOnlineBankingClaimsSoapClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var xmlInvoice = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>" +
                "<Invoice xmlns:cbc=\"urn: oasis: names: specification: ubl: schema: xsd: CommonBasicComponents - 2\" xmlns:cac=\"urn: oasis: names: specification: ubl: schema: xsd: CommonAggregateComponents - 2\" xmlns=\"urn: oasis: names: specification: ubl: schema: xsd: Invoice - 2\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                "<cbc:UBLVersionID>2.0</cbc:UBLVersionID>" +
                "<cbc:CustomizationID>NESUBL-2.0</cbc:CustomizationID>" +
                "<cbc:ProfileID schemeID=\"Profile\" schemeAgencyID=\"NES\">urn:www.nesubl.eu:profiles:profile4:ver2.0</cbc:ProfileID>" +
                $"<cbc:ID>RU2380782</cbc:ID><cbc:IssueDate>{DueDate.ToString("yyyy-MM-dd")}</cbc:IssueDate>" +
                "<cbc:InvoiceTypeCode listID=\"UN/ECE 1001 Subset\" listAgencyID=\"NES\">380</cbc:InvoiceTypeCode>" +
                "<cbc:DocumentCurrencyCode listID=\"ISO 4217 Alpha\" listAgencyID=\"6\">ISK</cbc:DocumentCurrencyCode>" +
                $"<cac:InvoicePeriod><cbc:StartDate>{DueDate.ToString("yyyy-MM-dd")}</cbc:StartDate><cbc:EndDate>{DueDate.ToString("yyyy-MM-dd")}</cbc:EndDate>" +
                "</cac:InvoicePeriod>" +
                $"<cac:AccountingSupplierParty><cac:Party><cbc:EndpointID schemeID=\"IS:KT\" schemeAgencyID=\"ZZZ\">{ClaimantID}</cbc:EndpointID>" +
                "<cac:PartyIdentification>" +
                $"<cbc:ID schemeID=\"IS:KT\" schemeAgencyID=\"ZZZ\">{ClaimantID}</cbc:ID>" +
                "</cac:PartyIdentification>" +
                "<cac:PartyName><cbc:Name>Kópavogsbær</cbc:Name>" +
                "</cac:PartyName><cac:PostalAddress>" +
                $"<cbc:ID schemeID=\"IS:KT\" schemeAgencyID=\"ZZZ\">{ClaimantID}</cbc:ID>" +
                "<cbc:AddressFormatCode listID=\"Address Format\" listAgencyID=\"NES\">StructuredLax</cbc:AddressFormatCode>" +
                "<cbc:StreetName>Laugarvegur 178</cbc:StreetName>" +
                "<cbc:CityName>Reykjavík</cbc:CityName><cbc:PostalZone>101</cbc:PostalZone>" +
                "<cac:Country>" +
                "<cbc:IdentificationCode listID=\"ISO3166-1\" listAgencyID=\"6\">IS</cbc:IdentificationCode>" +
                "</cac:Country></cac:PostalAddress>" +
                "<cac:PartyTaxScheme>" +
                "<cbc:CompanyID schemeID=\"IS:VSKNR\" schemeAgencyID=\"ZZZ\">0</cbc:CompanyID>" +
                "<cac:TaxScheme><cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID>" +
                "</cac:TaxScheme></cac:PartyTaxScheme>" +
                "<cac:PartyLegalEntity>" +
                $"<cbc:CompanyID schemeID=\"IS:KT\" schemeAgencyID=\"ZZZ\">{ClaimantID}</cbc:CompanyID>" +
                "</cac:PartyLegalEntity></cac:Party>" +
                "</cac:AccountingSupplierParty><cac:AccountingCustomerParty>" +
                "<cac:Party><cbc:EndpointID schemeID=\"IS:KT\" schemeAgencyID=\"ZZZ\">{PayorID}</cbc:EndpointID>" +
                $"<cac:PartyIdentification><cbc:ID schemeID=\"IS:KT\" schemeAgencyID=\"ZZZ\">{PayorID}</cbc:ID>" +
                "</cac:PartyIdentification><cac:PartyName><cbc:Name>Ólafur Karl Siggeirsson</cbc:Name>" +
                $"</cac:PartyName><cac:PostalAddress><cbc:ID schemeID=\"IS:KT\" schemeAgencyID=\"ZZZ\">{PayorID}</cbc:ID>" +
                "<cbc:AddressFormatCode listID=\"Address Format\" listAgencyID=\"NES\">StructuredLax</cbc:AddressFormatCode>" +
                "<cbc:StreetName>Laugarvegur 178</cbc:StreetName><cbc:CityName>Reykjavík</cbc:CityName>" +
                "<cbc:PostalZone>101</cbc:PostalZone></cac:PostalAddress><cac:PartyLegalEntity>" +
                $"<cbc:CompanyID schemeID=\"IS:KT\" schemeAgencyID=\"ZZZ\">{PayorID}</cbc:CompanyID>" +
                "</cac:PartyLegalEntity></cac:Party></cac:AccountingCustomerParty><cac:PaymentMeans>" +
                "<cbc:PaymentMeansCode>49</cbc:PaymentMeansCode>" +
                $"<cbc:PaymentDueDate>{DueDate.ToString("yyyy-MM-dd")}</cbc:PaymentDueDate>" +
                $"<cac:PayeeFinancialAccount><cbc:ID>{Reference}</cbc:ID>" +
                "<cbc:AccountTypeCode listID=\"Account Type\" listAgencyID=\"NES\">IS:66</cbc:AccountTypeCode>" +
                $"<cac:FinancialInstitutionBranch><cbc:ID>{Account.PadRight(4)}</cbc:ID>" +
                "</cac:FinancialInstitutionBranch></cac:PayeeFinancialAccount></cac:PaymentMeans>" +
                "<cac:TaxTotal><cbc:TaxAmount currencyID=\"ISK\">0</cbc:TaxAmount>" +
                "<cac:TaxSubtotal><cbc:TaxableAmount currencyID=\"ISK\">100</cbc:TaxableAmount>" +
                "<cbc:TaxAmount currencyID=\"ISK\">0</cbc:TaxAmount>" +
                "<cac:TaxCategory><cbc:ID schemeID=\"UN/ECE 5305\" schemeAgencyID=\"6\">E</cbc:ID><cbc:Percent>0</cbc:Percent>" +
                "<cac:TaxScheme><cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID></cac:TaxScheme></cac:TaxCategory>" +
                "</cac:TaxSubtotal></cac:TaxTotal><cac:LegalMonetaryTotal><cbc:LineExtensionAmount currencyID=\"ISK\">100</cbc:LineExtensionAmount>" +
                "<cbc:TaxExclusiveAmount currencyID=\"ISK\">100</cbc:TaxExclusiveAmount>" +
                "<cbc:TaxInclusiveAmount currencyID=\"ISK\">100</cbc:TaxInclusiveAmount>" +
                "<cbc:PayableAmount currencyID=\"ISK\">100</cbc:PayableAmount>" +
                "</cac:LegalMonetaryTotal><cac:InvoiceLine><cbc:ID>1</cbc:ID>" +
                "<cbc:InvoicedQuantity unitCode=\"C62\">4.5</cbc:InvoicedQuantity>" +
                "<cbc:LineExtensionAmount currencyID=\"ISK\">100</cbc:LineExtensionAmount>" +
                "<cac:TaxTotal><cbc:TaxAmount currencyID=\"ISK\">0</cbc:TaxAmount>" +
                "<cbc:TaxSubtotal><cbc:TaxableAmount currencyID=\"ISK\">100</cbc:TaxableAmount>" +
                "<cbc:TaxAmount currencyID=\"ISK\">0</cbc:TaxAmount>" +
                "<cac:TaxCategory><cbc:ID schemeID=\"UN/ECE 5305\" schemeAgencyID=\"6\">E</cbc:ID>" +
                "<cbc:Percent>0</cbc:Percent><cac:TaxScheme><cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID>" +
                "</cac:TaxScheme></cac:TaxCategory></cbc:TaxSubtotal></cac:TaxTotal>" +
                "<cac:Item><cbc:Name>Test reikningur Inkasso</cbc:Name>" +
                "<cac:SellersItemIdentification><cbc:ID>INK-01</cbc:ID></cac:SellersItemIdentification>" +
                "<cac:ClassifiedTaxCategory><cbc:ID schemeID=\"UN/ECE 5305\" schemeAgencyID=\"6\">E</cbc:ID>" +
                "<cac:TaxScheme><cbc:ID schemeID=\"UN/ECE 5153\" schemeAgencyID=\"6\">VAT</cbc:ID></cac:TaxScheme>" +
                "</cac:ClassifiedTaxCategory></cac:Item><cac:Price>" +
                "<cbc:PriceAmount currencyID=\"ISK\">5000</cbc:PriceAmount></cac:Price>" +
                "</cac:InvoiceLine></Invoice>";

            var claim = new IOBS_serv_DEMO.Claim() {
                Key = new IOBS_serv_DEMO.ClaimKey {
                    Account = Account,
                    ClaimantID = ClaimantID,
                    DueDate = DueDate
                },
                PayorID = PayorID,
                CancellationDate = CancellationDate,
                Identifier = Identifier,
                Amount = Amount,
                Reference = Reference,
                FinalDueDate = FinalDueDate,
                BillNumber = BillNumber,
                CustomerNumber = CustomerNumber,
                NoticeAndPaymentFee = new IOBS_serv_DEMO.NoticeAndPaymentFee {
                    Printing = 100,
                    Paperless = 100
                },
                DefaultCharge = null,
                OtherDefaultCosts = 0,
                DefaultInterest = null,
                CurrencyInformation = null,
                PermitOutOfSequencePayment = false,
                BillPresentmentSystem = new IOBS_serv_DEMO.BillPresentmentSystem { Type = null, Parameters = xmlInvoice }
            };

            var result = client.CreateClaim(claim);

            var builder = new StringBuilder();
            var t = result.GetType();
            var pi = t.GetProperties();

            foreach (PropertyInfo p in pi) {
                builder.AppendLine(p.Name + " : " + p.GetValue(result));
            }
            Debug.WriteLine("Test Finished: " + Environment.NewLine + builder.ToString());
            Debugger.Break();
        }
    }
}


