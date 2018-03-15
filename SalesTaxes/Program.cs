using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using SalesTaxes.Entities;
using SalesTaxes.Services;
using SalesTaxes.Services.Taxes;
using SalesTaxes.Services.Taxes.Rules;
using SalesTaxes.Settings;

namespace SalesTaxes
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Bootstrap
            
            var configurationHelper = new ConfigurationHelper();
            IConfigurationRoot configuration = configurationHelper.GetConfigurationRoot();
            TaxSettings taxSettings = configurationHelper.GetTaxSettings(configuration);
            IList<TaxRule> taxRules = taxSettings.GetAllTaxRules();
            var taxCalculator = new TaxCalculator(taxRules);
            IDictionary<string, CategoryType> productCategories = configurationHelper.GetProductCategories(configuration);
            var shoppingBasketCreator = new ShoppingBasketCreator();
            var receiptDetailCreator = new ReceiptDetailCreator();
            var receiptDeatilPrinter = new ReceiptDeatilPrinter();

            #endregion

            string inputFilePath = configuration[Constants.AppSettings.Input];
            string[] inputLines = File.ReadAllLines(inputFilePath);
            ShoppingBasket shoppingBasket = shoppingBasketCreator.CreateShoppingBasket(inputLines, productCategories);
            IList<TaxedProduct> taxedProducts = taxCalculator.ApplyTaxes(shoppingBasket);
            ReceiptDetail receiptDetail = receiptDetailCreator.CreateReceiptDetail(taxedProducts);
            receiptDeatilPrinter.Print(receiptDetail);
        }
    }
}
