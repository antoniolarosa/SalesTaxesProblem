using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SalesTaxes.Models;

namespace SalesTaxes.Services
{
    public class ReceiptDetailCreator
    {
        public ReceiptDetail CreateReceiptDetail(IList<TaxedProduct> taxedProducts)
        {
            if (taxedProducts == null) throw new ArgumentNullException(nameof(taxedProducts));
            if (!taxedProducts.Any())
            {
                return new ReceiptDetail(string.Empty, taxedProducts);
            }

            var stringBuilder = new StringBuilder();

            AddPurchasedItems(taxedProducts, stringBuilder);
            AddSalesTaxes(taxedProducts, stringBuilder);
            AddTotal(taxedProducts, stringBuilder);

            string receipt = stringBuilder.ToString();

            return new ReceiptDetail(receipt, taxedProducts);
        }

        private static void AddTotal(IList<TaxedProduct> taxedProducts, StringBuilder stringBuilder)
        {
            decimal total = taxedProducts.Sum(taxedGood => taxedGood.GrossAmount);
            stringBuilder.AppendLine($"Total: {total}");
        }

        private static void AddSalesTaxes(IList<TaxedProduct> taxedProducts, StringBuilder stringBuilder)
        {
            decimal salesTaxes = taxedProducts.Sum(taxedGood => taxedGood.Tax);
            stringBuilder.AppendLine($"Sales Taxes: {salesTaxes}");
        }

        private static void AddPurchasedItems(IList<TaxedProduct> taxedProducts, StringBuilder stringBuilder)
        {
            foreach (TaxedProduct taxedGood in taxedProducts)
            {
                string imported = " ";
                if (taxedGood.Product.IsImported)
                {
                    imported = $" {Constants.Imported} ";
                }

                stringBuilder.AppendLine(
                    $"{taxedGood.Product.Quantity}{imported}{taxedGood.Product.ProductName}: {taxedGood.GrossAmount}");
            }
        }
    }
}
