using SalesTaxes.Entities;
using SalesTaxes.Services.TaxCalculation.Decorators;

namespace SalesTaxes.Services.TaxCalculation.Taxes
{
    public abstract class Tax
    {
        public string Description { get; set; }
        public abstract bool IsApplicableFor(IProduct product);
        public abstract ProductDecorator GetTaxableProductDecorator(IProduct product);
    }
}
