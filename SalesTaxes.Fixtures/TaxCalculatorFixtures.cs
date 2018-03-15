using System;
using System.Collections.Generic;
using NUnit.Framework;
using SalesTaxes.Models;
using SalesTaxes.Services.Taxes;
using SalesTaxes.Services.Taxes.Rules;

namespace SalesTaxes.Fixtures
{
    [TestFixture]
    public class TaxCalculatorFixtures
    {
        private TaxCalculator _taxCalculator;

        [SetUp]
        public void Initialize()
        {
            IList<TaxRule> taxRules = new List<TaxRule>()
            {
                new FlatTaxRule()
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
                new ImportedTaxRule()
                {
                    Description = "Import duty",
                    Rate = 0.05m
                }
            };
            _taxCalculator = new TaxCalculator(taxRules);
        }

        [Test]
        public void ApplyTaxes_EmptyShoppingBasket_NoTaxedProducts()
        {
            //Arrange
            var shoppingBasket = new ShoppingBasket(new List<IProduct>());
            
            //Act
            IList<TaxedProduct> taxedProducts = _taxCalculator.ApplyTaxes(shoppingBasket);

            //Assert
            Assert.AreEqual(0, taxedProducts.Count);
        }

        [Test]
        public void ApplyTaxes_NullShoppingBasket_ThrowException()
        {
            //Arrange
            ShoppingBasket shoppingBasket = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() =>  _taxCalculator.ApplyTaxes(shoppingBasket));
        }

        [Test]
        public void ApplyTaxes_Input1_CorrectTaxedProducts()
        {
            //Arrange
            var shoppingBasket = new ShoppingBasket(new List<IProduct>()
            {
                new Product(1, 12.49m, "1 book", "book", false, CategoryType.Books),
                new Product(1, 14.99m, "1 music CD", "music CD", false, CategoryType.Other),
                new Product(1, 0.85m, "1 chocolate bar", "chocolate bar", false, CategoryType.Foods)
            });
           
            //Act
            IList<TaxedProduct> taxedProducts = _taxCalculator.ApplyTaxes(shoppingBasket);

            //Assert
            Assert.AreEqual(3, taxedProducts.Count);
            Assert.AreEqual("1 book", taxedProducts[0].Product.GetDescription());
            Assert.AreEqual(12.49m, taxedProducts[0].GrossAmount);
            Assert.AreEqual(0m, taxedProducts[0].Tax);
            Assert.AreEqual("1 music CD, Basic Sales Tax", taxedProducts[1].Product.GetDescription());
            Assert.AreEqual(16.49m, taxedProducts[1].GrossAmount);
            Assert.AreEqual(1.5m, taxedProducts[1].Tax);
            Assert.AreEqual("1 chocolate bar", taxedProducts[2].Product.GetDescription());
            Assert.AreEqual(0.85m, taxedProducts[2].GrossAmount);
            Assert.AreEqual(0m, taxedProducts[2].Tax);
        }

        [Test]
        public void ApplyTaxes_Input2_CorrectTaxedProducts()
        {
            //Arrange
            ShoppingBasket shoppingBasket = new ShoppingBasket(new List<IProduct>()
            {
                new Product(1, 10m, "1 imported box of chocolates", "box of chocolates", true, CategoryType.Foods),
                new Product(1, 47.50m, "1 imported bottle of perfume", "bottle of perfume", true, CategoryType.Other)
            });

            //Act
            IList<TaxedProduct> taxedProducts = _taxCalculator.ApplyTaxes(shoppingBasket);

            //Assert
            Assert.AreEqual(2, taxedProducts.Count);
            Assert.AreEqual("1 imported box of chocolates, Import duty", taxedProducts[0].Product.GetDescription());
            Assert.AreEqual(10.50m, taxedProducts[0].GrossAmount);
            Assert.AreEqual(0.5m, taxedProducts[0].Tax);
            Assert.AreEqual("1 imported bottle of perfume, Basic Sales Tax, Import duty", taxedProducts[1].Product.GetDescription());
            Assert.AreEqual(54.65m, taxedProducts[1].GrossAmount);
            Assert.AreEqual(7.15m, taxedProducts[1].Tax);
        }

        [Test]
        public void ApplyTaxes_Input3_CorrectTaxedProducts()
        {
            //Arrange
            var shoppingBasket = new ShoppingBasket(new List<IProduct>()
            {
                new Product(1, 27.99m, "1 imported bottle of perfume", "bottle of perfume", true, CategoryType.Other),
                new Product(1, 18.99m, "1 bottle of perfume", "bottle of perfume", false,CategoryType.Other),
                new Product(1, 9.75m, "1 packet of headache pills", "packet of headache pills", false, CategoryType.MedicalProducts),
                new Product(1, 11.25m, "1 box of imported chocolates", "box of chocolates", true, CategoryType.Foods),
            });

            //Act
            IList<TaxedProduct> taxedProducts = _taxCalculator.ApplyTaxes(shoppingBasket);

            //Assert
            Assert.AreEqual(4, taxedProducts.Count);
            Assert.AreEqual("1 imported bottle of perfume, Basic Sales Tax, Import duty", taxedProducts[0].Product.GetDescription());
            Assert.AreEqual(32.19m, taxedProducts[0].GrossAmount);
            Assert.AreEqual(4.2m, taxedProducts[0].Tax);
            Assert.AreEqual("1 bottle of perfume, Basic Sales Tax", taxedProducts[1].Product.GetDescription());
            Assert.AreEqual(20.89m, taxedProducts[1].GrossAmount);
            Assert.AreEqual(1.9m, taxedProducts[1].Tax);
            Assert.AreEqual("1 packet of headache pills", taxedProducts[2].Product.GetDescription());
            Assert.AreEqual(9.75m, taxedProducts[2].GrossAmount);
            Assert.AreEqual(0m, taxedProducts[2].Tax);
            Assert.AreEqual("1 box of imported chocolates, Import duty", taxedProducts[3].Product.GetDescription());
            Assert.AreEqual(11.85m, taxedProducts[3].GrossAmount);
            Assert.AreEqual(0.6m, taxedProducts[3].Tax);
        }
    }
}
