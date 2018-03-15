using System.Collections.Generic;

namespace SalesTaxes.Entities
{
    public class ShoppingBasket
    {
        public IList<IProduct> Products { get; }

        public ShoppingBasket(IList<IProduct> products)
        {
            Products = products;
        }
    }
}
