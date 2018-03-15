using System;
using System.Collections.Generic;
using NUnit.Framework;
using SalesTaxes.Entities;
using SalesTaxes.Services;
using SalesTaxes.Services.Taxes;
using SalesTaxes.Services.Taxes.Rules;

namespace SalesTaxes.Fixtures
{
    [TestFixture]
    public class IntegrationFixtures
    {
        private ShoppingBasketCreator _shoppingBasketCreator;
        private Dictionary<string, CategoryType> _productCategories;
        private TaxCalculator _taxCalculator;
        private ReceiptDetailCreator _receiptDetailCreator;

        [SetUp]
        public void Initialize()
        {
            _shoppingBasketCreator = new ShoppingBasketCreator();
            _productCategories = new Dictionary<string, CategoryType>()
            {
                {"book", CategoryType.Books},
                {"box of chocolates", CategoryType.Foods },
                {"chocolate bar", CategoryType.Foods },
                {"packet of headache pills", CategoryType.MedicalProducts },
            };

            IList<Tax> taxes = new List<Tax>()
            {
                new FlatTax()
                {
                    Description = "Basic Sales Tax",
                    ExcludedCategories = new HashSet<CategoryType>()
                    {
                        CategoryType.Books,
                        CategoryType.Foods,
                        CategoryType.MedicalProducts
                    },
                    Rate = 0.1m
                },
                new ImportedTax()
                {
                    Description = "Import duty",
                    Rate = 0.05m
                }
            };
            _taxCalculator = new TaxCalculator(taxes);
            _receiptDetailCreator = new ReceiptDetailCreator();


        }

        [Test]
        public void Input1()
        {
            //Arrange
            decimal bookPriceExpected = 12.49m;
            decimal musicCdPriceExpected = 16.49m;
            decimal chocolateBarPriceExpected = 0.85m;
            decimal salesTaxExpected = 1.5m;
            decimal totalExpected = 29.83m;
           
            string[] inputLines = new string[]
            {
                "1 book at 12.49",
                "1 music CD at 14.99",
                "1 chocolate bar at 0.85"
            };

            //Act
            ShoppingBasket shoppingBasket = _shoppingBasketCreator.CreateShoppingBasket(inputLines, _productCategories);
            IList<TaxedProduct> taxedProducts = _taxCalculator.ApplyTaxes(shoppingBasket);
            ReceiptDetail receiptDetail = _receiptDetailCreator.CreateReceiptDetail(taxedProducts);

            //Assert
            Assert.AreEqual($"1 book: {bookPriceExpected}{Environment.NewLine}1 music CD: {musicCdPriceExpected}{Environment.NewLine}1 chocolate bar: {chocolateBarPriceExpected}{Environment.NewLine}Sales Taxes: {salesTaxExpected.ToString().PadRight(4, '0')}{Environment.NewLine}Total: {totalExpected}{Environment.NewLine}", receiptDetail.Receipt);
        }

        [Test]
        public void Input2()
        {
            //Arrange
            decimal importedBoxOfChocolatesPriceExpected = 10.50m;
            decimal importedBottleOfPerfumePriceExpected = 54.65m;
            decimal salesTaxExpected = 7.65m;
            decimal totalExpected = 65.15m;

            string[] inputLines = new string[]
            {
                "1 imported box of chocolates at 10.00",
                "1 imported bottle of perfume at 47.50"

            };

            //Act
            ShoppingBasket shoppingBasket = _shoppingBasketCreator.CreateShoppingBasket(inputLines, _productCategories);
            IList<TaxedProduct> taxedProducts = _taxCalculator.ApplyTaxes(shoppingBasket);
            ReceiptDetail receiptDetail = _receiptDetailCreator.CreateReceiptDetail(taxedProducts);

            //Assert
            Assert.AreEqual($"1 imported box of chocolates: {importedBoxOfChocolatesPriceExpected}{Environment.NewLine}1 imported bottle of perfume: {importedBottleOfPerfumePriceExpected}{Environment.NewLine}Sales Taxes: {salesTaxExpected.ToString().PadRight(4, '0')}{Environment.NewLine}Total: {totalExpected}{Environment.NewLine}", receiptDetail.Receipt);
        }

        [Test]
        public void Input3()
        {
            //Arrange
            decimal importedBottleOfPerfumePriceExpected = 32.19m;
            decimal bottleOfPerfumePriceExpected = 20.89m;
            decimal packetOfHeadachePillsPriceExpected = 9.75m;
            decimal boxOfImportedChocolatesPriceExpected = 11.85m;
            decimal salesTaxExpected = 6.70m;
            decimal totalExpected = 74.68m;

            string[] inputLines = new string[]
            {
                "1 imported bottle of perfume at 27.99",
                "1 bottle of perfume at 18.99",
                "1 packet of headache pills at 9.75",
                "1 box of imported chocolates at 11.25"

            };

            //Act
            ShoppingBasket shoppingBasket = _shoppingBasketCreator.CreateShoppingBasket(inputLines, _productCategories);
            IList<TaxedProduct> taxedProducts = _taxCalculator.ApplyTaxes(shoppingBasket);
            ReceiptDetail receiptDetail = _receiptDetailCreator.CreateReceiptDetail(taxedProducts);

            //Assert
            Assert.AreEqual($"1 imported bottle of perfume: {importedBottleOfPerfumePriceExpected}{Environment.NewLine}1 bottle of perfume: {bottleOfPerfumePriceExpected}{Environment.NewLine}1 packet of headache pills: {packetOfHeadachePillsPriceExpected}{Environment.NewLine}1 imported box of chocolates: {boxOfImportedChocolatesPriceExpected}{Environment.NewLine}Sales Taxes: {salesTaxExpected.ToString().PadRight(4, '0')}{Environment.NewLine}Total: {totalExpected}{Environment.NewLine}", receiptDetail.Receipt);
        }

    }
}
