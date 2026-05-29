using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using InkassoMiddleware.Models;

namespace InkassoMiddleware
{
    /// <summary>
    /// Raw SOAP client for Inkasso integration that bypasses WCF complexity
    /// </summary>
    public static class InkassoRawSoapClient
    {

        // To use the mock endpoint for testing, set the INKASSO_URL environment variable:
        // For local development: http://localhost:5284/mock-inkasso
        // For Docker: http://host.docker.internal:5284/mock-inkasso
        // Example: Environment.SetEnvironmentVariable("INKASSO_URL", "http://localhost:5284/mock-inkasso");
        private static readonly string SoapEndpoint = Environment.GetEnvironmentVariable("INKASSO_URL") ??
            "https://secure.inkasso.is/SOAP/IOBS/IcelandicOnlineBankingClaimsSoap.svc/NoSc";
        
        // JSON endpoint for direct mock data (bypasses SOAP parsing)
        private static readonly string JsonMockEndpoint = "http://localhost:5284/mock-inkasso-json";
        
        private const string QueryClaimsSoapAction = "http://IcelandicOnlineBanking/2005/12/01/Claims/IIcelandicOnlineBankingClaimsSoap/QueryClaims";
        private const string CreateClaimsSoapAction = "http://IcelandicOnlineBanking/2005/12/01/Claims/IIcelandicOnlineBankingClaimsSoap/CreateClaims";
        private const string GetClaimOperationResultSoapAction = "http://IcelandicOnlineBanking/2005/12/01/Claims/IIcelandicOnlineBankingClaimsSoap/GetClaimOperationResult";
        private static readonly string Username = Environment.GetEnvironmentVariable("INKASSO_USERNAME") ?? "";
        private static readonly string Password = Environment.GetEnvironmentVariable("INKASSO_PASSWORD") ?? "";

        /// <summary>
        /// Queries claims using raw SOAP 1.2 request
        /// </summary>
        /// <param name="claimantId">The claimant ID to query</param>
        /// <param name="fromDate">Start date for the query range</param>
        /// <param name="toDate">End date for the query range</param>
        /// <returns>Raw XML response from the SOAP service</returns>
        public static async Task<string> QueryClaimsAsync(string claimantId, DateTime fromDate, DateTime toDate)
        {
            // Check if we should use the JSON mock endpoint
            bool useMockJson = IsUsingMockEndpoint() && SoapEndpoint.Contains("mock-inkasso");
            
            if (useMockJson)
            {
                try
                {
                    Console.WriteLine("--- Using JSON Mock Endpoint ---");
                    Console.WriteLine($"Endpoint: {JsonMockEndpoint}");
                    using var httpClient = new HttpClient();
                    
                    // Create HTTP request for JSON endpoint
                    using var jsonRequest = new HttpRequestMessage(HttpMethod.Get, JsonMockEndpoint);
                    
                    // Send request
                    HttpResponseMessage jsonResponse = await httpClient.SendAsync(jsonRequest);
                    
                    // Get response content
                    string jsonResponseContent = await jsonResponse.Content.ReadAsStringAsync();
                    
                    // Log the response for debugging
                    Console.WriteLine("--- JSON Mock Response ---");
                    Console.WriteLine($"Status: {jsonResponse.StatusCode}");
                    Console.WriteLine(jsonResponseContent);
                    
                    // Convert JSON to SOAP format for compatibility
                    string soapResponse = ConvertJsonResponseToSoap(jsonResponseContent);
                    
                    // Log the converted SOAP response
                    Console.WriteLine("--- Converted SOAP Response ---");
                    Console.WriteLine(FormatXml(soapResponse));
                    
                    // Return converted SOAP response
                    return soapResponse;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--- Error using JSON mock endpoint ---");
                    Console.WriteLine($"Exception: {ex.GetType().Name}");
                    Console.WriteLine($"Message: {ex.Message}");
                    Console.WriteLine("Falling back to SOAP endpoint...");
                }
            }
            
            // Construct SOAP envelope with security header
            string soapEnvelope = ConstructQueryClaimsEnvelope(claimantId, fromDate, toDate);
            
            // Log the request for debugging
            Console.WriteLine("--- SOAP Request ---");
            Console.WriteLine(FormatXml(soapEnvelope));
            
            return await ExecuteSoapRequestAsync(soapEnvelope, QueryClaimsSoapAction);
        }

