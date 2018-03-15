using System;
using NUnit.Framework;
using SalesTaxes.Models;
using SalesTaxes.Services.Taxes.Decorators;
using SalesTaxes.Services.Taxes.Rules;

namespace SalesTaxes.Fixtures
{
    [TestFixture]
    public class ImportedTaxRuleFixtures
    {
        private ImportedTaxRule _importedTaxRule;

        [SetUp]
        public void Initialize()
        {
            _importedTaxRule = new ImportedTaxRule()
            {
                Description = "Import duty",
                Rate = 0.05m
            };
        }

        [Test]
        public void IsApplyiableTo_NoImportedProduct_IsNotApplyiable()
        {
            //Arrange
            IProduct product = new Product(1,1, "1 book", "book", false, CategoryType.Books);
            
            //Act
            bool isApplyiable = _importedTaxRule.IsApplyiableTo(product);

            //Assert
            Assert.IsFalse(isApplyiable);
        }

        [Test]
        public void IsApplyiableTo_ImportedProduct_IsApplyiable()
        {
            //Arrange
            IProduct product = new Product(1, 1, "1 imported book", "book", true, CategoryType.Books);

            //Act
            bool isApplyiable = _importedTaxRule.IsApplyiableTo(product);

            //Assert
            Assert.IsTrue(isApplyiable);
        }

        [Test]
        public void IsApplyiableTo_NullProduct_ThrowException()
        {
            //Arrange
            IProduct product = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _importedTaxRule.IsApplyiableTo(product));
        }

        [Test]
        public void GetTaxableProductDecorator_ImportedTaxRule_FlatTaxDecorator()
        {
            //Arrange
            IProduct product = new Product(1, 1, "1 imported book", "book", true, CategoryType.Books);

            //Act
            ProductDecorator productDecorator = _importedTaxRule.GetTaxableProductDecorator(product);

            //Assert
            Assert.AreEqual("1 imported book, Import duty", productDecorator.GetDescription());
            Assert.IsInstanceOf<FlatTaxDecorator>(productDecorator);
        }

        [Test]
        public void GetTaxableProductDecorator_NullProduct_ThrowException()
        {
            //Arrange
            IProduct product = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _importedTaxRule.GetTaxableProductDecorator(product));
        }
    }
}
