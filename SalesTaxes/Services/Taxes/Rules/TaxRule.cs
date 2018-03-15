using SalesTaxes.Entities;
using SalesTaxes.Services.Taxes.Decorators;

namespace SalesTaxes.Services.Taxes.Rules
{
    public abstract class TaxRule
    {
        public string Description { get; set; }
        public abstract bool IsApplicableFor(IProduct product);
        public abstract ProductDecorator GetTaxableProductDecorator(IProduct product);
    }
}
