﻿using System.Collections.Generic;
using System.Linq;
using SalesTaxes.Services.Taxes.Rules;

namespace SalesTaxes.Settings
{
    public class TaxSettings
    {
        public IList<FlatTax> FlatTaxes { get; set; } = new List<FlatTax>();
        public IList<ImportedTax> ImportedTaxes { get; set; } = new List<ImportedTax>();

        public IList<Tax> GetAllTaxRules()
        {
            IEnumerable<Tax> flatTaxRules = FlatTaxes;
            IEnumerable<Tax> importedProductTaxRules = ImportedTaxes;
            IEnumerable<Tax> taxRules = flatTaxRules.Concat(importedProductTaxRules);
            return taxRules.ToList();
        }
    }
}
