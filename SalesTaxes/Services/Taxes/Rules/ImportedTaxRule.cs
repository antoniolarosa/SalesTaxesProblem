using System;
using SalesTaxes.Entities;
using SalesTaxes.Services.Taxes.Decorators;

namespace SalesTaxes.Services.Taxes.Rules
{
    public class ImportedTaxRule : TaxRule
    {
        public decimal Rate { get; set; }

        public override bool IsApplyiableTo(IProduct product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            return product.IsImported;
        }

        public override ProductDecorator GetTaxableProductDecorator(IProduct product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            return new FlatTaxDecorator(product, Description, Rate);
        }
    }
}