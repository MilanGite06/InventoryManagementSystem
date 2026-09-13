using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Web.Data;
using InventoryManagement.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InventoryManagement.Web.Controllers
{
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stock - shows current stock levels for all products
        public IActionResult Index()
        {
            var stocks = _context.Stocks
                .Include(s => s.Product)
                .ToList();

            return View(stocks);
        }

        // GET: Stock/Initialize - set up stock tracking for a product that doesn't have it yet
        public IActionResult Initialize()
        {
            var productsWithoutStock = _context.Products
                .Where(p => !_context.Stocks.Any(s => s.ProductId == p.ProductId))
                .ToList();

            ViewBag.Products = productsWithoutStock;
            return View();
        }

        [HttpPost]
        public IActionResult Initialize(int productId, int quantityAvailable, int reorderLevel)
        {
            var stock = new Stock
            {
                ProductId = productId,
                QuantityAvailable = quantityAvailable,
                ReorderLevel = reorderLevel
            };
            _context.Stocks.Add(stock);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Stock/StockIn
        public IActionResult StockIn()
        {
            ViewBag.Stocks = _context.Stocks.Include(s => s.Product).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult StockIn(int productId, int quantity, string performedBy)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.ProductId == productId);
            if (stock == null)
            {
                ViewBag.Error = "Stock record not found for this product.";
                ViewBag.Stocks = _context.Stocks.Include(s => s.Product).ToList();
                return View();
            }

            stock.QuantityAvailable += quantity;

            var transaction = new StockTransaction
            {
                ProductId = productId,
                Type = TransactionType.StockIn,
                Quantity = quantity,
                PerformedBy = performedBy
            };
            _context.StockTransactions.Add(transaction);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Stock/StockOut
        public IActionResult StockOut()
        {
            ViewBag.Stocks = _context.Stocks.Include(s => s.Product).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult StockOut(int productId, int quantity, string performedBy)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.ProductId == productId);
            if (stock == null)
            {
                ViewBag.Error = "Stock record not found for this product.";
                ViewBag.Stocks = _context.Stocks.Include(s => s.Product).ToList();
                return View();
            }

            if (quantity > stock.QuantityAvailable)
            {
                ViewBag.Error = "Insufficient stock available.";
                ViewBag.Stocks = _context.Stocks.Include(s => s.Product).ToList();
                return View();
            }

            stock.QuantityAvailable -= quantity;

            var transaction = new StockTransaction
            {
                ProductId = productId,
                Type = TransactionType.StockOut,
                Quantity = quantity,
                PerformedBy = performedBy
            };
            _context.StockTransactions.Add(transaction);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Stock/Transactions - view transaction history
        public IActionResult Transactions()
        {
            var transactions = _context.StockTransactions
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();

            return View(transactions);
        }
    }
}