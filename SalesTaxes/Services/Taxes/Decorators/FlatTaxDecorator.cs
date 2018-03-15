using System;
using SalesTaxes.Entities;

namespace SalesTaxes.Services.Taxes.Decorators
{
    public class FlatTaxDecorator : ProductDecorator
    {
        private readonly decimal _rate;

        public FlatTaxDecorator(IProduct product, string description, decimal rate) : base(product)
        {
            if (rate == 0)
            {
                throw new ArgumentException($"rate cannot be zero");
            }
            _rate = rate;
            Description = description;
        }

        public override string GetDescription()
        {
            return $"{Product.GetDescription()}, {Description}";
        }

        public override decimal CalculateGrossAmount()
        {
            return Product.CalculateGrossAmount() + RoundingUpToTheNearest0Dot05(_rate * Price);
        }

        private static decimal RoundingUpToTheNearest0Dot05(decimal value)
        {
            return Math.Ceiling(value * 20) / 20;
        }
    }
}
