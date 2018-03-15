using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using SalesTaxes.Models;

namespace SalesTaxes.Settings
{
    public class ConfigurationHelper
    {
        public  IConfigurationRoot GetConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }

        public TaxSettings GetTaxSettings(IConfigurationRoot configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var taxSettings = new TaxSettings();
            configuration.GetSection(Constants.AppSettings.Taxes).Bind(taxSettings);
            return taxSettings;
        }

        public IDictionary<string, CategoryType> GetProductCategories(IConfigurationRoot configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            IList<Category> categories = new List<Category>();
            configuration.GetSection(Constants.AppSettings.Categories).Bind(categories);

            IDictionary<string, CategoryType> productCategories = new Dictionary<string, CategoryType>();
            foreach (Category category in categories)
            {
                foreach (string productName in category.ProductNames)
                {
                    productCategories.Add(productName, category.CategoryType);
                }
            }

            return productCategories;
        }
    }
}
