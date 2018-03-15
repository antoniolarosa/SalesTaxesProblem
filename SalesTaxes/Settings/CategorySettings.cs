using System.Collections.Generic;

namespace SalesTaxes.Settings
{
    public class CategorySettings
    {
        public IList<Category> Categories { get; set; } = new List<Category>();
    }
}
