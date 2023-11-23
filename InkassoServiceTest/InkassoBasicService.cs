using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Security;
using System.Net;
using System.Reflection;
using System.Text;
using System.Diagnostics;

namespace InkassoServiceTest {

    /// <summary>
    /// Test class for the InkassoIBasicService.
    /// </summary>
    /// 
    [TestClass]
    public class InkassoBasicService {

        public string UserName = "test.inkasso";
        public string Password = "$ILove2Code";

        /// <summary>
        /// Tests the functionality of adding a comment to a claim.
        /// </summary>
        /// <param name="claimKey">
        /// The key to identify the claim, including Account, ClaimantId, and DueDate.
        /// </param>
        /// <param name="comment">The comment to be added to the claim.</param>        
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
        /// Adds a comment to a claimant.
        /// </summary>
        /// <param name="claimantID">The unique identifier for the claimant.</param>
        /// <param name="clientId">The unique identifier for the client.</param>
        /// <param name="comment">The comment to be added to the claimant.</param> 
        [TestMethod]
        public void AddCommentToClaimant() {

            string ClaimantID = "0101307789";
            string ClientId= "0101305069";
            DateTime DueDate = new DateTime(2023, 11, 7);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new BASIC_serv_DEMO.InkassoBasicServiceClient();

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(InkassoTools.ValidateServerCertificate);


            client.AddCommentToClient(ClaimantID, ClientId, "Test comment");

            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }

        /// <summary>
        /// Stops collection for a specific claim.
        /// </summary>
        /// <param name="claimKey">The key identifying the claim, including:
        /// - Account: The account number (AccountNr)
        /// - ClaimantId: The claimant's ID (ClaimantID)
        /// - DueDate: The due date of the claim (DueDate)
        /// </param>
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
        /// Starts collection for a specific claim.
        /// </summary>
        /// <param name="claimKey">The key identifying the claim, including:
        /// - Account: The account number (AccountNr)
        /// - ClaimantId: The claimant's ID (ClaimantID)
        /// - DueDate: The due date of the claim (DueDate)
        /// </param>
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
        /// Returns the specified claim to the bank.
        /// </summary>
        /// <param name="claimKey">The key identifying the claim, including:
        /// - Account: The account number (AccountNr)
        /// - ClaimantId: The claimant's ID (ClaimantID)
        /// - DueDate: The due date of the claim (DueDate)
        /// </param>
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
        /// Gets history information about claims.
        /// </summary>
        /// <param name="claimKey">The key identifying the claim, including:
        /// - Account: The account number (AccountNr)
        /// - ClaimantId: The claimant's ID (ClaimantID)
        /// - DueDate: The due date of the claim (DueDate)
        /// </param>
        /// <returns>
        /// An array of <see cref="ClaimHistoryItem"/> containing history events, including:
        /// - Event: A string describing the event (e.g., "Claim Created", "Claim Updated")
        /// - Date: The date and time of the event (DateTime)
        /// - User: The user associated with the event (string)
        /// </returns>
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
        /// Adds final payment to claim, sets claim status to paid.
        /// </summary>
        /// <param name="claimKey">The key identifying the claim, including:
        /// - Account: The account number (AccountNr)
        /// - ClaimantId: The claimant's ID (ClaimantID)
        /// - DueDate: The due date of the claim (DueDate)
        /// </param>
        /// <param name="amount">The amount of the final payment (Decimal)</param>
        /// <param name="split">The split amount (Decimal)</param>
        /// <returns>
        /// An array of <see cref="InkassoServiceAddPaymentResult"/> containing payment results, including:
        /// - PaymentId: The ID of the payment (int)
        /// - Success: A boolean indicating if the payment was successful (bool)
        /// - Message: A message describing the result of the payment (string)
        /// </returns>
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
        /// Postpones claims until a set date.
        /// </summary>
        /// <param name="claimKey">The key identifying the claim, including:
        /// - Account: The account number (AccountNr)
        /// - ClaimantId: The claimant's ID (ClaimantID)
        /// - DueDate: The current due date of the claim (DueDate)
        /// </param>
        /// <param name="postponeDate">The new due date to which the claim is postponed (DateTime)</param>
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


