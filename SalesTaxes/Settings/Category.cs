using System.Collections.Generic;
using SalesTaxes.Models;

namespace SalesTaxes.Settings
{
    public class Category
    {
        public CategoryType CategoryType { get; set; }
        public IList<string> ProductNames { get; set; } = new List<string>();
    }
}