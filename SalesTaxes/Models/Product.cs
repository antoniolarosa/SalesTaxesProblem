namespace SalesTaxes.Models
{
    public class Product : IProduct
    {
        public int Quantity { get; }
        public decimal Price { get; }
        public bool IsImported { get; }
        public CategoryType Category { get; }
        public string ProductName { get; }
        public string Description { get; }

        public Product(int quantity, decimal price, string description, string productName, bool isImported, CategoryType category)
        {
            Quantity = quantity;
            Price = price * quantity;
            Description = description;
            ProductName = productName;
            IsImported = isImported;
            Category = category;
        }

        public string GetDescription()
        {
            return Description;
        }

        public decimal CalculateGrossAmount()
        {
            return Price;
        }
    }
}
