# Sales taxes problem [![master](https://travis-ci.org/antoniolarosa/SalesTaxesProblem.svg?branch=master)](https://travis-ci.org/antoniolarosa/SalesTaxesProblem)

- [Description](#description)
- [Approach to the problem](#approach-to-the-problem)

## Description

Basic sales tax is applicable at a rate of 10% on all goods, except books, food, and medical products that are exempt. Import duty is an additional sales tax applicable on all imported goods at a rate of 5%, with no exemptions.

When I purchase items I receive a receipt which lists the name of all the items and their price (including tax), finishing with the total cost of the items, and the total amounts of sales taxes paid. The rounding rules for sales tax are that for a tax rate of n%, a shelf price of p contains (np/100 rounded up to the nearest 0.05) amount of sales tax.

Write an application that prints out the receipt details for these shopping baskets...

```
INPUT:

Input 1:
1 book at 12.49
1 music CD at 14.99
1 chocolate bar at 0.85

Input 2:
1 imported box of chocolates at 10.00
1 imported bottle of perfume at 47.50

Input 3:
1 imported bottle of perfume at 27.99
1 bottle of perfume at 18.99
1 packet of headache pills at 9.75
1 box of imported chocolates at 11.25
```

```
OUTPUT

Output 1:
1 book : 12.49
1 music CD: 16.49
1 chocolate bar: 0.85
Sales Taxes: 1.50
Total: 29.83

Output 2:
1 imported box of chocolates: 10.50
1 imported bottle of perfume: 54.65
Sales Taxes: 7.65
Total: 65.15

Output 3:
1 imported bottle of perfume: 32.19
1 bottle of perfume: 20.89
1 packet of headache pills: 9.75
1 imported box of chocolates: 11.85
Sales Taxes: 6.70
Total: 74.68
```

## Approach to the problem

The core of this software is the engine for tax calculations. There are a lot of responsabilities to handle so I'll explain how I split them in different classes.

I add to a target product the logic of tax calculation at runtime with the **Decorator pattern**.
So a concrete decorator has the responsability to handle business logic for tax calculation.

I create Tax base class for handling other 2 responsabilities:
1) Logic for tax applicability
2) Instantiate the right concrete decorator

```c#
public abstract class Tax
{
    public string Description { get; set; }
    public abstract bool IsApplicableFor(IProduct product);
    public abstract ProductDecorator GetTaxableProductDecorator(IProduct product);
}
```
 
As an example this is an extension of `Tax`:

```c#
public class ImportedTax : Tax
{
    public decimal Rate { get; set; }

    public override bool IsApplicableFor(IProduct product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        return product.IsImported;
    }

    public override ProductDecorator GetTaxableProductDecorator(IProduct product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));

        return new FlatTaxDecorator(product, Description, Rate);
    }
}
```

Then I need a `TaxCalculator` which orchestrates everything:

```c#
public IList<TaxedProduct> ApplyTaxes(ShoppingBasket shoppingBasket)
{
    if (shoppingBasket == null) throw new ArgumentNullException(nameof(shoppingBasket));

    IList<TaxedProduct> taxedProducts = new List<TaxedProduct>();
    for (var i = 0; i < shoppingBasket.Products.Count; i++)
    {
        IProduct product = shoppingBasket.Products[i];
        foreach (Tax taxRule in _taxRules)
        {
            if (taxRule.IsApplicableFor(product))
            {
                ProductDecorator productDecorator = taxRule.GetTaxableProductDecorator(product);
                product = productDecorator;
            }
        }
        decimal grossAmount = product.CalculateGrossAmount();
        taxedProducts.Add(new TaxedProduct(product, grossAmount));
    }
    return taxedProducts;
}
```

Running time analysis in this method gives us a `O(number of taxes * number of products in shopping cart)`, which is a linear running time if we consider that number of taxes is constant:

```
O(constant * number of products) = O(number of products)
```

Note that I don't want to make a new deploy if the rate of some tax change. That's why some information are configurable in appsettings.json



Now I can handle a lot of use cases.

- What if I want to change the rate of a tax?
	I can update the appsettings.json. No deploy needed.

- What if I want to remove a category from excluded categories in Basic Sales Tax?
	I can update the appsettings.json. No deploy needed.

- What if I want to add a category?
	I can't just update the appsettings.json because every cagegory is mapped to CategoryType. Each category is not a string but an enumeration, something more than a string due to his business value.

- What if I want to add a new tax of the same type (same is applyiable logic, same concrete decoretor, different description and parameters) of existing one?
	I can update the appsettings.json. No deploy needed.

- What is I want to insert a new tax?
	a) add a new section in appsettings.json/taxes
	b) update the TaxSettings class
	c) Create a new class thta inherit from TaxRule

The only class that we open is TaxSettings. We don't touch the tax calculator engine. We just create new classes.
With this organization I'm following the open/close principle.
My software is close for modification and open for extension.

I can test with decorator

