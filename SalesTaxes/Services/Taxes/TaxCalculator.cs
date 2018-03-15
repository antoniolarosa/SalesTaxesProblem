using System;
using System.Collections.Generic;
using SalesTaxes.Entities;
using SalesTaxes.Services.Taxes.Decorators;
using SalesTaxes.Services.Taxes.Rules;

namespace SalesTaxes.Services.Taxes
{
    public class TaxCalculator
    {
        private readonly IList<TaxRule> _taxRules;

        public TaxCalculator(IList<TaxRule> taxRules)
        {
            _taxRules = taxRules;         
        }

        public IList<TaxedProduct> ApplyTaxes(ShoppingBasket shoppingBasket)
        {
            if (shoppingBasket == null) throw new ArgumentNullException(nameof(shoppingBasket));

            IList<TaxedProduct> taxedProducts = new List<TaxedProduct>();
            for (var i = 0; i < shoppingBasket.Products.Count; i++)
            {
                IProduct product = shoppingBasket.Products[i];
                foreach (TaxRule taxRule in _taxRules)
                {
                    if (taxRule.IsApplicableFor(product))
                    {
                        ProductDecorator productDecorator = taxRule.GetTaxableProductDecorator(product);
                        product = productDecorator;
                    }
                }
                decimal grossAmount = product.CalculateGrossAmount();
                taxedProducts.Add(new TaxedProduct(product, grossAmount));
            }
            return taxedProducts;
        }
    }
}

