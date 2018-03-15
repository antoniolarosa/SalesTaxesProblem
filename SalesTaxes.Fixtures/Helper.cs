using System.IO;
using Microsoft.Extensions.Configuration;

namespace SalesTaxes.Fixtures
{
    public static class Helper
    {
        public static IConfigurationRoot GetConfigurationRoot(string jsonFile)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(jsonFile);

            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }
    }
}
