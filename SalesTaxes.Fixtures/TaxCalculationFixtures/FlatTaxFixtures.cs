using System;
using System.Collections.Generic;
using NUnit.Framework;
using SalesTaxes.Entities;
using SalesTaxes.Services.TaxCalculation.Decorators;
using SalesTaxes.Services.TaxCalculation.Taxes;

namespace SalesTaxes.Fixtures.TaxCalculationFixtures
{
    [TestFixture]
    public class FlatTaxFixtures
    {
        private FlatTax _flatTax;

        [SetUp]
        public void Initialize()
        {
            _flatTax = new FlatTax()
            {
                Description = "Basic Sales Tax",
                Rate = 0.1m
            };
        }

        [Test]
        public void IsApplicableFor_ExcludedCategoryProduct_NotAppyiable()
        {
            //Arrange
            _flatTax.ExcludedCategories = new HashSet<CategoryType>()
            {
                CategoryType.Books
            };
            IProduct product = new Product(1, 1, "1 book", "book", false, CategoryType.Books);

            //Act
            bool isApplyiable = _flatTax.IsApplicableFor(product);

            //Assert
            Assert.IsFalse(isApplyiable);
        }

        [Test]
        public void IsApplicableFor_NotExcludedCategoryProduct_Appyiable()
        {
            //Arrange
            _flatTax.ExcludedCategories = new HashSet<CategoryType>();
            IProduct product = new Product(1, 1, "1 book", "book", false, CategoryType.Books);

            //Act
            bool isApplyiable = _flatTax.IsApplicableFor(product);

            //Assert
            Assert.IsTrue(isApplyiable);
        }

        [Test]
        public void IsApplicableFor_NullProduct_ThrowException()
        {
            //Arrange
            IProduct product = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _flatTax.IsApplicableFor(product));
        }

        [Test]
        public void GetTaxableProductDecorator_FlatTax_FlatTaxDecorator()
        {
            //Arrange
            IProduct product = new Product(1, 1, "1 imported book", "book", true, CategoryType.Books);

            //Act
            ProductDecorator productDecorator = _flatTax.GetTaxableProductDecorator(product);

            //Assert
            Assert.AreEqual("1 imported book, Basic Sales Tax", productDecorator.GetDescription());
            Assert.IsInstanceOf<FlatTaxDecorator>(productDecorator);
        }

        [Test]
        public void GetTaxableProductDecorator_NullProduct_ThrowException()
        {
            //Arrange
            IProduct product = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _flatTax.GetTaxableProductDecorator(product));
        }
    }
}
