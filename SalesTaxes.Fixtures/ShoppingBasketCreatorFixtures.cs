using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SalesTaxes.Models;
using SalesTaxes.Services;

namespace SalesTaxes.Fixtures
{
    [TestFixture]
    public class ShoppingBasketCreatorFixtures
    {
        private ShoppingBasketCreator _shoppingBasketCreator;

        [SetUp]
        public void Initialize()
        {
            _shoppingBasketCreator = new ShoppingBasketCreator();
        }

        [Test]
        public void CreateShoppingBasket_0InputLine_EmptyShoppingBasket()
        {
            //Arrange
            string[] inputLines = new string [0];
            IDictionary< string, CategoryType > productCategories = new Dictionary<string, CategoryType>();

            //Act
            ShoppingBasket shoppingBasket = _shoppingBasketCreator.CreateShoppingBasket(inputLines, productCategories);

            //Assert
            Assert.AreEqual(0, shoppingBasket.Products.Count);
        }

        [Test]
        public void CreateShoppingBasket_NullInputLines_ThrowException()
        {
            //Arrange
            string[] inputLines = null;
            IDictionary<string, CategoryType> productCategories = new Dictionary<string, CategoryType>();

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _shoppingBasketCreator.CreateShoppingBasket(inputLines, productCategories));
        }

        [Test]
        public void CreateShoppingBasket_NullProductCategories_ThrowException()
        {
            //Arrange
            string[] inputLines =  new string[0];
            IDictionary<string, CategoryType> productCategories = null;

            //Act and Assert
            Assert.Throws<ArgumentNullException>(() => _shoppingBasketCreator.CreateShoppingBasket(inputLines, productCategories));
        }

        [Test]
        public void CreateShoppingBasket_1InputLineWith1Product_1ProductInShoppingBasket()
        {
            //Arrange
            string[] inputLines = new string[]
            {
                "1 productName at 12.49"
            };
            IDictionary<string, CategoryType> productCategories = new Dictionary<string, CategoryType>();

            //Act
            ShoppingBasket shoppingBasket = _shoppingBasketCreator.CreateShoppingBasket(inputLines, productCategories);

            //Assert
            Assert.AreEqual(1, shoppingBasket.Products.Count);

            IProduct product = shoppingBasket.Products.Single();

            Assert.AreEqual(1, product.Quantity);
            Assert.AreEqual("productName", product.ProductName);
            Assert.AreEqual("1 productName", product.Description);
            Assert.AreEqual(CategoryType.Other, product.Category);
            Assert.AreEqual(false, product.IsImported);
            Assert.AreEqual(12.49, product.Price);
            Assert.AreEqual("1 productName", product.GetDescription());
            Assert.AreEqual(12.49, product.CalculateGrossAmount());
        }

        [Test]
        public void CreateShoppingBasket_1InputLineWith1Book_1BookInShoppingBasket()
        {
            //Arrange
            string[] inputLines = new string[]
            {
                "1 book at 12.49"
            };
            IDictionary<string, CategoryType> productCategories = new Dictionary<string, CategoryType>()
            {
                {"book", CategoryType.Books}
            };

            //Act
            ShoppingBasket shoppingBasket = _shoppingBasketCreator.CreateShoppingBasket(inputLines, productCategories);

            //Assert
            Assert.AreEqual(1, shoppingBasket.Products.Count);

            IProduct product = shoppingBasket.Products.Single();

            Assert.AreEqual(1, product.Quantity);
            Assert.AreEqual("book", product.ProductName);
            Assert.AreEqual("1 book", product.Description);
            Assert.AreEqual(CategoryType.Books, product.Category);
            Assert.AreEqual(false, product.IsImported);
            Assert.AreEqual(12.49, product.Price);
            Assert.AreEqual("1 book", product.GetDescription());
            Assert.AreEqual(12.49, product.CalculateGrossAmount());
        }

        [Test]
        public void CreateShoppingBasket_1InputLineWith1ImportedBook_1ImportedBookInShoppingBasket()
        {
            //Arrange
            string[] inputLines = new string[]
            {
                "1 imported book at 12.49"
            };
            IDictionary<string, CategoryType> productCategories = new Dictionary<string, CategoryType>()
            {
                {"book", CategoryType.Books}
            };

            //Act
            ShoppingBasket shoppingBasket = _shoppingBasketCreator.CreateShoppingBasket(inputLines, productCategories);

            //Assert
            Assert.AreEqual(1, shoppingBasket.Products.Count);

            IProduct product = shoppingBasket.Products.Single();

            Assert.AreEqual(1, product.Quantity);
            Assert.AreEqual("book", product.ProductName);
            Assert.AreEqual("1 imported book", product.Description);
            Assert.AreEqual(CategoryType.Books, product.Category);
            Assert.AreEqual(true, product.IsImported);
            Assert.AreEqual(12.49, product.Price);
            Assert.AreEqual("1 imported book", product.GetDescription());
            Assert.AreEqual(12.49, product.CalculateGrossAmount());
        }

        [Test]
        public void CreateShoppingBasket_2InputLinesWith1ImportedBookAnd2Cakes_2ProductsInShoppingBasket()
        {
            //Arrange
            string[] inputLines = new string[]
            {
                "1 imported book at 12.49",
                "2 cake at 10",
            };
            IDictionary<string, CategoryType> productCategories = new Dictionary<string, CategoryType>()
            {
                {"book", CategoryType.Books},
                {"cake", CategoryType.Foods }
            };

            //Act
            ShoppingBasket shoppingBasket = _shoppingBasketCreator.CreateShoppingBasket(inputLines, productCategories);

            //Assert
            Assert.AreEqual(2, shoppingBasket.Products.Count);

            IProduct book = shoppingBasket.Products.First();

            Assert.AreEqual(1, book.Quantity);
            Assert.AreEqual("book", book.ProductName);
            Assert.AreEqual("1 imported book", book.Description);
            Assert.AreEqual(CategoryType.Books, book.Category);
            Assert.AreEqual(true, book.IsImported);
            Assert.AreEqual(12.49, book.Price);
            Assert.AreEqual("1 imported book", book.GetDescription());
            Assert.AreEqual(12.49, book.CalculateGrossAmount());

            IProduct cake = shoppingBasket.Products.Last();

            Assert.AreEqual(2, cake.Quantity);
            Assert.AreEqual("cake", cake.ProductName);
            Assert.AreEqual("2 cake", cake.Description);
            Assert.AreEqual(CategoryType.Foods, cake.Category);
            Assert.AreEqual(false, cake.IsImported);
            Assert.AreEqual(20, cake.Price);
            Assert.AreEqual("2 cake", cake.GetDescription());
            Assert.AreEqual(20, cake.CalculateGrossAmount());
        }
    }
}
