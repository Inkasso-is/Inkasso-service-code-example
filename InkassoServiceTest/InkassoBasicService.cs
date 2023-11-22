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

            client.GetClaimHistory(claimKey);

            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }

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

            client.AddFinalPayment(claimKey,500,100);

            Debug.WriteLine("Test Finished: ");
            Debugger.Break();
        }

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


