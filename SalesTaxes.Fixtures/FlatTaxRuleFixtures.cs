using System;
using System.Collections.Generic;
using NUnit.Framework;
using SalesTaxes.Models;
using SalesTaxes.Services.Taxes.Decorators;
using SalesTaxes.Services.Taxes.Rules;

namespace SalesTaxes.Fixtures
{
    [TestFixture]
    public class FlatTaxRuleFixtures
    {
        private FlatTaxRule _flatTaxRule;

        [SetUp]
        public void Initialize()
        {
            _flatTaxRule = new FlatTaxRule()
            {
                Description = "Basic Sales Tax",
                Rate = 0.1m
            };
        }

        [Test]
        public void IsApplyiableTo_ExcludedCategoryProduct_NotAppyiable()
        {
            //Arrange
            _flatTaxRule.ExcludedCategories = new HashSet<CategoryType>()
            {
                CategoryType.Books
            };
            IProduct product = new Product(1, 1, "1 book", "book", false, CategoryType.Books);

            //Act
            bool isApplyiable = _flatTaxRule.IsApplyiableTo(product);

            //Assert
            Assert.IsFalse(isApplyiable);
        }

        [Test]
        public void IsApplyiableTo_NotExcludedCategoryProduct_Appyiable()
        {
            //Arrange
            _flatTaxRule.ExcludedCategories = new HashSet<CategoryType>();
            IProduct product = new Product(1, 1, "1 book", "book", false, CategoryType.Books);

            //Act
            bool isApplyiable = _flatTaxRule.IsApplyiableTo(product);

            //Assert
            Assert.IsTrue(isApplyiable);
        }

        [Test]
        public void IsApplyiableTo_NullProduct_ThrowException()
        {
            //Arrange
            IProduct product = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _flatTaxRule.IsApplyiableTo(product));
        }

        [Test]
        public void GetTaxableProductDecorator_FlatTaxRule_FlatTaxDecorator()
        {
            //Arrange
            IProduct product = new Product(1, 1, "1 imported book", "book", true, CategoryType.Books);

            //Act
            ProductDecorator productDecorator = _flatTaxRule.GetTaxableProductDecorator(product);

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
            Assert.Throws<ArgumentNullException>(() => _flatTaxRule.GetTaxableProductDecorator(product));
        }
    }
}
