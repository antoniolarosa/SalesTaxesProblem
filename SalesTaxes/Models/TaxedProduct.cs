namespace SalesTaxes.Models
{
    public class TaxedProduct 
    {
        public IProduct Product { get; }
        public decimal GrossAmount { get; }
        public decimal Tax => GrossAmount - Product.Price ;

        public TaxedProduct(IProduct product, decimal grossAmount)
        {
            Product = product;
            GrossAmount = grossAmount;
        }

        public override string ToString()
        {
            return $"{nameof(Product.Description)}: {Product.Description}, {nameof(GrossAmount)}: {GrossAmount}, {nameof(Tax)}: {Tax}";
        }
    }
}
