using InventoryManagement.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace InventoryManagement.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Username = username;
            ViewBag.Role = HttpContext.Session.GetString("Role");

            // Key metrics
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalSuppliers = _context.Suppliers.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
            ViewBag.LowStockCount = _context.Stocks.Count(s => s.QuantityAvailable <= s.ReorderLevel);

            var totalValue = _context.Products
                .Sum(p => p.Price * (_context.Stocks
                    .Where(s => s.ProductId == p.ProductId)
                    .Select(s => s.QuantityAvailable)
                    .FirstOrDefault()));
            ViewBag.TotalInventoryValue = totalValue;

            ViewBag.RecentTransactions = _context.StockTransactions
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToList();

            return View();
        }
    }
}