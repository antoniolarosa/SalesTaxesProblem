using System;
using NUnit.Framework;
using SalesTaxes.Entities;
using SalesTaxes.Services.TaxCalculation.Decorators;
using SalesTaxes.Services.TaxCalculation.Taxes;

namespace SalesTaxes.Fixtures
{
    [TestFixture]
    public class ImportedTaxFixtures
    {
        private ImportedTax _importedTax;

        [SetUp]
        public void Initialize()
        {
            _importedTax = new ImportedTax()
            {
                Description = "Import duty",
                Rate = 0.05m
            };
        }

        [Test]
        public void IsApplicableFor_NoImportedProduct_IsNotApplyiable()
        {
            //Arrange
            IProduct product = new Product(1,1, "1 book", "book", false, CategoryType.Books);
            
            //Act
            bool isApplyiable = _importedTax.IsApplicableFor(product);

            //Assert
            Assert.IsFalse(isApplyiable);
        }

        [Test]
        public void IsApplicableFor_ImportedProduct_IsApplyiable()
        {
            //Arrange
            IProduct product = new Product(1, 1, "1 imported book", "book", true, CategoryType.Books);

            //Act
            bool isApplyiable = _importedTax.IsApplicableFor(product);

            //Assert
            Assert.IsTrue(isApplyiable);
        }

        [Test]
        public void IsApplicableFor_NullProduct_ThrowException()
        {
            //Arrange
            IProduct product = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _importedTax.IsApplicableFor(product));
        }

        [Test]
        public void GetTaxableProductDecorator_ImportedTax_FlatTaxDecorator()
        {
            //Arrange
            IProduct product = new Product(1, 1, "1 imported book", "book", true, CategoryType.Books);

            //Act
            ProductDecorator productDecorator = _importedTax.GetTaxableProductDecorator(product);

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
            Assert.Throws<ArgumentNullException>(() => _importedTax.GetTaxableProductDecorator(product));
        }
    }
}
