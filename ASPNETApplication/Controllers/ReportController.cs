using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryManagement.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Report - main reports menu
        public IActionResult Index()
        {
            return View();
        }

        // GET: Report/ProductCatalog
        public IActionResult ProductCatalog(string searchTerm)
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm));
            }

            ViewBag.SearchTerm = searchTerm;
            return View(products.ToList());
        }

        // GET: Report/StockSummary
        public IActionResult StockSummary()
        {
            var stocks = _context.Stocks.Include(s => s.Product).ToList();
            return View(stocks);
        }

        // GET: Report/LowStock
        public IActionResult LowStock()
        {
            var lowStock = _context.Stocks
                .Include(s => s.Product)
                .Where(s => s.QuantityAvailable <= s.ReorderLevel)
                .ToList();
            return View(lowStock);
        }

        // GET: Report/TransactionHistory
        public IActionResult TransactionHistory(string type)
        {
            var transactions = _context.StockTransactions
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                transactions = transactions.Where(t => t.Type.ToString() == type);
            }

            ViewBag.SelectedType = type;
            return View(transactions.ToList());
        }

        // GET: Report/InventoryValue
        public IActionResult InventoryValue()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .ToList();

            var stockLookup = _context.Stocks.ToList();

            var report = products.Select(p => new
            {
                Product = p,
                Quantity = stockLookup.FirstOrDefault(s => s.ProductId == p.ProductId)?.QuantityAvailable ?? 0,
                Value = p.Price * (stockLookup.FirstOrDefault(s => s.ProductId == p.ProductId)?.QuantityAvailable ?? 0)
            }).ToList();

            ViewBag.TotalValue = report.Sum(r => r.Value);
            return View(report);
        }
    }
}