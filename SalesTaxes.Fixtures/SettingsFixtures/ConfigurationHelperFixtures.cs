﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SalesTaxes.Entities;
using SalesTaxes.Services.TaxCalculation.Taxes;
using SalesTaxes.Settings;

namespace SalesTaxes.Fixtures.SettingsFixtures
{
    [TestFixture]
    public class ConfigurationHelperFixtures
    {
        [Test]
        public void GetTaxSettings_AppSettingsWithFlatTaxes_MappedFlatTaxes()
        {
            //Arrange
            IConfigurationRoot configurationRoot = Helper.GetConfigurationRoot(Path.Combine("SettingsFixtures", "appsettingsWithFlatTaxes.json"));
            ConfigurationHelper configurationHelper = new ConfigurationHelper(configurationRoot);

            //Act
            TaxSettings taxSettings = configurationHelper.GetTaxSettings();

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

            IList<Tax> taxes = taxSettings.GetAllTaxes();
            Assert.AreEqual(1, taxes.Count);

            Tax tax = taxes.Single();
            Assert.AreEqual("Basic Sales Tax", tax.Description);
        }

        [Test]
        public void GetTaxSettings_AppSettingsWithImportedTaxes_MappedImportedTaxes()
        {
            //Arrange
            IConfigurationRoot configurationRoot = Helper.GetConfigurationRoot(Path.Combine("SettingsFixtures", "appsettingsWithImportedTaxes.json"));
            ConfigurationHelper configurationHelper = new ConfigurationHelper(configurationRoot);

            //Act
            TaxSettings taxSettings = configurationHelper.GetTaxSettings();

            //Assert
            Assert.AreEqual(1, taxSettings.ImportedTaxes.Count);
            Assert.AreEqual(0, taxSettings.FlatTaxes.Count);

            ImportedTax importedTax = taxSettings.ImportedTaxes.Single();
            Assert.AreEqual(0.05m, importedTax.Rate);
            Assert.AreEqual("Import duty", importedTax.Description);

            IList<Tax> taxes = taxSettings.GetAllTaxes();
            Assert.AreEqual(1, taxes.Count);

            Tax tax = taxes.Single();
            Assert.AreEqual("Import duty", tax.Description);
        }

        [Test]
        public void GetTaxSettings_AppsettingsWithTaxes_MappedTaxes()
        {
            //Arrange
            IConfigurationRoot configurationRoot = Helper.GetConfigurationRoot(Path.Combine("SettingsFixtures", "appsettingsWithTaxes.json"));
            ConfigurationHelper configurationHelper = new ConfigurationHelper(configurationRoot);

            //Act
            TaxSettings taxSettings = configurationHelper.GetTaxSettings();

            //Assert
            Assert.AreEqual(1, taxSettings.ImportedTaxes.Count);
            Assert.AreEqual(1, taxSettings.FlatTaxes.Count);

            FlatTax flatTax = taxSettings.FlatTaxes.Single();
            Assert.AreEqual(0.1m, flatTax.Rate);
            Assert.AreEqual("Basic Sales Tax", flatTax.Description);
            Assert.AreEqual(3, flatTax.ExcludedCategories.Count);
            Assert.AreEqual(CategoryType.Books, flatTax.ExcludedCategories.First());
            Assert.AreEqual(CategoryType.Foods, flatTax.ExcludedCategories.Skip(1).First());
            Assert.AreEqual(CategoryType.MedicalProducts, flatTax.ExcludedCategories.Skip(2).First());

            ImportedTax importedTax = taxSettings.ImportedTaxes.Single();
            Assert.AreEqual(0.05m, importedTax.Rate);
            Assert.AreEqual("Import duty", importedTax.Description);

            IList<Tax> taxes = taxSettings.GetAllTaxes();
            Assert.AreEqual(2, taxes.Count);
          
            Assert.AreEqual("Import duty", taxes.Last().Description);
            Assert.AreEqual("Basic Sales Tax", taxes.First().Description);
        }

        [Test]
        public void GetProductCategories()
        {
            //Arrange
            IConfigurationRoot configurationRoot = Helper.GetConfigurationRoot(Path.Combine("SettingsFixtures", "appsettingsWithCategories.json"));
            ConfigurationHelper configurationHelper = new ConfigurationHelper(configurationRoot);

            //Act
            IDictionary<string, CategoryType> productCategories = configurationHelper.GetProductCategories();

            //Assert
            Assert.AreEqual(4, productCategories.Count);

            Assert.AreEqual("book", productCategories.First().Key);
            Assert.AreEqual(CategoryType.Books, productCategories.First().Value);
            Assert.AreEqual("box of chocolates", productCategories.Skip(1).First().Key);
            Assert.AreEqual(CategoryType.Foods, productCategories.Skip(1).First().Value);
            Assert.AreEqual("chocolate bar", productCategories.Skip(2).First().Key);
            Assert.AreEqual(CategoryType.Foods, productCategories.Skip(2).First().Value);
            Assert.AreEqual("packet of headache pills", productCategories.Skip(3).First().Key);
            Assert.AreEqual(CategoryType.MedicalProducts, productCategories.Skip(3).First().Value);
        }
    }
}
