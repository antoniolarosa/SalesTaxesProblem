using System.Collections.Generic;
using System.Linq;
using SalesTaxes.Services.Taxes.Rules;

namespace SalesTaxes.Settings
{
    public class TaxSettings
    {
        public IList<FlatTaxRule> FlatTaxRules { get; set; } = new List<FlatTaxRule>();
        public IList<ImportedTaxRule> ImportedTaxRule { get; set; } = new List<ImportedTaxRule>();

        public IList<TaxRule> GetAllTaxRules()
        {
            IEnumerable<TaxRule> flatTaxRules = FlatTaxRules;
            IEnumerable<TaxRule> importedProductTaxRules = ImportedTaxRule;
            IEnumerable<TaxRule> taxRules = flatTaxRules.Concat(importedProductTaxRules);
            return taxRules.ToList();
        }
    }
}
