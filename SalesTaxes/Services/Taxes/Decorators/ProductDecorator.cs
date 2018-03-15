using SalesTaxes.Entities;

namespace SalesTaxes.Services.Taxes.Decorators
{
    public abstract class ProductDecorator : IProduct
    {
        protected readonly IProduct Product;

        public decimal Price { get; }
        public int Quantity { get; }
        public bool IsImported { get; }
        public CategoryType Category { get; }
        public string ProductName { get; }
        public string Description { get; set; }

        protected ProductDecorator(IProduct product)
        {
            Product = product;
            Price = Product.Price;
            Category = Product.Category;
            IsImported = Product.IsImported;
            Quantity = Product.Quantity;
            ProductName = Product.ProductName;
            Description = Product.Description;
        }

        public abstract string GetDescription();
        public abstract decimal CalculateGrossAmount();

    }
}
