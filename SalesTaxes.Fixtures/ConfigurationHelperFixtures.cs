using System.Linq;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SalesTaxes.Entities;
using SalesTaxes.Services.Taxes.Rules;
using SalesTaxes.Settings;

namespace SalesTaxes.Fixtures
{
    [TestFixture]
    public class ConfigurationHelperFixtures
    {
        private ConfigurationHelper _configurationHelper;

        [SetUp]
        public void Initialize()
        {
            _configurationHelper = new ConfigurationHelper();
        }

        [Test]
        public void GetTaxSettings_AppSettingsWithFlatTaxes_MappedFlatTaxes()
        {
            //Arrange
            IConfigurationRoot configurationRoot = Helper.GetConfigurationRoot("appsettingsWithFlatTaxes.json");

            //Act
            TaxSettings taxSettings = _configurationHelper.GetTaxSettings(configurationRoot);

            //Assert
            Assert.AreEqual(0, taxSettings.ImportedTaxes.Count);
            Assert.AreEqual(1, taxSettings.FlatTaxes.Count);

            FlatTax flatTax = taxSettings.FlatTaxes.Single();
            Assert.AreEqual(0.1m, flatTax.Rate);
            Assert.AreEqual("Basic Sales Tax", flatTax.Description);
            Assert.AreEqual(3, flatTax.ExcludedCategories.Count);
            Assert.AreEqual(CategoryType.Books, flatTax.ExcludedCategories.First());
            Assert.AreEqual(CategoryType.Foods, flatTax.ExcludedCategories.Skip(1).First());
            Assert.AreEqual(CategoryType.MedicalProducts, flatTax.ExcludedCategories.Skip(2).First());
        }
    }
}
