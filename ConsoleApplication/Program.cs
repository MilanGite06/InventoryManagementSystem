using System;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Services;
using InventoryManagementSystem.Reports;
using System.Linq;

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
                            Console.Write("Enter reorder level for this product: ");
                            int reorderLevel = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter supplier name: ");
                            string supplierName = Console.ReadLine();
                            Console.Write("Enter category name: ");
                            string categoryName = Console.ReadLine();

                            Category category = new Category(1, categoryName);
                            Supplier supplier = new Supplier(1, supplierName, "0000000000");

                            productService.AddProduct(name, price, qty, category, supplier);

                            var addedProduct = productService.GetAllProducts()[productService.GetAllProducts().Count - 1];
                            stockService.InitializeStock(addedProduct, qty, reorderLevel);

                            Console.WriteLine("\n----- Product Added Successfully -----");
                            Console.WriteLine(addedProduct.ToString());
                            Console.WriteLine("---------------------------------------");
                            break;

                        case "2":
                            var allProducts = productService.GetAllProducts();
                            if (allProducts.Count == 0)
                            {
                                Console.WriteLine("No products available to update.");
                                break;
                            }

                            Console.WriteLine("\n----- Select Product to Update -----");
                            foreach (var p in allProducts)
                            {
                                Console.WriteLine(p.ToString());
                            }
                            Console.Write("\nEnter product ID to update: ");
                            int updateId = Convert.ToInt32(Console.ReadLine());

                            var productToUpdate = allProducts.FirstOrDefault(p => p.ProductId == updateId);
                            if (productToUpdate == null)
                            {
                                Console.WriteLine("Product not found.");
                                break;
                            }

                            Console.WriteLine($"\nCurrent details: {productToUpdate.ToString()}");

                            Console.Write($"Enter new name (leave blank to keep '{productToUpdate.ProductName}'): ");
                            string newName = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(newName)) newName = productToUpdate.ProductName;

                            Console.Write($"Enter new price (leave blank to keep {productToUpdate.Price}): ");
                            string priceInput = Console.ReadLine();
                            decimal newPrice = string.IsNullOrWhiteSpace(priceInput) ? productToUpdate.Price : Convert.ToDecimal(priceInput);

                            Console.Write($"Enter new quantity (leave blank to keep {productToUpdate.Quantity}): ");
                            string qtyInput = Console.ReadLine();
                            int newQty = string.IsNullOrWhiteSpace(qtyInput) ? productToUpdate.Quantity : Convert.ToInt32(qtyInput);

                            Console.Write($"Enter new category (leave blank to keep '{productToUpdate.Category?.CategoryName}'): ");
                            string newCategoryName = Console.ReadLine();
                            Category newCategory = string.IsNullOrWhiteSpace(newCategoryName)
                                ? productToUpdate.Category
                                : new Category(productToUpdate.Category?.CategoryId ?? 1, newCategoryName);

                            Console.Write($"Enter new supplier (leave blank to keep '{productToUpdate.Supplier?.SupplierName}'): ");
                            string newSupplierName = Console.ReadLine();
                            Supplier newSupplier = string.IsNullOrWhiteSpace(newSupplierName)
                                ? productToUpdate.Supplier
                                : new Supplier(productToUpdate.Supplier?.SupplierId ?? 1, newSupplierName, productToUpdate.Supplier?.ContactNumber ?? "0000000000");

                            productService.UpdateProduct(updateId, newName, newPrice, newQty, newCategory, newSupplier);

                            Console.WriteLine("\n----- Updated Product -----");
                            Console.WriteLine(productToUpdate.ToString());
                            Console.WriteLine("----------------------------");
                            break;

                        case "3":
                            var productsForDelete = productService.GetAllProducts();
                            if (productsForDelete.Count == 0)
                            {
                                Console.WriteLine("No products available to delete.");
                                break;
                            }

                            Console.WriteLine("\n----- Select Product to Delete -----");
                            foreach (var p in productsForDelete)
                            {
                                Console.WriteLine(p.ToString());
                            }
                            Console.Write("\nEnter product ID to delete: ");
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
                            stockService.DisplayAllStock();
                            Console.Write("\nEnter product ID to stock in: ");
                            int inId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter quantity to add: ");
                            int inQty = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Performed by (staff name): ");
                            string inBy = Console.ReadLine();
                            stockService.StockIn(inId, inQty, inBy);
                            break;

                        case "2":
                            stockService.DisplayAllStock();
                            Console.Write("\nEnter product ID to stock out: ");
                            int outId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter quantity to remove: ");
                            int outQty = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Performed by (staff name): ");
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