        /// <summary>
        /// Creates one or many claims using raw SOAP 1.2 request.
        /// </summary>
        public static async Task<string> CreateClaimsAsync(IEnumerable<ClaimBatchItem> claims)
        {
            var claimList = claims?.ToList() ?? new List<ClaimBatchItem>();
            if (claimList.Count == 0)
            {
                throw new ArgumentException("At least one claim must be provided.", nameof(claims));
            }

            string soapEnvelope = ConstructCreateClaimsEnvelope(claimList);

            Console.WriteLine("--- SOAP Request ---");
            Console.WriteLine(FormatXml(soapEnvelope));

            return await ExecuteSoapRequestAsync(soapEnvelope, CreateClaimsSoapAction);
        }
        

        /// <summary>
        /// Retrieves the status/result of a previously submitted claim operation.
        /// </summary>
        public static async Task<string> GetClaimOperationResultAsync(string operationId)
        {
            if (string.IsNullOrWhiteSpace(operationId))
            {
                throw new ArgumentException("Operation ID must be provided.", nameof(operationId));
            }

            string soapEnvelope = ConstructGetClaimOperationResultEnvelope(operationId);

            Console.WriteLine("--- SOAP Request ---");
            Console.WriteLine(FormatXml(soapEnvelope));

            return await ExecuteSoapRequestAsync(soapEnvelope, GetClaimOperationResultSoapAction);
        }

