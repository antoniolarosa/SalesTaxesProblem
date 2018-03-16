using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SalesTaxes.Entities;
using SalesTaxes.Services;
using SalesTaxes.Services.TaxCalculation.Decorators;

namespace SalesTaxes.Fixtures
{
    [TestFixture]
    public class ReceiptDetailCreatorFixtures
    {
        private ReceiptDetailCreator _receiptDetailCreator;

        [SetUp]
        public void Initialize()
        {
            _receiptDetailCreator = new ReceiptDetailCreator();
        }

        [Test]
        public void CreateReceiptDetail_NoTaxedProducts_EmptyReceipt()
        {
            //Arrange
            IList<TaxedProduct> taxedProducts = new List<TaxedProduct>();

            //Act
            ReceiptDetail receiptDetail = _receiptDetailCreator.CreateReceiptDetail(taxedProducts);
            
            //Assert
            Assert.AreEqual("", receiptDetail.Receipt);
        }

        [Test]
        public void CreateReceiptDetail_NullTaxedProducts_ThrowException()
        {
            //Arrange
            IList<TaxedProduct> taxedProducts = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _receiptDetailCreator.CreateReceiptDetail(taxedProducts));
        }

        [Test]
        public void CreateReceiptDetail_1TaxedProductWithBasicSalesTax_CorrectReceipt()
        {
            //Arrange
            decimal grossAmount = 16.49m;
            decimal total = 16.49m;
            decimal salesTaxes = 1.50m;
            IList<TaxedProduct> taxedProducts = new List<TaxedProduct>()
            {
                new TaxedProduct(
                    new FlatTaxDecorator(
                        new Product(1, 14.99m , "1 music CD", "music CD", false, CategoryType.Other), 
                        "Basic Sales Tax", 0.1m ), grossAmount )
            };

            //Act
            ReceiptDetail receiptDetail = _receiptDetailCreator.CreateReceiptDetail(taxedProducts);

            //Assert
            Assert.AreEqual($"1 music CD: {grossAmount}{Environment.NewLine}" +
                            $"Sales Taxes: {salesTaxes}{Environment.NewLine}" +
                            $"Total: {total}{Environment.NewLine}", receiptDetail.Receipt);
            Assert.AreEqual(1, receiptDetail.TaxedProducts.Count);
            Assert.AreEqual("1 music CD, Basic Sales Tax", receiptDetail.TaxedProducts.Single().Product.GetDescription());
        }

        [Test]
        public void CreateReceiptDetail_2TaxedProducts_CorrectReceipt()
        {
            //Arrange
            decimal musicCdGrossAmount = 16.49m;
            decimal bookGrossAmount = 14.99m;
            decimal total = musicCdGrossAmount + bookGrossAmount;
            decimal salesTaxes = 4.00m;
            IList<TaxedProduct> taxedProducts = new List<TaxedProduct>()
            {
                new TaxedProduct(
                    new FlatTaxDecorator(
                        new Product(1, 14.99m , "1 music CD", "music CD", false, CategoryType.Other),
                        "Basic Sales Tax", 0.1m ), musicCdGrossAmount ),
                new TaxedProduct(
                    new FlatTaxDecorator(
                        new Product(1,12.49m, "1 book", "book", false, CategoryType.Books),
                        "Another tax",0.2m), bookGrossAmount)
            };

            //Act
            ReceiptDetail receiptDetail = _receiptDetailCreator.CreateReceiptDetail(taxedProducts);

            //Assert
            Assert.AreEqual($"1 music CD: {musicCdGrossAmount}{Environment.NewLine}" +
                            $"1 book: {bookGrossAmount}{Environment.NewLine}" +
                            $"Sales Taxes: {salesTaxes}{Environment.NewLine}" +
                            $"Total: {total}{Environment.NewLine}", receiptDetail.Receipt);
            Assert.AreEqual(2, receiptDetail.TaxedProducts.Count);
            Assert.AreEqual("1 music CD, Basic Sales Tax", receiptDetail.TaxedProducts.First().Product.GetDescription());
            Assert.AreEqual("1 book, Another tax", receiptDetail.TaxedProducts.Last().Product.GetDescription());
        }
    }
}
