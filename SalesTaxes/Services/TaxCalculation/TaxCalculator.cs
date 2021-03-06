﻿using System;
using System.Collections.Generic;
using SalesTaxes.Entities;
using SalesTaxes.Services.TaxCalculation.Decorators;
using SalesTaxes.Services.TaxCalculation.Taxes;

namespace SalesTaxes.Services.TaxCalculation
{
    public class TaxCalculator
    {
        private readonly IList<Tax> _taxRules;

        public TaxCalculator(IList<Tax> taxRules)
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
                foreach (Tax taxRule in _taxRules)
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