        /// <summary>
        /// Converts a JSON response to SOAP format for compatibility
        /// </summary>
        /// <param name="jsonResponse">The JSON response from the mock endpoint</param>
        /// <returns>A SOAP formatted response</returns>
        private static string ConvertJsonResponseToSoap(string jsonResponse)
        {
            // Create a SOAP envelope with the mock data
            StringBuilder soapBuilder = new StringBuilder();
            soapBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            soapBuilder.AppendLine("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\"");
            soapBuilder.AppendLine("               xmlns:iobs=\"http://IcelandicOnlineBanking/2005/12/01/Claims\">");
            soapBuilder.AppendLine("  <soap:Body>");
            soapBuilder.AppendLine("    <iobs:QueryClaimsResponse>");
            soapBuilder.AppendLine("      <iobs:QueryClaimsResult>");
            
            try
            {
                // Parse the JSON response - use a case-insensitive option to handle property name casing
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                // Try to deserialize directly
                var claims = System.Text.Json.JsonSerializer.Deserialize<List<ClaimResult>>(jsonResponse, options);
                
                // Add each claim to the SOAP response
                if (claims != null)
                {
                    foreach (var claim in claims)
                    {
                        Console.WriteLine($"Converting claim: {claim.ClaimId}, {claim.Reference}, {claim.DueDate}, {claim.Capital}");
                        
                        soapBuilder.AppendLine("        <iobs:Claim>");
                        soapBuilder.AppendLine($"          <iobs:ClaimId>{claim.ClaimId}</iobs:ClaimId>");
                        soapBuilder.AppendLine($"          <iobs:Reference>{claim.Reference}</iobs:Reference>");
                        soapBuilder.AppendLine($"          <iobs:DueDate>{claim.DueDate:yyyy-MM-dd}</iobs:DueDate>");
                        soapBuilder.AppendLine($"          <iobs:Capital>{claim.Capital}</iobs:Capital>");
                        soapBuilder.AppendLine("        </iobs:Claim>");
                    }
                }
                else
                {
                    Console.WriteLine("No claims were deserialized from JSON");
                    
                    // Try to parse manually as a fallback
                    Console.WriteLine("Attempting manual JSON parsing...");
                    
                    // Use System.Text.Json to parse the JSON manually
                    using (var document = System.Text.Json.JsonDocument.Parse(jsonResponse))
                    {
                        var root = document.RootElement;
                        
                        // Check if it's an array
                        if (root.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            foreach (var element in root.EnumerateArray())
                            {
                                string claimId = "";
                                string reference = "";
                                string dueDate = DateTime.Now.ToString("yyyy-MM-dd");
                                string capital = "0";
                                
                                if (element.TryGetProperty("claimId", out var claimIdElement))
                                {
                                    claimId = claimIdElement.GetString() ?? "";
                                }
                                
                                if (element.TryGetProperty("reference", out var referenceElement))
                                {
                                    reference = referenceElement.GetString() ?? "";
                                }
                                
                                if (element.TryGetProperty("dueDate", out var dueDateElement))
                                {
                                    if (dueDateElement.ValueKind == System.Text.Json.JsonValueKind.String)
                                    {
                                        var dateString = dueDateElement.GetString();
                                        if (DateTime.TryParse(dateString, out var date))
                                        {
                                            dueDate = date.ToString("yyyy-MM-dd");
                                        }
                                    }
                                }
                                
                                if (element.TryGetProperty("capital", out var capitalElement))
                                {
                                    if (capitalElement.ValueKind == System.Text.Json.JsonValueKind.Number)
                                    {
                                        capital = capitalElement.GetDecimal().ToString();
                                    }
                                }
                                
                                Console.WriteLine($"Manually parsed claim: {claimId}, {reference}, {dueDate}, {capital}");
                                
                                soapBuilder.AppendLine("        <iobs:Claim>");
                                soapBuilder.AppendLine($"          <iobs:ClaimId>{claimId}</iobs:ClaimId>");
                                soapBuilder.AppendLine($"          <iobs:Reference>{reference}</iobs:Reference>");
                                soapBuilder.AppendLine($"          <iobs:DueDate>{dueDate}</iobs:DueDate>");
                                soapBuilder.AppendLine($"          <iobs:Capital>{capital}</iobs:Capital>");
                                soapBuilder.AppendLine("        </iobs:Claim>");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting JSON to SOAP: {ex.Message}");
                Console.WriteLine($"JSON content: {jsonResponse}");
                
                // Add a default claim if JSON parsing fails
                soapBuilder.AppendLine("        <iobs:Claim>");
                soapBuilder.AppendLine("          <iobs:ClaimId>ERROR</iobs:ClaimId>");
                soapBuilder.AppendLine("          <iobs:Reference>Error parsing JSON</iobs:Reference>");
                soapBuilder.AppendLine("          <iobs:DueDate>2024-01-01</iobs:DueDate>");
                soapBuilder.AppendLine("          <iobs:Capital>0</iobs:Capital>");
                soapBuilder.AppendLine("        </iobs:Claim>");
            }
            
            soapBuilder.AppendLine("      </iobs:QueryClaimsResult>");
            soapBuilder.AppendLine("    </iobs:QueryClaimsResponse>");
            soapBuilder.AppendLine("  </soap:Body>");
            soapBuilder.AppendLine("</soap:Envelope>");
            
            return soapBuilder.ToString();
        }

        /// <summary>
        /// Constructs a SOAP 1.2 envelope with security header and query body
        /// </summary>
        private static string ConstructQueryClaimsEnvelope(string claimantId, DateTime fromDate, DateTime toDate)
        {
            // Instead of using XmlDocument, let's use a simple string builder approach
            // This avoids issues with XML namespaces and attributes
            var sb = new StringBuilder();
            
            // Create unique IDs for the security elements
            string messageId = "urn:uuid:" + Guid.NewGuid().ToString();
            string tokenId = "UsernameToken-1";
            string timestampId = "TS-1";
            
            // Format timestamps
            string createdTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string expiresTime = DateTime.UtcNow.AddMinutes(10).ToString("yyyy-MM-ddTHH:mm:ssZ");
            
            // Build the SOAP envelope as a string
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\"");
            sb.AppendLine("               xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"");
            sb.AppendLine("               xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"");
            sb.AppendLine("               xmlns:wsa=\"http://www.w3.org/2005/08/addressing\"");
            sb.AppendLine("               xmlns:iob=\"http://IcelandicOnlineBanking/2005/12/01/Claims\">");
            
            // Header with WS-Addressing and Security
            sb.AppendLine("  <soap:Header>");
            
            // WS-Addressing headers
            sb.AppendLine($"    <wsa:Action soap:mustUnderstand=\"1\">{QueryClaimsSoapAction}</wsa:Action>");
            sb.AppendLine($"    <wsa:MessageID>{messageId}</wsa:MessageID>");
            sb.AppendLine($"    <wsa:To soap:mustUnderstand=\"1\">{SoapEndpoint}</wsa:To>");
            
            // WS-Security
            sb.AppendLine("    <wsse:Security soap:mustUnderstand=\"1\">");
            
            // Timestamp first
            sb.AppendLine($"      <wsu:Timestamp wsu:Id=\"{timestampId}\">");
            sb.AppendLine($"        <wsu:Created>{createdTime}</wsu:Created>");
            sb.AppendLine($"        <wsu:Expires>{expiresTime}</wsu:Expires>");
            sb.AppendLine("      </wsu:Timestamp>");
            
            // Username token
            sb.AppendLine($"      <wsse:UsernameToken wsu:Id=\"{tokenId}\">");
            sb.AppendLine($"        <wsse:Username>{Username}</wsse:Username>");
            sb.AppendLine($"        <wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{Password}</wsse:Password>");
            sb.AppendLine($"        <wsu:Created>{createdTime}</wsu:Created>");
            sb.AppendLine("      </wsse:UsernameToken>");
            
            sb.AppendLine("    </wsse:Security>");
            sb.AppendLine("  </soap:Header>");
            
            // Body with query
            sb.AppendLine("  <soap:Body>");
            sb.AppendLine("    <iob:QueryClaims>");
            sb.AppendLine("      <iob:query>");
            sb.AppendLine("        <iob:EntryFrom>1</iob:EntryFrom>");
            sb.AppendLine("        <iob:EntryTo>10</iob:EntryTo>");
            sb.AppendLine($"        <iob:Claimant>{claimantId}</iob:Claimant>");
            sb.AppendLine("        <iob:Period>");
            sb.AppendLine($"          <iob:DateFrom>{fromDate:yyyy-MM-dd}</iob:DateFrom>");
            sb.AppendLine($"          <iob:DateTo>{toDate:yyyy-MM-dd}</iob:DateTo>");
            sb.AppendLine("          <iob:DateSpanReferenceDate>CreationDate</iob:DateSpanReferenceDate>");
            sb.AppendLine("        </iob:Period>");
            sb.AppendLine("      </iob:query>");
            sb.AppendLine("    </iob:QueryClaims>");
            sb.AppendLine("  </soap:Body>");
            sb.AppendLine("</soap:Envelope>");
            
            return sb.ToString();
        }

        private static string ConstructCreateClaimsEnvelope(IReadOnlyCollection<ClaimBatchItem> claims)
        {
            var sb = new StringBuilder();

            string messageId = "urn:uuid:" + Guid.NewGuid();
            string tokenId = "UsernameToken-1";
            string timestampId = "TS-1";
            string createdTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string expiresTime = DateTime.UtcNow.AddMinutes(10).ToString("yyyy-MM-ddTHH:mm:ssZ");

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\"");
            sb.AppendLine("               xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"");
            sb.AppendLine("               xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"");
            sb.AppendLine("               xmlns:wsa=\"http://www.w3.org/2005/08/addressing\"");
            sb.AppendLine(">");
            sb.AppendLine("  <soap:Header>");
            sb.AppendLine($"    <wsa:Action soap:mustUnderstand=\"1\">{CreateClaimsSoapAction}</wsa:Action>");
            sb.AppendLine($"    <wsa:MessageID>{messageId}</wsa:MessageID>");
            sb.AppendLine($"    <wsa:To soap:mustUnderstand=\"1\">{SoapEndpoint}</wsa:To>");
            sb.AppendLine("    <wsse:Security soap:mustUnderstand=\"1\">");
            sb.AppendLine($"      <wsu:Timestamp wsu:Id=\"{timestampId}\">");
            sb.AppendLine($"        <wsu:Created>{createdTime}</wsu:Created>");
            sb.AppendLine($"        <wsu:Expires>{expiresTime}</wsu:Expires>");
            sb.AppendLine("      </wsu:Timestamp>");
            sb.AppendLine($"      <wsse:UsernameToken wsu:Id=\"{tokenId}\">");
            sb.AppendLine($"        <wsse:Username>{Username}</wsse:Username>");
            sb.AppendLine($"        <wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{Password}</wsse:Password>");
            sb.AppendLine($"        <wsu:Created>{createdTime}</wsu:Created>");
            sb.AppendLine("      </wsse:UsernameToken>");
            sb.AppendLine("    </wsse:Security>");
            sb.AppendLine("  </soap:Header>");
            //sb.AppendLine(OneClaim);
            sb.AppendLine("  <soap:Body \t\txmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"\r\n\t\txmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            sb.AppendLine("    <CreateClaims xmlns=\"http://IcelandicOnlineBanking/2005/12/01/Claims\">");
            sb.AppendLine("      <claims>");
            var claim = claims.First();
            string bank = claim.AccountNumber.Substring(0, 4);
            string hb = claim.AccountNumber.Substring(4, 2);
            int accountNum = int.Parse(claim.AccountNumber.Substring(6));
            for (int i = 0; i< 300; i++)
            {
                sb.AppendLine("      <Claim xmlns=\"http://IcelandicOnlineBanking/2005/12/01/ClaimTypes\">");
                sb.AppendLine("        <Key>");
                sb.AppendLine($"          <ClaimantID>{SecurityElement.Escape(claim.ClaimantId)}</ClaimantID>");
                sb.AppendLine($"          <Account>{bank}{hb}{accountNum}</Account>");
                sb.AppendLine($"          <DueDate>{claim.DueDate:yyyy-MM-dd}</DueDate>");
                sb.AppendLine("        </Key>");
                sb.AppendLine($"        <PayorID>{SecurityElement.Escape(claim.PayorId)}</PayorID>");
                sb.AppendLine($"        <Identifier>{SecurityElement.Escape(claim.Identifier)}</Identifier>");
                sb.AppendLine($"        <Amount>{claim.Capital}</Amount>");
                sb.AppendLine($"        <Reference>{SecurityElement.Escape(claim.Reference)}</Reference>");
                sb.AppendLine($"        <FinalDueDate>{claim.DueDate.AddDays(10):yyyy-MM-dd}</FinalDueDate>");
                sb.AppendLine($"        <CancellationDate>{claim.DueDate.AddYears(4).AddDays(-1):yyyy-MM-dd}</CancellationDate>");
                sb.AppendLine($"        <BillNumber>{SecurityElement.Escape(claim.AccountNumber.Substring(6))}</BillNumber>");
                sb.AppendLine($"        <CustomerNumber>{SecurityElement.Escape(claim.PayorId)}</CustomerNumber>");
                sb.AppendLine($"        <NoticeAndPaymentFee>");
                sb.AppendLine($"          <Printing>100</Printing>");
                sb.AppendLine($"          <Paperless>100</Paperless>");
                sb.AppendLine($"        </NoticeAndPaymentFee>");
                sb.AppendLine($"        <OtherCosts>0</OtherCosts>");
                sb.AppendLine($"        <OtherDefaultCosts>0</OtherDefaultCosts>");
                sb.AppendLine($"        <PermitOutOfSequencePayment>false</PermitOutOfSequencePayment>");
                sb.AppendLine($"        <IsPartialPaymentAllowed>false</IsPartialPaymentAllowed>");
                sb.AppendLine("      </Claim>");
                accountNum++;
            }
            sb.AppendLine("      </claims>");
            sb.AppendLine("    </CreateClaims>");
            sb.AppendLine("  </soap:Body>");
            sb.AppendLine("</soap:Envelope>");

            return sb.ToString();
        }


        private static string ConstructGetClaimOperationResultEnvelope(string operationId)
        {
            var sb = new StringBuilder();

            string messageId = "urn:uuid:" + Guid.NewGuid();
            string tokenId = "UsernameToken-1";
            string timestampId = "TS-1";
            string createdTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string expiresTime = DateTime.UtcNow.AddMinutes(10).ToString("yyyy-MM-ddTHH:mm:ssZ");

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\"");
            sb.AppendLine("               xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"");
            sb.AppendLine("               xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\"");
            sb.AppendLine("               xmlns:wsa=\"http://www.w3.org/2005/08/addressing\"");
            sb.AppendLine("               xmlns:iob=\"http://IcelandicOnlineBanking/2005/12/01/Claims\">");
            sb.AppendLine("  <soap:Header>");
            sb.AppendLine($"    <wsa:Action soap:mustUnderstand=\"1\">{GetClaimOperationResultSoapAction}</wsa:Action>");
            sb.AppendLine($"    <wsa:MessageID>{messageId}</wsa:MessageID>");
            sb.AppendLine($"    <wsa:To soap:mustUnderstand=\"1\">{SoapEndpoint}</wsa:To>");
            sb.AppendLine("    <wsse:Security soap:mustUnderstand=\"1\">");
            sb.AppendLine($"      <wsu:Timestamp wsu:Id=\"{timestampId}\">");
            sb.AppendLine($"        <wsu:Created>{createdTime}</wsu:Created>");
            sb.AppendLine($"        <wsu:Expires>{expiresTime}</wsu:Expires>");
            sb.AppendLine("      </wsu:Timestamp>");
            sb.AppendLine($"      <wsse:UsernameToken wsu:Id=\"{tokenId}\">");
            sb.AppendLine($"        <wsse:Username>{Username}</wsse:Username>");
            sb.AppendLine($"        <wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{Password}</wsse:Password>");
            sb.AppendLine($"        <wsu:Created>{createdTime}</wsu:Created>");
            sb.AppendLine("      </wsse:UsernameToken>");
            sb.AppendLine("    </wsse:Security>");
            sb.AppendLine("  </soap:Header>");
            sb.AppendLine("  <soap:Body>");
            sb.AppendLine("    <iob:GetClaimOperationResult>");
            sb.AppendLine($"      <iob:id>{SecurityElement.Escape(operationId)}</iob:id>");
            sb.AppendLine("    </iob:GetClaimOperationResult>");
            sb.AppendLine("  </soap:Body>");
            sb.AppendLine("</soap:Envelope>");

            return sb.ToString();
        }

        private static async Task<string> ExecuteSoapRequestAsync(string soapEnvelope, string soapAction)
        {
            using var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, SoapEndpoint);
            httpClient.Timeout = TimeSpan.FromSeconds(300);
            request.Content = new StringContent(soapEnvelope, Encoding.UTF8, "application/soap+xml");
            request.Content.Headers.ContentType.Parameters.Add(
                new System.Net.Http.Headers.NameValueHeaderValue("action", $"\"{soapAction}\""));

            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                stopwatch.Start();
                HttpResponseMessage response = await httpClient.SendAsync(request);
                stopwatch.Stop();
                Console.WriteLine($"SOAP request completed in {stopwatch.ElapsedMilliseconds} ms");
                string responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine("--- SOAP Response ---");
                Console.WriteLine($"Status: {response.StatusCode}");
                Console.WriteLine(FormatXml(responseContent));

                return responseContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine("--- Error ---");
                Console.WriteLine($"Exception: {ex.GetType().Name}");
                Console.WriteLine($"Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Parses the SOAP response and extracts claim data
        /// </summary>
        /// <param name="soapResponse">The raw SOAP response XML</param>
        /// <returns>A list of parsed claim results</returns>
        public static List<ClaimResult> ParseClaimsFromSoapResponse(string soapResponse)
        {
            var claims = new List<ClaimResult>();
            
            try
            {
                // Load the XML document
                var doc = XDocument.Parse(soapResponse);
                
                // Define namespaces
                XNamespace soapNs = "http://www.w3.org/2003/05/soap-envelope";
                XNamespace iobsNs = "http://IcelandicOnlineBanking/2005/12/01/Claims";
                
                // Log the XML for debugging
                Console.WriteLine("Parsing XML response:");
                Console.WriteLine(FormatXml(soapResponse));
                
                try
                {
                    // First try with the standard namespace approach
                    var claimElements = doc.Descendants(iobsNs + "Claim");
                    
                    if (!claimElements.Any())
                    {
                        // If no elements found, try with a more flexible approach using local name
                        Console.WriteLine("No claims found with standard namespace. Trying with local name...");
                        claimElements = doc.Descendants()
                            .Where(e => e.Name.LocalName == "Claims");
                    }
                    
                    foreach (var claimElement in claimElements)
                    {
                        try
                        {
                            // Helper function to get element value by local name
                            string GetElementValue(XElement parent, string localName, string defaultValue = "")
                            {
                                var element = parent.Elements()
                                    .FirstOrDefault(e => e.Name.LocalName == localName);
                                return element?.Value ?? defaultValue;
                            }

                            var keyElement = claimElement.Elements()
                                .FirstOrDefault(e => e.Name.LocalName == "Key");
                            
                            var claim = new ClaimResult
                            {


                                PayorID = GetElementValue(claimElement, "PayorID"),
                                CancellationDate = DateTime.Parse(GetElementValue(claimElement, "CancellationDate")),
                                ClaimId = GetElementValue(claimElement, "Identifier"),
                                Status = GetElementValue(claimElement, "Status"),
                                Reference = GetElementValue(claimElement, "Reference"),
                                Amount = GetElementValue(claimElement, "Amount"),
                                DueDate = DateTime.Parse(GetElementValue(keyElement, "DueDate")),
                                Account = GetElementValue(keyElement, "Account"),
                                // Capital = decimal.Parse(GetElementValue(claimElement, "Capital", "0"))
                            };
                            
                            claims.Add(claim);
                            // Console.WriteLine($"Parsed claim: {claim.ClaimId}, {claim.Reference}, {claim.DueDate}, {claim.Capital}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing claim: {ex.Message}");
                            // Continue with the next claim
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during claim element selection: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing SOAP response: {ex.Message}");
                // Return empty list on error
            }
            
            return claims;
        }

        /// <summary>
        /// Determines if the current endpoint is the mock endpoint
        /// </summary>
        /// <returns>True if using the mock endpoint, false otherwise</returns>
        public static bool IsUsingMockEndpoint()
        {
            string? endpoint = Environment.GetEnvironmentVariable("INKASSO_URL");
            return !string.IsNullOrEmpty(endpoint) && endpoint.Contains("mock-inkasso");
        }

        /// <summary>
        /// Formats XML for better readability in console output
        /// </summary>
        private static string FormatXml(string xml)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                
                var sb = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = Environment.NewLine,
                    NewLineHandling = NewLineHandling.Replace
                };
                
                using (var writer = XmlWriter.Create(sb, settings))
                {
                    doc.Save(writer);
                }
                
                return sb.ToString();
            }
            catch
            {
                // If XML formatting fails, return the original string
                return xml;
            }
        }
    }
}
