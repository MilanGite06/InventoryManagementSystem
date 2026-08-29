using System;

namespace InventoryManagementSystem.Models
{
    public class Product
    {
        private int _productId;
        private string _productName;
        private decimal _price;
        private int _quantity;

        public int ProductId
        {
            get => _productId;
            set => _productId = value;
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Product name cannot be empty.");
                _productName = value;
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                _price = value;
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be negative.");
                _quantity = value;
            }
        }

        // Relationships
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }

        public Product(int productId, string productName, decimal price, int quantity, Category category, Supplier supplier)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
            Category = category;
            Supplier = supplier;
        }

        public override string ToString()
        {
            return $"[{ProductId}] {ProductName} | Price: {Price:C} | Qty: {Quantity} | Category: {Category?.CategoryName} | Supplier: {Supplier?.SupplierName}";
        }
    }
}