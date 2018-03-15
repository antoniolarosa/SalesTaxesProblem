using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SalesTaxes.Models;

namespace SalesTaxes.Services
{
    public class ShoppingBasketCreator
    {
        public ShoppingBasket CreateShoppingBasket(string[] inputLines, IDictionary<string, CategoryType> productCategories)
        {
            if (inputLines == null) throw new ArgumentNullException(nameof(inputLines));
            if (productCategories == null) throw new ArgumentNullException(nameof(productCategories));

            IList<IProduct> taxableProducts = new List<IProduct>();

            foreach (string line in inputLines)
            {
                string[] splittedLine = line.Split(" ");
                int quantity = GetQuantity(splittedLine);
                decimal price = GetPrice(splittedLine);
                string order = GetOrder(splittedLine);
                bool isImported = order.Contains(Constants.Imported);

                CategoryType category = CategoryType.Other;
                string productName = order.Replace($"{Constants.Imported} ", "");
                if (productCategories.ContainsKey(productName))
                {
                    category = productCategories[productName];
                }

                string description = GetDescription(splittedLine);

                IProduct product = new Product(quantity, price, description, productName, isImported, category);
                taxableProducts.Add(product);
            }

            return new ShoppingBasket(taxableProducts);
        }

        private static string GetDescription(string[] splittedLine)
        {
            //1 book at 12.49
            int atAndPriceItems = 2;
            return String.Join(" ", splittedLine.Take(splittedLine.Length - atAndPriceItems));
        }

        private static string GetOrder(string[] splittedLine)
        {
            //1 imported book at 12.49
            int quantityPosition = 1;
            int quantityAndAtAndPriceItems = 3;
            string order = String.Join(" ", splittedLine.Skip(quantityPosition).Take(splittedLine.Length - quantityAndAtAndPriceItems));

            return order; //for example: imported book
        }

        private static decimal GetPrice(string[] splittedLine)
        {
            return decimal.Parse(splittedLine.Last(), CultureInfo.InvariantCulture);
        }

        private static int GetQuantity(string[] splittedLine)
        {
            string quantityAsString = splittedLine[0];
            return Int32.Parse(quantityAsString);
        }
    }
}
