using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Models
{
    public enum TransactionType
    {
        StockIn,
        StockOut
    }

    public class StockTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public TransactionType Type { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public string PerformedBy { get; set; } = string.Empty;
    }
}