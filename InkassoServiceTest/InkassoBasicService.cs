using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Security;
using System.Net;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace InkassoServiceTest {
    [TestClass]
    public class InkassoBasicService {

        public string UserName = "test.inkasso";
        public string Password = "$ILove2Code";

        /// <summary>
        /// Description: This method tests the functionality of adding a comment to a claim.
        /// Input: BankClaimKey() {
        ///     Account = AccountNr,
        ///     ClaimantId = ClaimantID,
        ///     DueDate = DueDate
        /// }
        /// string Comment
        /// </summary>
        [TestMethod]
        public void AddCommentToclaim() {

            string Account = "090066000001";
            string ClaimantID = "0101307789";
            DateTime DueDate = new DateTime(2023,11,7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var claimKey = new BASIC_serv_DEMO.BankClaimKey() {
                
                Account = Account,
                ClaimantId = ClaimantID,
                DueDate = DueDate
            };

            client.AddCommentToClaim(claimKey,"Test comment");
        
            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }

        /// <summary>
        /// Description: Adds a comment to a claimant.
        /// Input: BankClaimKey() {
        ///     Account = AccountNr,
        ///     ClaimantId = ClaimantID,
        ///     DueDate = DueDate
        /// }
        /// Input string Comment
        /// </summary>
        [TestMethod]
        public void AddCommentToClaimant() {

            string Account = "090066000001";
            string ClaimantID = "0101307789";
            string ClientId= "0101305069";
            DateTime DueDate = new DateTime(2023, 11, 7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var claimKey = new BASIC_serv_DEMO.BankClaimKey() {

                Account = Account,
                ClaimantId = ClaimantID,
                DueDate = DueDate
            };

            client.AddCommentToClient(ClaimantID, ClientId, "Test comment");

            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }

        /// <summary>
        /// Description: Stops collection for this claim.
        /// Input: BankClaimKey() {
        ///     Account = AccountNr,
        ///     ClaimantId = ClaimantID,
        ///     DueDate = DueDate
        /// }
        /// </summary>
        [TestMethod]
        public void StopCollection() {

            string Account = "090066000001";
            string ClaimantID = "0101307789";
            DateTime DueDate = new DateTime(2023, 11, 7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var claimKey = new BASIC_serv_DEMO.BankClaimKey() {

                Account = Account,
                ClaimantId = ClaimantID,
                DueDate = DueDate
            };

            client.StopCollection(claimKey);

            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }

        /// <summary>
        /// Description: Starts collection for this claim.
        /// Input: BankClaimKey() {
        ///     Account = AccountNr,
        ///     ClaimantId = ClaimantID,
        ///     DueDate = DueDate
        /// }
        /// </summary>
        [TestMethod]
        public void StartCollection() {

            string Account = "090066000001";
            string ClaimantID = "0101307789";
            DateTime DueDate = new DateTime(2023, 11, 7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var claimKey = new BASIC_serv_DEMO.BankClaimKey() {

                Account = Account,
                ClaimantId = ClaimantID,
                DueDate = DueDate
            };

            client.StartCollection(claimKey);

            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }

        /// <summary>
        /// Description: returns this claim to bank.
        /// Input: BankClaimKey() {
        ///     Account = AccountNr,
        ///     ClaimantId = ClaimantID,
        ///     DueDate = DueDate
        /// }
        /// </summary>
        [TestMethod]
        public void ReturnClaim() {

            string Account = "090066000001";
            string ClaimantID = "0101307789";
            DateTime DueDate = new DateTime(2023, 11, 7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var claimKey = new BASIC_serv_DEMO.BankClaimKey() {

                Account = Account,
                ClaimantId = ClaimantID,
                DueDate = DueDate
            };

            client.ReturnClaim(claimKey);

            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }

        /// <summary>
        /// Description: Gets history information about claims.
        /// Input: BankClaimKey() {
        ///     Account = AccountNr,
        ///     ClaimantId = ClaimantID,
        ///     DueDate = DueDate
        /// }
        /// Output: ClaimHistoryItem[] {  
        /// new ClaimHistoryItem
        /// {
        ///     Event = "Claim Created",
        ///     Date = DateTime.Now,
        ///     User = "John Doe"
        /// },{
        ///     Event = "Claim Updated",
        ///     Date = DateTime.Now.AddDays(1),
        ///     User = "Jane Smith"
        ///}
        /// </summary>
        [TestMethod]
        public void GetClaimHistory() {

            string Account = "090066000001";
            string ClaimantID = "0101307789";
            DateTime DueDate = new DateTime(2023, 11, 7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var claimKey = new BASIC_serv_DEMO.BankClaimKey() {

                Account = Account,
                ClaimantId = ClaimantID,
                DueDate = DueDate
            };

            var result = client.GetClaimHistory(claimKey);

            var builder = new StringBuilder();
            var t = result.GetType();
            var pi = t.GetProperties();

            foreach (PropertyInfo p in pi) {
                builder.AppendLine(p.Name + " : " + p.GetValue(result));
            }
            Debug.WriteLine("Test Finished: " + Environment.NewLine + builder.ToString());
            Debugger.Break();
        }

        /// <summary>
        /// Description: Adds final payment to claim, sets claim status to payed.
        /// Input: BankClaimKey() {
        ///     Account = AccountNr,
        ///     ClaimantId = ClaimantID,
        ///     DueDate = DueDate
        /// }
        /// Decimal Amount
        /// Decimal Split
        /// Output: InkassoServiceAddPaymentResult[]
        /// {
        ///     PaymentId = 1,
        ///     Success = true,
        ///     Message = "Payment added successfully"
        /// },{
        ///     PaymentId = 2,
        ///     Success = false,
        ///     Message = "Failed to add payment"
        /// }
        /// </summary>
        [TestMethod]
        public void AddFinalPayment() {

            string Account = "090066000001";
            string ClaimantID = "0101307789";
            DateTime DueDate = new DateTime(2023, 11, 7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            var claimKey = new BASIC_serv_DEMO.BankClaimKey() {

                Account = Account,
                ClaimantId = ClaimantID,
                DueDate = DueDate
            };

            var result = client.AddFinalPayment(claimKey,500,100);

            var builder = new StringBuilder();
            var t = result.GetType();
            var pi = t.GetProperties();

            foreach (PropertyInfo p in pi) {
                builder.AppendLine(p.Name + " : " + p.GetValue(result));
            }
            Debug.WriteLine("Test Finished: " + Environment.NewLine + builder.ToString());
            Debugger.Break();
        }

        /// <summary>
        /// Description: Postpones claims, until set date.
        /// Input: BankClaimKey() {
        ///     Account = AccountNr,
        ///     ClaimantId = ClaimantID,
        ///     DueDate = DueDate
        /// }
        /// DateTime PostponeDate
        /// </summary>
        [TestMethod]
        public void SetPostponeDate() {

            string Account = "090066000001";
            string ClaimantID = "0101307789";
            DateTime DueDate = new DateTime(2023, 11, 7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);

            
            var claimKey = new BASIC_serv_DEMO.BankClaimKey() {

                Account = Account,
                ClaimantId = ClaimantID,
                DueDate = DueDate
            };


            var postPoneDate = new BASIC_serv_DEMO.SetPostponeDateRecord {PostponeDate= DateTime.Now.AddMonths(1),ClaimKey=claimKey,ExtensionData = null};

            client.SetPostponeDate(postPoneDate);

            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }
    }
}


