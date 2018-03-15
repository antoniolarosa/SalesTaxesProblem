using System.Collections.Generic;

namespace SalesTaxes.Models
{
    public class ReceiptDetail
    {
        public string Receipt { get; }
        public IList<TaxedProduct> TaxedProducts { get; }

        public ReceiptDetail(string receipt, IList<TaxedProduct> taxedProducts)
        {
            Receipt = receipt;
            TaxedProducts = taxedProducts;
        }
    }
}