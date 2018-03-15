using System;
using System.Collections.Generic;
using SalesTaxes.Entities;
using SalesTaxes.Services.Taxes.Decorators;

namespace SalesTaxes.Services.Taxes.Rules
{
    public class FlatTax : Tax
    {
        public decimal Rate { get; set; }
        public HashSet<CategoryType> ExcludedCategories { get; set; } = new HashSet<CategoryType>();

        public override bool IsApplicableFor(IProduct product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            return !ExcludedCategories.Contains(product.Category);
        }

        public override ProductDecorator GetTaxableProductDecorator(IProduct product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            return new FlatTaxDecorator(product, Description, Rate);
        }
    }
}