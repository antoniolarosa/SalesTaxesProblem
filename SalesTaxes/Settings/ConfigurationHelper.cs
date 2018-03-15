using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SalesTaxes.Entities;

namespace SalesTaxes.Settings
{
    public class ConfigurationHelper
    {
        private readonly IConfigurationRoot _configuration;

        public ConfigurationHelper(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public TaxSettings GetTaxSettings()
        {
            var taxSettings = new TaxSettings();
            _configuration.GetSection(Constants.AppSettings.Taxes).Bind(taxSettings);
            return taxSettings;
        }

        public IDictionary<string, CategoryType> GetProductCategories()
        {
            IList<Category> categories = new List<Category>();
            _configuration.GetSection(Constants.AppSettings.Categories).Bind(categories);

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
