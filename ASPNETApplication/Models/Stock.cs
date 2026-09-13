namespace InventoryManagement.Web.Models
{
    public class Stock
    {
        public int StockId { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }
    }
}