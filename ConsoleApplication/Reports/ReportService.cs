using System;
using System.Linq;
using InventoryManagementSystem.Services;

namespace InventoryManagementSystem.Reports
{
    public class ReportService
    {
        private ProductService _productService;
        private StockService _stockService;

        public ReportService(ProductService productService, StockService stockService)
        {
            _productService = productService;
            _stockService = stockService;
        }

        public void GenerateStockSummaryReport()
        {
            Console.WriteLine("\n===== STOCK SUMMARY REPORT =====");
            _stockService.DisplayAllStock();
            Console.WriteLine("=================================\n");
        }

        public void GenerateLowStockReport()
        {
            Console.WriteLine("\n===== LOW STOCK REPORT =====");
            var lowStockItems = _stockService.GetLowStockItems();

            if (lowStockItems.Count == 0)
            {
                Console.WriteLine("No low stock items.");
            }
            else
            {
                foreach (var item in lowStockItems)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            Console.WriteLine("=============================\n");
        }

        public void GenerateTransactionReport()
        {
            Console.WriteLine("\n===== TRANSACTION HISTORY REPORT =====");
            _stockService.DisplayTransactionHistory();
            Console.WriteLine("========================================\n");
        }

        public void GenerateProductCatalogReport()
        {
            Console.WriteLine("\n===== PRODUCT CATALOG REPORT =====");
            _productService.DisplayAllProducts();
            Console.WriteLine("===================================\n");
        }

        public void GenerateInventoryValueReport()
        {
            Console.WriteLine("\n===== INVENTORY VALUE REPORT =====");
            var products = _productService.GetAllProducts();

            if (products.Count == 0)
            {
                Console.WriteLine("No products available.");
            }
            else
            {
                decimal totalValue = 0;
                foreach (var product in products)
                {
                    decimal value = product.Price * product.Quantity;
                    totalValue += value;
                    Console.WriteLine($"{product.ProductName} - Value: {value:C}");
                }
                Console.WriteLine($"\nTotal Inventory Value: {totalValue:C}");
            }
            Console.WriteLine("===================================\n");
        }
    }
}