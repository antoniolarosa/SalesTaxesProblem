namespace SalesTaxes.Models
{
    public interface IProduct
    {
        string ProductName { get; }
        string Description { get;  }
        decimal Price { get;}
        bool IsImported { get; }
        CategoryType Category { get; }
        int Quantity { get; }

        string GetDescription();
        decimal CalculateGrossAmount();
    }
}
