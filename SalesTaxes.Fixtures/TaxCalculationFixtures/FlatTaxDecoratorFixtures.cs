using NUnit.Framework;
using SalesTaxes.Entities;
using SalesTaxes.Services.TaxCalculation.Decorators;

namespace SalesTaxes.Fixtures.TaxCalculationFixtures
{
    [TestFixture]
    public class FlatTaxDecoratorFixtures
    {
        [Test]
        public void CalculateGrossAmount_TaxUnder0Dot05_CorrectGrossAmountWithTaxRoundedUpToTheNearestFiveCent()
        {
            //Arrange
            IProduct product = new Product(1, 12.49m, "1 product", "product", false, CategoryType.Books);
            var flatTaxDecorator = new FlatTaxDecorator(product, "Tax description", 0.1m);

            //Act
            decimal grossAmount = flatTaxDecorator.CalculateGrossAmount();

            //Assert
            //tax: 1.249 --> 1.25
            //tax + price = 1.25 + 12.49 = 13.74
            Assert.AreEqual(13.74m, grossAmount);
        }

        [Test]
        public void CalculateGrossAmount_TaxOver0Dot05_CorrectGrossAmountWithTaxRoundedUpToTheNearestFiveCent()
        {
            //Arrange
            IProduct product = new Product(1, 12.51m, "1 product", "product", false, CategoryType.Books);
            var flatTaxDecorator = new FlatTaxDecorator(product, "Tax description", 0.1m);

            //Act
            decimal grossAmount = flatTaxDecorator.CalculateGrossAmount();

            //Assert
            //tax: 1.251 --> 1.3
            //tax + price = 1.3 + 12.51 = 13.81
            Assert.AreEqual(13.81m, grossAmount);
        }

        [Test]
        public void GetDescription_1FlatTaxDecorator_CorrectDescription()
        {
            //Arrange
            IProduct product = new Product(1, 12.49m, "1 product", "product", false, CategoryType.Books);
            var flatTaxDecorator = new FlatTaxDecorator(product, "Tax description", 0.1m);

            //Act
            var description = flatTaxDecorator.GetDescription();

            //Assert
            Assert.AreEqual("1 product, Tax description", description);
        }

        [Test]
        public void GetDescription_2FlatTaxDecorators_CorrectDescription()
        {
            //Arrange
            IProduct product = new Product(1, 12.49m, "1 product", "product", false, CategoryType.Books);
            var flatTaxDecorator1 = new FlatTaxDecorator(product, "Tax description", 0.1m);
            var flatTaxDecorator2 = new FlatTaxDecorator(flatTaxDecorator1, "another Tax description", 0.05m);

            //Act
            var description = flatTaxDecorator2.GetDescription();

            //Assert
            Assert.AreEqual("1 product, Tax description, another Tax description", description);
        }
    }
}
