using System;

namespace InkassoMiddleware.Models
{
    /// <summary>
    /// Represents a parsed claim result from the Inkasso SOAP response
    /// </summary>
    public class ClaimResult
    {
        public string ClaimId { get; set; } = string.Empty;
        public string PayorID { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CancellationDate { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;




        public DateTime DueDate { get; set; }
        public decimal Capital { get; set; }
    }
}
