using System.Collections.Generic;
using System.Linq;
using SalesTaxes.Services.Taxes.Rules;

namespace SalesTaxes.Settings
{
    public class TaxSettings
    {
        public IList<FlatTax> FlatTaxRules { get; set; } = new List<FlatTax>();
        public IList<ImportedTax> ImportedTaxRule { get; set; } = new List<ImportedTax>();

        public IList<Tax> GetAllTaxRules()
        {
            IEnumerable<Tax> flatTaxRules = FlatTaxRules;
            IEnumerable<Tax> importedProductTaxRules = ImportedTaxRule;
            IEnumerable<Tax> taxRules = flatTaxRules.Concat(importedProductTaxRules);
            return taxRules.ToList();
        }
    }
}
