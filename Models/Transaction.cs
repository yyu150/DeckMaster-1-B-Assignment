using System;

namespace DeckMaster.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string? TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string? Currency { get; set; }

        public string? PayerName { get; set; }

        public string? PayerEmail { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}