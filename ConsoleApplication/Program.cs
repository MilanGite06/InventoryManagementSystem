using System;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Reports;

namespace InventoryManagementSystem
{
    class Program
    {
        static ProductService productService = new ProductService();
        static StockService stockService = new StockService();
        static ReportService reportService;

        static void Main(string[] args)
        {
            reportService = new ReportService(productService, stockService);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n===== INVENTORY MANAGEMENT SYSTEM =====");
                Console.WriteLine("1. Product Management");
                Console.WriteLine("2. Stock Management");
                Console.WriteLine("3. Reports");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProductMenu();
                        break;
                    case "2":
                        StockMenu();
                        break;
                    case "3":
                        ReportsMenu();
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Exiting... Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        // ---------------- PRODUCT MENU ----------------
        static void ProductMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n----- Product Management -----");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. Search Product");
                Console.WriteLine("5. View All Products");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter product name: ");
                            string name = Console.ReadLine();
                            Console.Write("Enter price: ");
                            decimal price = Convert.ToDecimal(Console.ReadLine());
                            Console.Write("Enter quantity: ");
                            int qty = Convert.ToInt32(Console.ReadLine());

                            Category category = new Category(1, "General"); // simplified for console phase
                            Supplier supplier = new Supplier(1, "Default Supplier", "0000000000");

                            productService.AddProduct(name, price, qty, category, supplier);

                            // Also initialize stock record for this product
                            var addedProduct = productService.GetAllProducts()[productService.GetAllProducts().Count - 1];
                            Console.Write("Enter reorder level for this product: ");
                            int reorderLevel = Convert.ToInt32(Console.ReadLine());
                            stockService.InitializeStock(addedProduct, qty, reorderLevel);
                            break;

                        case "2":
                            Console.Write("Enter product ID to update: ");
                            int updateId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter new name: ");
                            string newName = Console.ReadLine();
                            Console.Write("Enter new price: ");
                            decimal newPrice = Convert.ToDecimal(Console.ReadLine());
                            Console.Write("Enter new quantity: ");
                            int newQty = Convert.ToInt32(Console.ReadLine());
                            productService.UpdateProduct(updateId, newName, newPrice, newQty);
                            break;

                        case "3":
                            Console.Write("Enter product ID to delete: ");
                            int deleteId = Convert.ToInt32(Console.ReadLine());
                            productService.DeleteProduct(deleteId);
                            break;

                        case "4":
                            Console.Write("Enter search keyword: ");
                            string keyword = Console.ReadLine();
                            var results = productService.SearchProduct(keyword);
                            if (results.Count == 0)
                                Console.WriteLine("No matching products found.");
                            else
                                results.ForEach(p => Console.WriteLine(p.ToString()));
                            break;

                        case "5":
                            productService.DisplayAllProducts();
                            break;

                        case "6":
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please enter the correct data type.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        // ---------------- STOCK MENU ----------------
        static void StockMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n----- Stock Management -----");
                Console.WriteLine("1. Stock In");
                Console.WriteLine("2. Stock Out");
                Console.WriteLine("3. View All Stock");
                Console.WriteLine("4. View Transaction History");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter product ID: ");
                            int inId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter quantity to add: ");
                            int inQty = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Performed by (name): ");
                            string inBy = Console.ReadLine();
                            stockService.StockIn(inId, inQty, inBy);
                            break;

                        case "2":
                            Console.Write("Enter product ID: ");
                            int outId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter quantity to remove: ");
                            int outQty = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Performed by (name): ");
                            string outBy = Console.ReadLine();
                            stockService.StockOut(outId, outQty, outBy);
                            break;

                        case "3":
                            stockService.DisplayAllStock();
                            break;

                        case "4":
                            stockService.DisplayTransactionHistory();
                            break;

                        case "5":
                            back = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please enter the correct data type.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        // ---------------- REPORTS MENU ----------------
        static void ReportsMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n----- Reports -----");
                Console.WriteLine("1. Product Catalog Report");
                Console.WriteLine("2. Stock Summary Report");
                Console.WriteLine("3. Low Stock Report");
                Console.WriteLine("4. Transaction History Report");
                Console.WriteLine("5. Inventory Value Report");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        reportService.GenerateProductCatalogReport();
                        break;
                    case "2":
                        reportService.GenerateStockSummaryReport();
                        break;
                    case "3":
                        reportService.GenerateLowStockReport();
                        break;
                    case "4":
                        reportService.GenerateTransactionReport();
                        break;
                    case "5":
                        reportService.GenerateInventoryValueReport();
                        break;
                    case "6":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}