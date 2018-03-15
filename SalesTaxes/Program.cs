using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using SalesTaxes.Entities;
using SalesTaxes.Services;
using SalesTaxes.Services.TaxCalculation;
using SalesTaxes.Services.TaxCalculation.Taxes;
using SalesTaxes.Settings;

namespace SalesTaxes
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Bootstrap
            
            IConfigurationRoot configuration = GetConfigurationRoot();
            var configurationHelper = new ConfigurationHelper(configuration);
            TaxSettings taxSettings = configurationHelper.GetTaxSettings();
            IList<Tax> taxes = taxSettings.GetAllTaxes();
            var taxCalculator = new TaxCalculator(taxes);
            IDictionary<string, CategoryType> productCategories = configurationHelper.GetProductCategories();
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

        private static IConfigurationRoot GetConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }
    }
}
